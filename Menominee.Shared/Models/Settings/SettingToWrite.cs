﻿using Menominee.Domain.Entities.Settings;

namespace Menominee.Shared.Models.Settings
{
    public class SettingToWrite
    {
        public long Id { get; set; }
        public SettingName SettingName { get; set; }
        public string SettingValue { get; set; }
        public SettingGroup SettingGroup { get; set; }
    }
}