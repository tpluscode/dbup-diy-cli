using dbup_diy_cli.EntityFramework;
using Example.Ef;

namespace Example.Sprint99
{
    public class EfDynamicSeed : EntityFrameworkUpdate<Context>
    {
        protected override Context CreateContext(string connectionString)
        {
            return new Context(connectionString);
        }

        protected override void PerformUpdate(Context ctx)
        {
            ctx.Tests.Add(new Test { Col1 = 1 });
        }
    }
}