using FluentValidation;
using FluentValidation.Results;
using LeaveManagement.Application.Abstractions.Messaging;
using Microsoft.Extensions.Validation;
using SharedKernel.Shared.Errors;
using SharedKernel.Shared.Result;
using System.Runtime.CompilerServices;

namespace LeaveManagement.Application.Decorators
{
    public class ValidationDecorator
    {
        private static async Task<ValidationFailure[]> ValidateAsync<TCommand>(TCommand command, IEnumerable<IValidator<TCommand>> validators)
        {
            if(!validators.Any())
                return Array.Empty<ValidationFailure>();

            var validationContext = new ValidationContext<TCommand>(command);

            ValidationResult[] validationResults = await Task
                .WhenAll(validators
                .Select(v => v.ValidateAsync(validationContext)));

            ValidationFailure[] validationFailure = validationResults
                .Where(v => !v.IsValid)
                .SelectMany(validationResults => validationResults.Errors)
                .ToArray();

            return validationFailure;
        }

        private static ValidationError CreateValidationError(ValidationFailure[] validationFailures) =>
            new(validationFailures.Select(v => new Error(v.ErrorCode, v.ErrorMessage)).ToArray());
        internal sealed class CommandHandler<TCommand, TResponse>(
            ICommandHandler<TCommand, TResponse> innerHandler,
            IEnumerable<IValidator<TCommand>> validators)
            : ICommandHandler<TCommand, TResponse> 
            where TCommand : ICommand<TResponse>
        {
            public async Task<ResultT<TResponse>> Handle(TCommand command, CancellationToken token = default)
            {
                var validationFailures = await ValidateAsync(command, validators);

                if(validationFailures.Length == 0)
                {
                    return await innerHandler.Handle(command, token);
                }

                return ResultT<TResponse>.Failure(CreateValidationError(validationFailures));
            }
        }
    }
}
