using FluentValidation;
using FluentValidation.Results;
using LeaveManagement.Application.Abstractions.Messaging;
using SharedKernel.Shared.Errors;
using SharedKernel.Shared.Result;

namespace LeaveManagement.Application.Decorators
{
    public class ValidationDecorator
    {
        private static async Task<ValidationFailure[]> ValidateAsync<TCommand>(
            TCommand command, 
            IEnumerable<IValidator<TCommand>> validators)
        {
            if (!validators.Any())
                return [];

            ValidationContext<TCommand> validationContext = new(command);

            ValidationResult[] validationResult = await Task.WhenAll(validators
                .Select(v => v.ValidateAsync(validationContext))
                .ToArray());

            ValidationFailure[] validationFailures = validationResult
                .Where(v => !v.IsValid)
                .SelectMany(e => e.Errors)
                .ToArray();

            return validationFailures;
        }

        private static ValidationError CreateValidationErrors(ValidationFailure[] validationFailures) => 
            new(validationFailures
                .Select(v => new Error(v.ErrorCode, v.ErrorMessage))
                .ToArray());

        internal sealed class CommandHandler<TCommand, TResponse>(
            ICommandHandler<TCommand, TResponse> innerHandler,
            IEnumerable<IValidator<TCommand>> validators)
            : ICommandHandler<TCommand, TResponse>
            where TCommand : ICommand<TResponse>
        {
            public async Task<ResultT<TResponse>> Handle(
                TCommand command, 
                CancellationToken token = default)
            {
                ValidationFailure[] validationFailures = await ValidateAsync(command, validators);

                if(validationFailures.Length == 0)
                {
                    await innerHandler.Handle(command, token);
                }
                   
                return ResultT<TResponse>.Failure(CreateValidationErrors(validationFailures));
            }
        }

        internal sealed class BaseCommandHandler<TCommand>(
            ICommandHandler<TCommand> innerHandler,
            IEnumerable<IValidator<TCommand>> validators)
            : ICommandHandler<TCommand>
            where TCommand : ICommand
        {
            public async Task<Result> Handle(TCommand command, CancellationToken token = default)
            {
                ValidationFailure[] validationFailures = await ValidateAsync(command, validators);

                if(validationFailures.Length == 0)
                {
                    await innerHandler.Handle(command, token);
                }

                return Result.Failure(new ValidationError([..validationFailures.Select(v => new Error(v.ErrorCode, v.ErrorMessage))]));
            }
        }
    }
}
