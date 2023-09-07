---
name: Feature request
about: Create a feature request
title: "[BUG]"
labels: ""
assignees: ""
---

**Describe the bug**
A clear and concise description of the new feature or enhancement you are proposing.

**Pipelines configuration**
Please provide the Pipeline configuration that is either intended for use or is being requested for support.

- Builder

```
services.AddPipeline()
    .AddInput(typeof(Types.IRequest<>))
    .AddHandler((typeof(Types.IRequestHandler<,>)), executingAssembly)
    .AddDispatcher<IRequestDispatcher>(executingAssembly)
    .Build();
```

- Input

```
public interface IInput<TResult>{ }
```

- Handler

```
public interface IHandler<in TInput, TResult> where TInput : IInput<TResult>
{
    public Task<TResult> HandleAsync(TInput input, CancellationToken token);
}
```

- Dispatcher

```
public interface IDispatcher
{
    public Task<TResult> SendAsync<TResult>(IInput<TResult> input, CancellationToken token);
}
```

**Use Case**
Describe a specific use case or scenario where this feature would be applicable or helpful.

**Additional Information**
Include any additional context, information, or references that may be relevant to this feature request.
