using System;

namespace Pipelines.Generators.Exceptions;

public abstract class GeneratorException : Exception
{
    protected GeneratorException(string errorMessage) : base(errorMessage)
    {
    }
}