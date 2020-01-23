using System;
using System.Data;
using Microsoft.EntityFrameworkCore;
using DbUp.Engine;

namespace dbup_diy_cli.EntityFramework
{
    /// <summary>
    /// A class which represents a script, which allows you to call
    /// EF before constructing the SQL command
    /// </summary>
    /// <typeparam name="TContext">The type of the context.</typeparam>
    /// <seealso cref="DbUp.Engine.IScript" />
    public abstract class EntityFrameworkScript<TContext> : IScript
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
                return ProvideScript(ctx);
            }
        }

        /// <summary>
        /// Creates the Entity Framework context.
        /// </summary>
        protected abstract TContext CreateContext(string connectionString);

        /// <summary>
        /// Provides the script to execute or empty string for a no-op.
        /// </summary>
        protected abstract string ProvideScript(TContext context);
    }
}
