using System;
using System.Collections.Generic;
using System.Reflection;
using CommandLine;
using CommandLine.Text;

namespace DbUp.Cli
{
    public sealed class Upgrader
    {
        private readonly IEnumerable<string> args;

        public Upgrader(IEnumerable<string> args)
        {
            this.args = args;
        }

        public int Run()
        {
            var callingAssembly = Assembly.GetCallingAssembly();

            return Parser.Default.ParseArguments<LocalOptions, RemoteOptions>(this.args)
                .MapResult(
                    (LocalOptions options) => UpgradeDatabase(callingAssembly, options),
                    (RemoteOptions options) => UpgradeRemote(callingAssembly, options),
                    errors => 1);
        }

        private static int UpgradeRemote(Assembly callingAssembly, RemoteOptions options)
        {
            if (string.IsNullOrWhiteSpace(options.ConnectionStringOption) ||
                string.IsNullOrWhiteSpace(options.ConnectionString))
            {
                Console.WriteLine($"Please provide either -c or -n switch. Run {System.Diagnostics.Process.GetCurrentProcess().ProcessName}.exe help for details");
                return 1;
            }

            return UpgradeDatabase(callingAssembly, options);
        }

        private static int UpgradeDatabase(Assembly callingAssembly, BaseOptions options)
        {
            bool success;
            var upgradeEngine = new DatabaseUpgrader(callingAssembly, options);

            if (options.MarkAsExecuted)
            {
                success = upgradeEngine.MarkAsExecuted();
            }
            else
            {
                success = upgradeEngine.PerformUpgrade();
            }

            return success ? 0 : 1;
        }
    }
}