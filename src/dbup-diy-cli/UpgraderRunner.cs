using System;
using System.Reflection;
using DbUp.Engine;
using DbUp.Helpers;
using Resourcer;

namespace DbUp.Cli
{
    public class UpgraderRunner
    {
        private readonly UpgradeEngine upgradeEngine;
        private readonly UpgradeEngine recreationEngine;
        private readonly UpgradeEngine procedureEngine;
        private readonly BaseOptions options;

        public UpgraderRunner(Assembly callingAssembly, BaseOptions options)
        {
            this.options = options;

            this.upgradeEngine = DeployChanges.To
                .SqlDatabase(options.ConnectionString)
                .WithScriptsEmbeddedInAssembly(callingAssembly, this.IncludeDevSeeds)
                .WithTransactionPerScript()
                .Build();

            this.procedureEngine = DeployChanges.To
                .SqlDatabase(options.ConnectionString)
                .WithScriptsEmbeddedInAssembly(callingAssembly, this.IsStoredProcedure)
                .JournalTo(new NullJournal())
                .WithTransactionPerScript()
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

        private static bool IsRecreateScript(string fileName)
        {
            return fileName.Contains("XX.RecreateDatabase.sql");
        }

        private bool IsStoredProcedure(string fileName)
        {
            var shouldIncludeScript = IsRecreateScript(fileName) == false;

            shouldIncludeScript &= this.options.StoredProcedurePattern.IsMatch(fileName) == true;

            return shouldIncludeScript;
        }

        private bool IncludeDevSeeds(string fileName)
        {
            var shouldIncludeScript = IsRecreateScript(fileName) == false;

            if (this.options.IncludeDeveloperSeeds == false)
            {
                shouldIncludeScript &= this.options.DevSeedPattern.IsMatch(fileName) == false;
            }

            return shouldIncludeScript;
        }
    }
}