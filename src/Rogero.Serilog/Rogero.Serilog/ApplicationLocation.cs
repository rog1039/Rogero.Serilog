using System;

namespace Rogero.Serilog
{
    public static class ApplicationLocation
    {
        public static string AppBase = $@"{EnsurePathEndsWithSlash(ApplicationPath)}";
        public static string AppBaseConfig = $@"{EnsurePathEndsWithSlash(ApplicationPath)}\Config";
        public static string AppBaseConfigs = $@"{EnsurePathEndsWithSlash(ApplicationPath)}\Configs";

        public static string ApplicationPath
        {
            get
            {
                if (string.IsNullOrEmpty(AppDomain.CurrentDomain.RelativeSearchPath))
                {
                    return AppDomain.CurrentDomain.BaseDirectory; //exe folder for WinForms, Consoles, Windows Services
                }
                else
                {
                    return AppDomain.CurrentDomain.RelativeSearchPath; //bin folder for Web Apps 
                }
            }
        }

        public static string EnsurePathEndsWithSlash(string configurationDirectory)
        {
            return configurationDirectory.EndsWith(@"\")
                ? configurationDirectory
                : configurationDirectory + @"\";
        }
    }
}