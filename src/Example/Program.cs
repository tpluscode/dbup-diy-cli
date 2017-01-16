using DbUp.Cli;

namespace Example
{
    public class Program
    {
        private static int Main(string[] args)
        {
            return new Upgrader(args).Run(SqlServerExtensions.SqlDatabase);
        }
    }
}
