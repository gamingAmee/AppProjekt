﻿using System;

namespace AppProjekt.Models
{
    public class Telemetrics
    {
        public string Id { get; set; }
        public string Temperatur { get; set; }
        public string Humidity { get; set; }
        public DateTime Timestamp { get; set; }
    }
}