using Pipelines.Tests.Utils.Types;
using Pipelines.Utils;

namespace Pipelines.Tests.Utils;

public class MethodInfoExtensionsTests
{
    [Test]
    public void GetReturnTypes_TaskString_ReturnsStringType()
    {
        var method = GetType().GetMethod("DummyMethod1");

        var returnTypes = method.GetReturnTypes();

        Assert.That(returnTypes, Is.EqualTo(new List<Type> { typeof(string) }));
    }

    [Test]
    public void GetReturnTypes_Tuple_ReturnsIntAndStringTypes()
    {
        var method = GetType().GetMethod("DummyMethod2");

        var returnTypes = method.GetReturnTypes();

        Assert.That(returnTypes, Is.EqualTo(new List<Type> { typeof(int), typeof(string) }));
    }

    [Test]
    public void GetReturnTypes_ValueTuple_ReturnsIntAndStringTypes()
    {
        var method = GetType().GetMethod("DummyMethod3");

        var returnTypes = method.GetReturnTypes();

        Assert.That(returnTypes, Is.EqualTo(new List<Type> { typeof(int), typeof(string) }));
    }

    [Test]
    public void GetReturnTypes_Int_ReturnsIntType()
    {
        var method = GetType().GetMethod("DummyMethod4");

        var returnTypes = method.GetReturnTypes();

        Assert.That(returnTypes, Is.EqualTo(new List<Type> { typeof(int) }));
    }

    [Test]
    public void GetReturnTypes_GenericTuple_ReturnsGenericTypes()
    {
        var method = GetType().GetMethod("DummyMethodGenericTuple");

        var returnTypes = method.GetReturnTypes();

        Assert.That(returnTypes.Select(x => x.Name), Is.EqualTo(new List<string> { "T1", "T2" }));
        Assert.True(returnTypes.Select(x => x.GetGenericParameterConstraints().Length).All(x => x == 0));
    }

    [Test]
    public void GetReturnTypes_GenericTask_ReturnsGenericType()
    {
        var method = GetType().GetMethod("DummyMethodGenericTask");

        var returnTypes = method.GetReturnTypes();

        Assert.That(returnTypes.Select(x => x.Name), Is.EqualTo(new List<string> { "T" }));
        Assert.True(returnTypes.Select(x => x.GetGenericParameterConstraints().Length).All(x => x == 0));
    }

    [Test]
    public void GetReturnTypes_GenericTaskTuple_ReturnsGenericTypes()
    {
        var method = GetType().GetMethod("DummyMethodGenericTaskTuple");

        var returnTypes = method.GetReturnTypes();

        Assert.That(returnTypes.Select(x => x.Name), Is.EqualTo(new List<string> { "T1", "T2" }));
        Assert.True(returnTypes.Select(x => x.GetGenericParameterConstraints().Length).All(x => x == 0));
    }

    [Test]
    public void GetReturnTypes_GenericTaskTupleWithConstraints_ReturnsGenericTypes()
    {
        var method = GetType().GetMethod("DummyMethodGenericTaskTupleWithConstraints");

        var returnTypes = method.GetReturnTypes();

        Assert.That(returnTypes.Select(x => x.Name), Is.EqualTo(new List<string> { "T1", "T2" }));
        foreach (var constraintTypes in returnTypes.Select(x => x.GetGenericParameterConstraints()))
        {
            Assert.True(TypeNamespaceComparer.Compare(constraintTypes.First(), typeof(ICommandInterface)));
        }
    }

    public async Task<string> DummyMethod1()
    {
        return await Task.FromResult("Test");
    }

    public Tuple<int, string> DummyMethod2()
    {
        return new Tuple<int, string>(1, "Test");
    }

    public (int, string) DummyMethod3()
    {
        return (1, "Test");
    }

    public int DummyMethod4()
    {
        return 1;
    }

    public Tuple<T1, T2> DummyMethodGenericTuple<T1, T2>(T1 item1, T2 item2)
    {
        return new Tuple<T1, T2>(item1, item2);
    }

    public async Task<T> DummyMethodGenericTask<T>(T result)
    {
        return await Task.FromResult(result);
    }

    public async Task<Tuple<T1, T2>> DummyMethodGenericTaskTuple<T1, T2>(T1 item1, T2 item2)
    {
        return await Task.FromResult(new Tuple<T1, T2>(item1, item2));
    }

    public async Task<Tuple<T1, T2>> DummyMethodGenericTaskTupleWithConstraints<T1, T2>(T1 item1, T2 item2)
        where T1 : ICommandInterface where T2 : ICommandInterface
    {
        return await Task.FromResult(new Tuple<T1, T2>(item1, item2));
    }
}