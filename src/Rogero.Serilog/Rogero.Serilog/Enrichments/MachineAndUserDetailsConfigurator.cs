using System;
using System.Net;
using System.Security.Principal;
using Serilog;

namespace Rogero.Serilog.Enrichments
{
    public class MachineAndUserDetailsConfigurator : ISerilogConfigurator
    {
        public LoggerConfiguration Apply(LoggerConfiguration configuration)
        {
            configuration = SetMachineName(configuration);
            configuration = SetWindowsUserName(configuration);
            configuration = SetIpAddresses(configuration);
            return configuration;
        }

        private static LoggerConfiguration SetIpAddresses(LoggerConfiguration loggingConfig)
        {
            try
            {
                IPHostEntry ip = Dns.GetHostEntry(Dns.GetHostName());
                loggingConfig = loggingConfig.Enrich.WithProperty("IPAddresses", ip.AddressList);
            }
            catch (Exception)
            {
            }
            return loggingConfig;
        }

        private static LoggerConfiguration SetWindowsUserName(LoggerConfiguration loggingConfig)
        {
            try
            {
                var userName = WindowsIdentity.GetCurrent().Name;
                loggingConfig = loggingConfig.Enrich.WithProperty("UserName", userName);
            }
            catch (Exception)
            {
            }
            return loggingConfig;
        }

        private static LoggerConfiguration SetMachineName(LoggerConfiguration loggingConfig)
        {
            try
            {
                var machineName = Environment.MachineName;
                if (!string.IsNullOrWhiteSpace(machineName))
                {
                    loggingConfig = loggingConfig.Enrich.WithProperty("MachineName", machineName);
                }
            }
            catch (Exception)
            {
            }
            return loggingConfig;
        }
    }
}