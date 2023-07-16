using Azure;
using FluentValidation;
using FluentValidation.Results;
using LanguageExt.Common;
using MediatR;

namespace Shared.PipelineBehaviors;
public sealed class ValidationBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : class, IRequest<TResponse>
    where TResponse : struct
{
    private readonly IEnumerable<IValidator<TRequest>> _validators;
    public ValidationBehavior(IEnumerable<IValidator<TRequest>> validators) => _validators = validators;

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        if (!_validators.Any())
        {
            return await next();
        }

        var context = new ValidationContext<TRequest>(request);

        var validationTasks = _validators
            .Select(x => x.ValidateAsync(context));

        var validationResults = await Task.WhenAll(validationTasks);

        var errors = validationResults
            .SelectMany(x => x.Errors)
            .Where(x => x != null)
            .Select(x => new ValidationFailure(x.PropertyName, x.ErrorMessage));

        if (errors?.Any() ?? false)
        {
            var error = new ValidationException(errors);
            return (TResponse)Activator.CreateInstance(typeof(TResponse), error)!;
        }
        return await next();
    }
}
