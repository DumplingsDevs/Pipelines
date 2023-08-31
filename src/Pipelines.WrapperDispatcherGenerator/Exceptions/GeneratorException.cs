using System;

namespace Pipelines.WrapperDispatcherGenerator.Exceptions;

public abstract class GeneratorException : Exception
{
    protected GeneratorException(string errorMessage) : base(errorMessage)
    {
    }
}