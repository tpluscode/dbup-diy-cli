﻿using System.Text.RegularExpressions;
using CommandLine;

namespace DbUp.Cli
{
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
            get { return this.DevSeedPattern.ToString(); }
            set { this.DevSeedPattern = new Regex(value, RegexOptions.IgnoreCase); }
        }

        public Regex DevSeedPattern { get; private set; }

        public abstract string ConnectionString { get; }

        public abstract bool EnsureDatabase { get; }
    }
}