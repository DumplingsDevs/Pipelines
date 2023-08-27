# Conventions

- Generic result types must have a `class` constraint.
- The `Input` must be the first parameter of the Dispatcher and Handler methods.
- Result types for the `Dispatcher` and `Handler` must match.
- Method parameters for the `Dispatcher` and `Handler` must match.
- `Input` Generic Arguments indicate the result type.
- If the `Dispatcher/Handler` returns a non-generic type, then the `Input` will not have any Generic Arguments.
- The `Decorator` must implement the Handler interface and accept Handler as a constructor parameter.