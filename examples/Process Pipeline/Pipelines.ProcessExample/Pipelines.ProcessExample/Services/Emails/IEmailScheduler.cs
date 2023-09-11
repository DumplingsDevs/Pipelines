namespace Pipelines.ProcessExample.Services.Emails;

public interface IEmailScheduler
{
    public Task Schedule(string content, CancellationToken cancellationToken);
}