using CommandLine;

namespace DbUp.Cli
{
    [Verb("remote", HelpText = "Updates remote database")]
    public class RemoteOptions : BaseOptions
    {
        [Option('c', "connection-string", HelpText = "connection string", Required = true)]
        public string ConnectionStringOption { get; set; }

        public override string ConnectionString => this.ConnectionStringOption;

        public override bool EnsureDatabase => false;
    }
}