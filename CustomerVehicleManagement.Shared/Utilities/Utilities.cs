﻿using System;

namespace CustomerVehicleManagement.Shared
{
    public static class Utilities
    {
        public static T ParseEnum<T>(string value)
        {
            return (T)Enum.Parse(typeof(T), value, true);
        }
    }
}