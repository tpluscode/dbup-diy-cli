using System.Data.SqlClient;
using CommandLine;
using NullGuard;

namespace DbUp.Cli
{
    [NullGuard(ValidationFlags.ReturnValues)]
    [Verb("local", HelpText = "Upgrades local database")]
    public class LocalOptions : BaseOptions
    {
        [Option('d', "database", Required = true, HelpText = "Database to upgrade")]
        public string DatabaseName { get; set; }

        [Option('s', "server", Required = true, HelpText = "Server name/address")]
        public string Server { get; set; }

        public override string ConnectionString
        {
            get
            {
                var sqlConnectionStringBuilder = new SqlConnectionStringBuilder
                {
                    IntegratedSecurity = true,
                    DataSource = this.Server,
                    ConnectTimeout = 120
                };

                if (string.IsNullOrWhiteSpace(this.DatabaseName) == false)
                {
                    sqlConnectionStringBuilder.InitialCatalog = this.DatabaseName;
                }

                return sqlConnectionStringBuilder.ConnectionString;
            }
        }

        public override bool EnsureDatabase => true;
    }
}