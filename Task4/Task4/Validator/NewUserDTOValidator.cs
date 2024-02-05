using FluentValidation;
using Task4.DTOs;

namespace Task4.Validator
{
    public class NewUserDTOValidator : AbstractValidator<NewUserDTO>
    {
        public NewUserDTOValidator()
        {
            RuleFor(x => x.FirstName).NotEmpty();
            RuleFor(x => x.LastName).NotEmpty();
            RuleFor(x => x.Telephone).NotEmpty()
                .Matches(@"^\+\d{1,3}(?:\s?\d{2,3}){2,3}\d{2}$")
                .WithMessage("Unvalid format of phone number.");
        }
    }
}
