using System.Text;

namespace Pipelines.WrapperDispatcherGenerator.Builders;

internal class ExceptionsBuilder
{
    private readonly StringBuilder _builder = new();

    public string Build()
    {
        BuildExceptions();

        return _builder.ToString();
    }
    
    private void BuildExceptions()
    {
        _builder.AppendLine(@"
using System;

internal class CannotCreateDispatcherWrapperException : Exception
{
    private const string ErrorMessage = ""Could not create wrapper type for {0}. Please create issue on https://github.com/DumplingsDevs/Pipelines"";

    internal CannotCreateDispatcherWrapperException(Type type) : base(string.Format(ErrorMessage, type))
    {
        
    }
}");
    }
}
