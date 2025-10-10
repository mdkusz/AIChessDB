using System.Configuration;

namespace GlobalCommonEntities.Config
{
    /// <summary>
    /// Update configuration file (app.config or web.config).
    /// </summary>
    public static class AppConfigWriter
    {
        private const string appSettings = "appSettings";
        private const string connectionStrings = "connectionStrings";
        /// <summary>
        /// Create or update an app setting in appSettings.
        /// </summary>
        public static void SetAppSetting(string key, string value, string exeConfigPath = null)
        {
            var config = GetConfig(exeConfigPath);

            // Ensure appSettings section exists
            var app = config.AppSettings ?? new AppSettingsSection();
            if (config.Sections[appSettings] == null)
                config.Sections.Add(appSettings, app);

            var settings = app.Settings;

            // If the key does not exist, add it
            if (settings[key] == null)
                settings.Add(key, value);
            else
                settings[key].Value = value;

            // Force save the section even if it appears unchanged
            app.SectionInformation.ForceSave = true;

            // Save and refresh
            config.Save(ConfigurationSaveMode.Modified);
            ConfigurationManager.RefreshSection(appSettings);
        }

        /// <summary>
        /// Create or update a connection string in connectionStrings.
        /// </summary>
        public static void SetConnectionString(
            string name,
            string connectionString,
            string providerName = null,
            string exeConfigPath = null)
        {
            var config = GetConfig(exeConfigPath);

            // Ensure connectionStrings section exists
            ConnectionStringsSection csSection;
            if (config.Sections[connectionStrings] is ConnectionStringsSection s)
            {
                csSection = s;
            }
            else
            {
                csSection = new ConnectionStringsSection();
                config.Sections.Add(connectionStrings, csSection);
            }

            // Check if the connection string already exists
            var local = csSection.ConnectionStrings[name];

            if (local == null)
            {
                var cs = new ConnectionStringSettings(name, connectionString);
                if (!string.IsNullOrEmpty(providerName))
                    cs.ProviderName = providerName;

                csSection.ConnectionStrings.Add(cs);
            }
            else
            {
                local.ConnectionString = connectionString;
                if (!string.IsNullOrEmpty(providerName))
                    local.ProviderName = providerName;
            }

            // Force save the section even if it appears unchanged
            csSection.SectionInformation.ForceSave = true;

            SaveAndRefresh(config, connectionStrings);
        }
        /// <summary>
        /// Remove a connection string in connectionStrings.
        /// </summary>
        public static void RemoveConnectionString(
            string name,
            string exeConfigPath = null)
        {
            var config = GetConfig(exeConfigPath);

            // Ensure connectionStrings section exists or exit
            ConnectionStringsSection csSection;
            if (config.Sections[connectionStrings] is ConnectionStringsSection s)
            {
                csSection = s;
            }
            else
            {
                return;
            }

            // Check if the connection string already exists
            var local = csSection.ConnectionStrings[name];

            if (local == null)
            {
                return;
            }
            else
            {
                csSection.ConnectionStrings.Remove(local);
            }

            // Force save the section even if it appears unchanged
            csSection.SectionInformation.ForceSave = true;

            SaveAndRefresh(config, connectionStrings);
        }

        // ---------- helpers ----------
        private static Configuration GetConfig(string exeConfigPath)
        {
            if (!string.IsNullOrEmpty(exeConfigPath))
            {
                var map = new ExeConfigurationFileMap { ExeConfigFilename = exeConfigPath };
                return ConfigurationManager.OpenMappedExeConfiguration(map, ConfigurationUserLevel.None);
            }
            return ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
        }

        private static void SaveAndRefresh(Configuration config, string sectionName)
        {
            config.Save(ConfigurationSaveMode.Modified);
            ConfigurationManager.RefreshSection(sectionName);
        }
    }
}
