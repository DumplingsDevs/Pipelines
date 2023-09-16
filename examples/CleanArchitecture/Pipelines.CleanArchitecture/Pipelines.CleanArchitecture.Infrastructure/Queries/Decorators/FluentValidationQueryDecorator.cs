using System.Reflection;
using FluentValidation.Results;
using Pipelines.CleanArchitecture.Abstractions.Errors;
using Pipelines.CleanArchitecture.Abstractions.Queries;
using Pipelines.CleanArchitecture.Abstractions.Queries.Exceptions;
using Pipelines.CleanArchitecture.Infrastructure.Utils;
using Pipelines.CleanArchitecture.Infrastructure.Utils.Exceptions;

namespace Pipelines.CleanArchitecture.Infrastructure.Queries.Decorators;

internal class FluentValidationQueryDecorator<TQuery, TResult> : IQueryHandler<TQuery, TResult>
    where TQuery : IQuery<TResult>
{
    private const string QUERY_VALIDATE_METHOD_NAME = "ValidateAsync";

    private readonly IServiceProvider _serviceProvider;
    private readonly IQueryHandler<TQuery, TResult> _queryHandler;

    public FluentValidationQueryDecorator(IServiceProvider serviceProvider, IQueryHandler<TQuery, TResult> queryHandler)
    {
        _serviceProvider = serviceProvider;
        _queryHandler = queryHandler;
    }

    public async Task<TResult> HandleAsync(TQuery query, CancellationToken token)
    {
        var queryType = query.GetType();
        var queryName = queryType.Name;

        var validationResult = await ValidateQuery(_serviceProvider, queryType, queryName, query,
            token);

        if (!validationResult.Errors.Any())
        {
            return await _queryHandler.HandleAsync(query,token).ConfigureAwait(false);
        }

        var errorDetails = validationResult.Errors
            .Select(x => new DetailedErrorDescription(x.ErrorCode, x.ErrorMessage, x.PropertyName)).ToList();
            
        throw new QueryValidationFailedException(errorDetails);

    }

    private async Task<ValidationResult> ValidateQuery(IServiceProvider provider, Type queryType,
        string queryName, TQuery query, CancellationToken cancellationToken)
    {
        var validator = ValidatorGetter.GetFluentQueryValidator(provider, queryType, queryName);
        if (validator == null)
        {
            return new ValidationResult();
        }

        MethodInfo methodInfo = MethodInfoGetter.GetByName(validator, QUERY_VALIDATE_METHOD_NAME);

        if (methodInfo == null)
        {
            throw new MethodNotFoundInGenericTypeException(queryName, QUERY_VALIDATE_METHOD_NAME);
        }


        ValidationResult result =
            await MethodExecutor.InvokeAsync<ValidationResult>(methodInfo, validator, (dynamic)query, cancellationToken);

        return result;
    }
}