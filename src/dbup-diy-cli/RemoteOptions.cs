using System.Configuration;
using CommandLine;

namespace DbUp.Cli
{
    [Verb("remote", HelpText = "Updates remote database")]
    public class RemoteOptions : BaseOptions
    {
        [Option('c', "connection-string", HelpText = "connection string", SetName = "connection")]
        public string ConnectionStringOption { get; set; }

        [Option('n', "connection-string-name", HelpText = "connection string name", SetName = "connection")]
        public string ConnectionStringName { get; set; }

        public override string ConnectionString
        {
            get
            {
                if (string.IsNullOrWhiteSpace(this.ConnectionStringOption) == false)
                {
                    return this.ConnectionStringOption;
                }

                return ConfigurationManager.ConnectionStrings[this.ConnectionStringName].ConnectionString;
            }
        }

        public override bool EnsureDatabase => false;
    }
}