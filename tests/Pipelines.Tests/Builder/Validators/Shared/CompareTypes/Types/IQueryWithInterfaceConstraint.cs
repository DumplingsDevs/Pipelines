namespace Pipelines.Tests.Builder.Validators.Shared.CompareTypes.Types;

public interface IQueryWithInterfaceConstraint<TQueryResult> where TQueryResult : IQueryMarker
{ }