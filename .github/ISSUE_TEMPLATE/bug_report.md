---
name: Bug report
about: Create a report to help us improve
title: "[BUG]"
labels: ''
assignees: ''

---

**Describe the bug**
A clear and concise description of what the bug is.

**Pipelines configuration**
Please provide configuration of Pipeline:

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

**Expected behavior**
A clear and concise description of what you expected to happen.

**Additional context**
Add any other context about the problem here.
