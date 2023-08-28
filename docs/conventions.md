# Conventions

- Generic result types must have a `class` constraint.

<details>
<summary style="color: green">📜 Show me example </summary>

```cs
public interface IInput<TResult> where TResult: class{ } 

public interface IHandler<in TInput, TResult> where TInput : IInput<TResult> where TResult: class
{
    public Task<TResult> HandleAsync(TInput input, CancellationToken token);
}

public interface IDispatcher
{
    public Task<TResult> SendAsync<TResult>(IInput<TResult> input, CancellationToken token) where TResult : class;
}
```

</details>

- The `Input` must be the first parameter of the Dispatcher and Handler methods.

<details>
<summary style="color: green">📜 Show me example </summary>

```cs
public interface IInput<TResult> where TResult: class{ } 

public interface IHandler<in TInput, TResult> where TInput : IInput<TResult> where TResult: class
{
    public Task<TResult> HandleAsync(TInput input, CancellationToken token);
}

public interface IDispatcher
{
    public Task<TResult> SendAsync<TResult>(IInput<TResult> input, CancellationToken token) where TResult : class;
}
```

</details>

- Result types for the `Dispatcher` and `Handler` must match. If `generic arguments` defined in `Input`,  they must also align with those in the `Dispatcher` and `Handler` 

<details>
<summary style="color: green">📜 Show me example 1 </summary>

```cs

public interface IInput<TResult> where TResult: class{ } 

public interface IHandler<in TInput, TResult> where TInput : IInput<TResult> where TResult: class
{
    public Task<TResult> HandleAsync(TInput input, CancellationToken token);
}

public interface IDispatcher
{
    public Task<TResult> SendAsync<TResult>(IInput<TResult> input, CancellationToken token) where TResult : class;
}
```

</details>

<details>
<summary style="color: green">📜 Show me example 2 </summary>

```cs
public interface IInput<TResult, TResult2> where TResult : class where TResult2 : class { } 

public interface IHandler<in TInput, TResult, TResult2>
    where TInput : IInput<TResult, TResult2> where TResult : class where TResult2 : class
{
    public Task<(TResult, TResult2)> HandleAsync(TInput input, CancellationToken token);
}

public interface IDispatcher
{
    public Task<(TResult, TResult2)> SendAsync<TResult, TResult2>(IInput<TResult, TResult2> input,
        CancellationToken token) where TResult : class where TResult2 : class;
}
```

</details>

<details>
<summary style="color: green">📜 Show me example 3 </summary>

```cs
public interface IInput { }

public interface IHandler<in TInput> where TInput : IInput
{
    public Task HandleAsync(TInput command, CancellationToken token);
}

public interface IDispatcher
{
    public Task SendAsync(IInput input, CancellationToken token);
}
```

</details>

<details>
<summary style="color: green">📜 Show me example 4 </summary>

```cs
public interface IInput { }

public interface IHandler<in TInput> where TInput : IInput
{
    public Task<string> HandleAsync(TInput command, CancellationToken token);
}

public interface IDispatcher
{
    public Task<string> SendAsync(IInput inputWithResult, CancellationToken token);
}
```

</details>

- Method parameters for the `Dispatcher` and `Handler` must match.

```cs
TO DO CODE SAMPLE
```

- `Input` Generic Arguments indicate the result type.

```cs
TO DO CODE SAMPLE
```

- If the `Dispatcher/Handler` returns a non-generic type, then the `Input` will not have any Generic Arguments.

```cs
TO DO CODE SAMPLE
```

- The `Decorator` must implement the Handler interface and accept Handler as a constructor parameter.

