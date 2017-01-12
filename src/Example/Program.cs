using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DbUp.Cli;

namespace Example
{
    public class Program
    {
        private static int Main(string[] args)
        {
            return new Upgrader(args).Run();
        }
    }
}
