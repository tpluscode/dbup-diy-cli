using System.Linq;
using dbup_diy_cli.EntityFramework;
using Example.Ef;

namespace Example.Sprint99
{
    public class EfDynamicScript : EntityFrameworkScript<Context>
    {
        protected override Context CreateContext(string connectionString)
        {
            return new Context(connectionString);
        }

        protected override string ProvideScript(Context context)
        {
            if (context.Tests.Any(t => t.Col1 == 2) == false)
            {
                return "INSERT INTO Test VALUES (2)";
            }

            return string.Empty;
        }
    }
}