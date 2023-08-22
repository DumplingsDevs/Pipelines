using System;

namespace Pipelines.Generators.Exceptions;

internal abstract class GeneratorException : Exception
{
    protected GeneratorException(string errorMessage) : base(errorMessage)
    {
    }
}