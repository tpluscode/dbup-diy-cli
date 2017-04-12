using DbUp.Builder;

namespace DbUp.Cli
{
    internal static class UpgradeEngineBuilderExtensions
    {
        internal static UpgradeEngineBuilder SetTimeout(this UpgradeEngineBuilder builder, int? commandExecutionTimeoutSeconds)
        {
            if (commandExecutionTimeoutSeconds.HasValue)
            {
                builder.Configure(configuration =>
                {
                    configuration.ScriptExecutor.ExecutionTimeoutSeconds = commandExecutionTimeoutSeconds;
                });
            }

            return builder;
        }
    }
}