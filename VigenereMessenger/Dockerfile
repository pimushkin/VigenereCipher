FROM mcr.microsoft.com/dotnet/core/sdk:latest
WORKDIR /src
COPY ["VigenereMessenger.csproj", "VigenereMessenger/"]
RUN dotnet restore "VigenereMessenger/VigenereMessenger.csproj"
WORKDIR "/src/VigenereMessenger"
COPY . .
RUN dotnet build "VigenereMessenger.csproj" -c Release -o /app/build

ENV ASPNETCORE_URLS http://*:80

RUN chmod +x ./entrypoint.sh
CMD /bin/bash ./entrypoint.sh