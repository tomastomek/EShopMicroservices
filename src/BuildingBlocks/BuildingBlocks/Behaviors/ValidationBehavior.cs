using BuildingBlocks.CQRS;
using FluentValidation;
using MediatR;

namespace BuildingBlocks.Behaviors
{
    // apply validation behavior only for command object
    // -> IRequest produces response ICommand<TResponse>
    /*Using IEnumerable<IValidator> allows the behavior pipeline to apply multiple validation rules to the 
     * same command or entity. In many cases, a command might have several aspects that need validation, 
     * and different validators can be responsible for specific parts of the validation logic.The dependency 
     * injection container typically provides IEnumerable<IValidator> when you have registered multiple 
     * validators. For example, when using FluentValidation, and you have multiple validators implementing 
     * the IValidator interface, the container automatically aggregates them when you inject IEnumerable<IValidator>. 
     * This happens because, during service registration, each validator is registered individually, and when the 
     * container is asked for IEnumerable<IValidator>, it collects all these registered validators.
     * IEnumerable collects all validators and validate them in one place
     */
    public class ValidationBehavior<TRequest, TResponse>
        (IEnumerable<IValidator<TRequest>> validators)
        : IPipelineBehavior<TRequest, TResponse>
        where TRequest : ICommand<TResponse>
    {
        public async Task<TResponse> Handle
            (TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            var context = new ValidationContext<TRequest>(request);

            var validationResults = await Task.WhenAll(
                validators.Select(v => v.ValidateAsync(context, cancellationToken)));

            var failures = 
                validationResults
                .Where(r => r.Errors.Any())
                .SelectMany(r => r.Errors)
                .ToList();

            if (failures.Any())
            {
                throw new ValidationException(failures);
            }

            return await next();
        }
    }
}