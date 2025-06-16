using FluentValidation;

namespace Voltly.Application.Features.Users.Commands.CreateUser;

public sealed class CreateUserValidator : AbstractValidator<CreateUserCommand>
{
    public CreateUserValidator()
    {
        RuleFor(c => c.Name)     .NotEmpty().MaximumLength(120);
        RuleFor(c => c.Email)    .NotEmpty().EmailAddress().MaximumLength(180);
        RuleFor(c => c.Password) .NotEmpty().MinimumLength(6).MaximumLength(50);
        RuleFor(c => c.BirthDate).LessThan(DateOnly.FromDateTime(DateTime.Today));
    }
}