﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
using System;
using System.Collections.Generic;

namespace Messenger.Data.Messenger
{
    public partial class Messages
    {
        public int Id { get; set; }
        public string Topic { get; set; }
        public string Message { get; set; }
        public string Sender { get; set; }
        public string Receiver { get; set; }
    }
}