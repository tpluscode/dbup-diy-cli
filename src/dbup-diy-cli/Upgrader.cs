using System.Collections.Generic;
using System.Reflection;
using CommandLine;

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
                    (RemoteOptions options) => UpgradeDatabase(callingAssembly, options),
                    errors => 1);
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