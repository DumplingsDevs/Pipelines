using Pipelines.Utils;

namespace Pipelines.Tests.Utils;

public class TypeExtensionsTests
{
    [TestCase(typeof(Tuple<int>))]
    [TestCase(typeof(Tuple<int, string>))]
    [TestCase(typeof(Tuple<int, string, DateTime>))]
    [TestCase(typeof(Tuple<int, string, DateTime, Type>))]
    [TestCase(typeof(Tuple<int, string, DateTime, Type, char>))]
    [TestCase(typeof(Tuple<int, string, DateTime, Type, char, float>))]
    [TestCase(typeof(Tuple<int, string, DateTime, Type, char, float, double>))]
    public void IsTuple_ReturnsTrue_ForTupleTypes(Type tupleType)
    {
        // Act & Assert
        Assert.IsTrue(tupleType.IsTuple());
    }

    [TestCase(typeof(int))]
    [TestCase(typeof(string))]
    [TestCase(typeof(TypeExtensionsTests))] // assuming this is a non-tuple type
    public void IsTuple_ReturnsFalse_ForNonTupleTypes(Type nonTupleType)
    {
        // Act & Assert
        Assert.IsFalse(nonTupleType.IsTuple());
    }
    
    [TestCase(typeof(ValueTuple<int>))]
    [TestCase(typeof(ValueTuple<int, string>))]
    [TestCase(typeof(ValueTuple<int, string, DateTime>))]
    [TestCase(typeof(ValueTuple<int, string, DateTime, Type>))]
    [TestCase(typeof(ValueTuple<int, string, DateTime, Type, char>))]
    [TestCase(typeof(ValueTuple<int, string, DateTime, Type, char, float>))]
    [TestCase(typeof(ValueTuple<int, string, DateTime, Type, char, float, double>))]
    public void IsValueTuple_ReturnsTrue_ForTupleTypes(Type tupleType)
    {
        // Act & Assert
        Assert.IsTrue(tupleType.IsValueTuple());
    }

    [TestCase(typeof(int))]
    [TestCase(typeof(string))]
    [TestCase(typeof(TypeExtensionsTests))] // assuming this is a non-tuple type
    public void IsValueTuple_ReturnsTrue_ForTupleTypes_ReturnsFalse_ForNonTupleTypes(Type nonTupleType)
    {
        // Act & Assert
        Assert.IsFalse(nonTupleType.IsValueTuple());
    }
}