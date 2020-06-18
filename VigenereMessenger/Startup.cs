using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using MatBlazor;
using Messenger.Data.Messenger;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using VigenereMessenger.Areas.Identity;
using VigenereMessenger.Data;

namespace VigenereMessenger
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            if (!(string.IsNullOrWhiteSpace(Environment.GetEnvironmentVariable("GOOGLE_CLIENT_ID")) || 
                  string.IsNullOrWhiteSpace(Environment.GetEnvironmentVariable("GOOGLE_CLIENT_SECRET"))))
            {
                services.AddAuthentication().AddGoogle(googleOptions =>
                {
                    googleOptions.ClientId = Environment.GetEnvironmentVariable("GOOGLE_CLIENT_ID");
                    googleOptions.ClientSecret = Environment.GetEnvironmentVariable("GOOGLE_CLIENT_SECRET");
                });
            }
            else if (!(string.IsNullOrWhiteSpace(Configuration["GoogleClientId"]) ||
                       string.IsNullOrWhiteSpace(Configuration["GoogleClientSecret"])))
            {
                services.AddAuthentication().AddGoogle(googleOptions =>
                {
                    googleOptions.ClientId = Configuration["GoogleClientId"];
                    googleOptions.ClientSecret = Configuration["GoogleClientSecret"];
                });
            }

            var connectionStringBuilder = new SqlConnectionStringBuilder();
            if (!(string.IsNullOrWhiteSpace(Environment.GetEnvironmentVariable("DB_SERVER")) || 
                  string.IsNullOrWhiteSpace(Environment.GetEnvironmentVariable("DB_NAME")) || 
                  string.IsNullOrWhiteSpace(Environment.GetEnvironmentVariable("DB_USER_ID")) || 
                  string.IsNullOrWhiteSpace(Environment.GetEnvironmentVariable("DB_USER_PASSWORD"))))
            {
                connectionStringBuilder = new SqlConnectionStringBuilder
                {
                    DataSource = Environment.GetEnvironmentVariable("DB_SERVER"),
                    InitialCatalog = Environment.GetEnvironmentVariable("DB_NAME"),
                    UserID = Environment.GetEnvironmentVariable("DB_USER_ID"),
                    Password = Environment.GetEnvironmentVariable("DB_USER_PASSWORD"),
                    MultipleActiveResultSets = true
                };
            }
            else
            {
                connectionStringBuilder.ConnectionString = Configuration.GetConnectionString("DefaultConnection");
            }

            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(connectionStringBuilder.ConnectionString));
            services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = false)
                .AddEntityFrameworkStores<ApplicationDbContext>();
            services.AddRazorPages();
            services.AddServerSideBlazor();
            services.AddScoped<AuthenticationStateProvider, RevalidatingIdentityAuthenticationStateProvider<IdentityUser>>();
            services.AddMatToaster(config =>
            {
                config.Position = MatToastPosition.BottomRight;
                config.PreventDuplicates = false;
                config.NewestOnTop = true;
                config.ShowCloseButton = true;
                config.MaximumOpacity = 95;
                config.VisibleStateDuration = 3000;
            });
            services.AddSingleton<CipherService>();
            services.AddSingleton<DocumentService>();
            services.AddScoped<MessageService>();
            services.AddScoped<UserService>();
            services.AddScoped<HttpClient>();
            services.AddDbContext<MessengerContext>(options =>
                options.UseSqlServer(connectionStringBuilder.ConnectionString));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapBlazorHub();
                endpoints.MapFallbackToPage("/_Host");
            });
        }
    }
}
