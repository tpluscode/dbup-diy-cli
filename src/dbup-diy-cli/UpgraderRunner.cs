using System;
using System.Reflection;
using DbUp.Builder;
using DbUp.Engine;
using DbUp.Helpers;
using Resourcer;

namespace DbUp.Cli
{
    public delegate UpgradeEngineBuilder SetTargetDatabase(SupportedDatabases s, string connectionString);

    public class UpgraderRunner
    {
        private readonly UpgradeEngine upgradeEngine;
        private readonly UpgradeEngine recreationEngine;
        private readonly UpgradeEngine procedureEngine;
        private readonly BaseOptions options;

        public UpgraderRunner(Assembly callingAssembly, BaseOptions options, SetTargetDatabase setTargetDatabase)
        {
            this.options = options;

            this.upgradeEngine = setTargetDatabase(DeployChanges.To, options.ConnectionString)
                .WithScriptsAndCodeEmbeddedInAssembly(callingAssembly, this.IncludeDevSeeds)
                .SetTimeout(options.CommandExecutionTimeoutSeconds)
                .WithTransactionPerScript()
                .LogToConsole()
                .Build();

            this.procedureEngine = setTargetDatabase(DeployChanges.To, options.ConnectionString)
                .WithScriptsEmbeddedInAssembly(callingAssembly, this.ShouldAlwaysExecute)
                .SetTimeout(options.CommandExecutionTimeoutSeconds)
                .JournalTo(new NullJournal())
                .WithTransactionPerScript()
                .LogToConsole()
                .Build();

            this.recreationEngine = setTargetDatabase(DeployChanges.To, options.ConnectionString)
                .WithScript("Database recreated at " + DateTime.Now, Resource.AsString("Scripts.XX.RecreateDatabase.sql"))
                .SetTimeout(options.CommandExecutionTimeoutSeconds)
                .WithTransactionPerScript()
                .LogToConsole()
                .Build();
        }

        public bool PerformUpgrade()
        {
            var success = true;

            if (this.options.EnsureDatabase)
            {
                EnsureDatabase.For.SqlDatabase(this.options.ConnectionString);
            }

            if (this.options.RecreateDatabase)
            {
                success = this.recreationEngine.PerformUpgrade().Successful;
            }

            return success
                && this.upgradeEngine.PerformUpgrade().Successful
                && this.procedureEngine.PerformUpgrade().Successful;
        }

        public bool MarkAsExecuted()
        {
            if (this.options.EnsureDatabase)
            {
                EnsureDatabase.For.SqlDatabase(this.options.ConnectionString);
            }

            return this.upgradeEngine.MarkAsExecuted().Successful;
        }

        private bool ShouldAlwaysExecute(string fileName)
        {
            return this.options.RunAlwaysPattern.IsMatch(fileName);
        }

        private bool IncludeDevSeeds(string fileName)
        {
            var shouldIncludeScript = this.ShouldAlwaysExecute(fileName) == false;

            if (this.options.IncludeDeveloperSeeds == false)
            {
                shouldIncludeScript &= this.options.DevSeedPattern.IsMatch(fileName) == false;
            }

            return shouldIncludeScript;
        }
    }
}