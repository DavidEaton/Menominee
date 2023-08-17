using Menominee.Api.Common;
using Menominee.Domain.Entities.Settings;
using Menominee.Shared.Models.Settings;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Menominee.Api.Settings
{
    public class SettingsController : BaseApplicationController<SettingsController>
    {
        private readonly ISettingsRepository repository;
        public SettingsController(ISettingsRepository repository, ILogger<SettingsController> logger) : base(logger)
        {
            this.repository = repository ??
                throw new ArgumentNullException(nameof(repository));
        }

        /// <summary>
        /// Get a single setting by its name
        /// </summary>
        /// <returns></returns>
        [HttpGet("{settingName}")]
        public async Task<ActionResult<SettingToRead>> GetSetting(SettingName settingName)
        {
            var setting = await repository.GetSetting(settingName);

            if (setting is null)
                return NotFound();

            return Ok(setting);
        }
        /// <summary>
        /// Gets all of the settings in the database
        /// </summary>
        /// <returns>list of settingToRead</returns>
        [HttpGet]
        public async Task<ActionResult<IReadOnlyList<SettingToRead>>> GetSettingsListAsync()
        {
            var settings = await repository.GetSettingsListAsync();

            return settings.Any()
                ? Ok(settings)
                : Ok();
        }

        /// <summary>
        /// Gets all setting in the database with that groupId
        /// </summary>
        /// <param name="groupId">settingGroup</param>
        /// <returns>list of settingToRead with associated groupId</returns>
        [HttpGet("group/{group}")]
        public async Task<ActionResult<IReadOnlyList<SettingToRead>>> GetSettingListByGroupAsync(SettingGroup group)
        {
            var settings = await repository.GetSettingListByGroupAsync(group);

            return settings.Any()
                ? Ok(settings)
                : Ok();
        }

        /// <summary>
        /// Saves a list of settings to the database
        /// </summary>
        /// <param name="settings">list of settings to be added to database</param>
        /// <returns>All of the settings the database</returns>
        [HttpPost("settingList")]
        public async Task<ActionResult<IReadOnlyList<SettingToRead>>> SaveSettingsListAsync(List<SettingToWrite> settings)
        {
            var updateSettings = await repository.SaveSettingsListAsync(settings);

            if (!updateSettings.Any())
                return BadRequest();

            return Created("Settings Created",updateSettings);
        }

        /// <summary>
        /// Bulk updates a list of settings
        /// </summary>
        /// <param name="settings">List of settings to update</param>
        /// <returns>List of all of the settings in the database after update</returns>
        [HttpPut("settingList")]
        public async Task<ActionResult<IReadOnlyList<SettingToWrite>>> UpdateSettingsListAsync(List<SettingToWrite> settings)
        {
            var updatedSettings = await repository.UpdateSettingsListAsync(settings);

            if (!updatedSettings.Any())
                return BadRequest();

            return Ok(updatedSettings);
        }

        /// <summary>
        /// Save a single setting to the database
        /// </summary>
        /// <param name="setting"></param>
        /// <returns>a single SettingToRead dto of the newly createde setting</returns>
        [HttpPost]
        public async Task<ActionResult<SettingToRead>> SaveSetting(SettingToWrite setting)
        {
            var updateSetting = await repository.SaveSetting(setting);

            if (updateSetting is null)
                return BadRequest();

            return Created("Setting Created", updateSetting);
        }

        /// <summary>
        /// Update a single setting in the database using the SettingName passed in the SettingToWrite dto
        /// </summary>
        /// <param name="setting"></param>
        /// <returns>a single SettingToRead dto of the updated settings</returns>
        [HttpPut]
        public async Task<ActionResult<SettingToRead>> UpdateSetting(SettingToWrite setting)
        {
            var updateSetting = await repository.UpdateSetting(setting);

            if(updateSetting is null)
                return BadRequest();

            return Ok(updateSetting);
        }

    }
}
