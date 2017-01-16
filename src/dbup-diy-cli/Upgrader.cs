using System;
using System.Collections.Generic;
using System.Reflection;
using CommandLine;
using CommandLine.Text;
using DbUp.Builder;

namespace DbUp.Cli
{
    public sealed class Upgrader
    {
        private readonly IEnumerable<string> args;

        public Upgrader(IEnumerable<string> args)
        {
            this.args = args;
        }

        public int Run(SetTargetDatabase targetDbFunc)
        {
            var callingAssembly = Assembly.GetCallingAssembly();

            var parserResult = Parser.Default.ParseArguments<LocalOptions, RemoteOptions>(this.args);
            return parserResult
                .MapResult(
                    (LocalOptions options) => UpgradeDatabase(callingAssembly, options, targetDbFunc),
                    (RemoteOptions options) => UpgradeRemote(callingAssembly, options, targetDbFunc, parserResult),
                    errors => 1);
        }

        private static int UpgradeRemote(Assembly callingAssembly, RemoteOptions options, SetTargetDatabase setTarget, ParserResult<object> result)
        {
            if (string.IsNullOrWhiteSpace(options.ConnectionStringOption) &&
                string.IsNullOrWhiteSpace(options.ConnectionStringName))
            {
                Console.WriteLine(HelpText.AutoBuild(
                    result,
                    text => text.AddPreOptionsText("Please provide either -c or -n switch"),
                    example => example,
                    true));

                return 1;
            }

            return UpgradeDatabase(callingAssembly, options, setTarget);
        }

        private static int UpgradeDatabase(Assembly callingAssembly, BaseOptions options, SetTargetDatabase setTarget)
        {
            bool success;
            var upgradeEngine = new UpgraderRunner(callingAssembly, options, setTarget);

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