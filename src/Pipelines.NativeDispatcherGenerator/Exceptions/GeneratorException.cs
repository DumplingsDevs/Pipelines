using System;

namespace Pipelines.NativeDispatcherGenerator.Exceptions;

public abstract class GeneratorException : Exception
{
    protected GeneratorException(string errorMessage) : base(errorMessage)
    {
    }
}