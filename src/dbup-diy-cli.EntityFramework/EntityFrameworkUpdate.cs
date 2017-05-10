using System;
using System.Data;
using System.Data.Entity;
using DbUp.Engine;

namespace dbup_diy_cli.EntityFramework
{
    /// <summary>
    /// A class which represents a script, which allows you to use
    /// Entity Framework to update the database
    /// </summary>
    /// <typeparam name="TContext">The type of the context.</typeparam>
    /// <seealso cref="DbUp.Engine.IScript" />
    public abstract class EntityFrameworkUpdate<TContext> : IScript
        where TContext : DbContext
    {
        /// <summary>
        /// Provides the Sql Script to execute
        /// </summary>
        /// <param name="dbCommandFactory">A factory to create open and active database commands</param>
        /// <returns>
        /// The Sql Script contents
        /// </returns>
        public string ProvideScript(Func<IDbCommand> dbCommandFactory)
        {
            var connectionString = dbCommandFactory().Connection.ConnectionString;
            using (var ctx = CreateContext(connectionString))
            {
                PerformUpdate(ctx);

                ctx.SaveChanges();
            }

            return string.Empty;
        }

        /// <summary>
        /// Creates the context.
        /// </summary>
        protected abstract TContext CreateContext(string connectionString);

        /// <summary>
        /// Performs the update. No need to call SaveChanges
        /// </summary>
        protected abstract void PerformUpdate(TContext context);
    }
}