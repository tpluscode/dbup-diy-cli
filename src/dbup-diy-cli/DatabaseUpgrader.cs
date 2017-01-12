using System;
using System.Reflection;
using DbUp.Engine;
using Resourcer;

namespace DbUp.Cli
{
    public class DatabaseUpgrader
    {
        private readonly UpgradeEngine upgradeEngine;
        private readonly UpgradeEngine recreationEngine;
        private readonly BaseOptions options;

        public DatabaseUpgrader(Assembly callingAssembly, BaseOptions options)
        {
            this.options = options;

            this.upgradeEngine = DeployChanges.To
                .SqlDatabase(options.ConnectionString)
                .WithScriptsEmbeddedInAssembly(callingAssembly, this.IncludeDevSeeds)
                .WithTransactionPerScript()
                .LogToConsole()
                .Build();

            this.recreationEngine = DeployChanges.To
                .SqlDatabase(options.ConnectionString)
                .WithScript("Database recreated at " + DateTime.Now, Resource.AsString("Scripts.XX.RecreateDatabase.sql"))
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

            return success && this.upgradeEngine.PerformUpgrade().Successful;
        }

        public bool MarkAsExecuted()
        {
            if (this.options.EnsureDatabase)
            {
                EnsureDatabase.For.SqlDatabase(this.options.ConnectionString);
            }

            return this.upgradeEngine.MarkAsExecuted().Successful;
        }

        private bool IncludeDevSeeds(string fileName)
        {
            var shouldIncludeScript = fileName.Contains("XX.RecreateDatabase.sql") == false;

            if (this.options.IncludeDeveloperSeeds == false)
            {
                shouldIncludeScript &= this.options.DevSeedPattern.IsMatch(fileName) == false;
            }

            return shouldIncludeScript;
        }
    }
}