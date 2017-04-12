using System.Text.RegularExpressions;
using CommandLine;
using NullGuard;

namespace DbUp.Cli
{
    [NullGuard(ValidationFlags.ReturnValues)]
    public abstract class BaseOptions
    {
        [Option("dev-seeds", HelpText = "Seed database with sample data")]
        public bool IncludeDeveloperSeeds { get; set; }

        [Option('r', "recreate", HelpText = "Completely recreate all database objects")]
        public bool RecreateDatabase { get; set; }

        [Option('m', "mark-as-executed", HelpText = "Don't run ever run current migration scripts")]
        public bool MarkAsExecuted { get; set; }

        [Option("dev-seed-pattern", Default = "_dev_", HelpText = "Regular expression to match and select developer seed script (case-insensitive)")]
        public string DevSeedPatternString
        {
            get { return this.DevSeedPattern?.ToString(); }
            set { this.DevSeedPattern = new Regex(value, RegexOptions.IgnoreCase); }
        }

        [Option("run-always-pattern", Default = "(sp|fn)_", HelpText = "Regular expression to match and scripts executed every time, such as stored procedures and functions (case-insensitive)")]
        public string RunAlwaysPatternString
        {
            get { return this.RunAlwaysPattern?.ToString(); }
            set { this.RunAlwaysPattern = new Regex(value, RegexOptions.IgnoreCase); }
        }

        public Regex DevSeedPattern { get; private set; }

        public Regex RunAlwaysPattern { get; private set; }

        [Option('t', "timeout", HelpText = "Timeout for each script in seconds")]
        public int? CommandExecutionTimeoutSeconds { get; set; }

        public abstract string ConnectionString { get; }

        public abstract bool EnsureDatabase { get; }
    }
}
