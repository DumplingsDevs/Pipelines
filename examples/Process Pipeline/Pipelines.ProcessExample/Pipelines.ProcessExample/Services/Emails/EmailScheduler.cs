namespace Pipelines.ProcessExample.Services.Emails;

public class EmailScheduler : IEmailScheduler
{
    public Task Schedule(string content, CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }
}