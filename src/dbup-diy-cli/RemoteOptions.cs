using CommandLine;
using NullGuard;

namespace DbUp.Cli
{
    [NullGuard(ValidationFlags.ReturnValues)]
    [Verb("remote", HelpText = "Updates remote database")]
    public class RemoteOptions : BaseOptions
    {
        [Option('c', "connection-string", HelpText = "connection string", SetName = "connection")]
        public string ConnectionStringOption { get; set; }

        [Option('n', "connection-string-name", HelpText = "connection string name", SetName = "connection")]
        public string ConnectionStringName { get; set; }

        public override string ConnectionString => this.ConnectionStringOption;

        public override bool EnsureDatabase => false;
    }
}