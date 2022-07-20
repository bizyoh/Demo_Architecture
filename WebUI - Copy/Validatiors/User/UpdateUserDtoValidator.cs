using Application.Models.DTO.UserDto;
using FluentValidation;


namespace WebUI.Validatiors.User

{
    public class UpdateUserDtoValidator : AbstractValidator<UpdateUserDto>
    {
        public UpdateUserDtoValidator()
        {
            RuleFor(x => x.NewPassword).NotEmpty().WithMessage("Password is required")
                .MaximumLength(30).WithMessage("Password's length is from 4 - 30 letters").MinimumLength(4).WithMessage("Password's length is from 4 - 30 letters")
                .Matches("^(?=.*[a-z])(?=.*[A-Z])(?=.*\\d)(?=.*[@$!%*?&])[A-Za-z\\d@$!%*?&]{8,}$").WithMessage("Password must be in format");

            RuleFor(x => x.ConfirmPassword).NotEmpty().WithMessage("Confirm Password is required")
                .MaximumLength(30).WithMessage("Confirm Password's length is from 4 - 30 letters").MinimumLength(4).WithMessage("Confirm Password's length is from 4 - 30 letters")
                .Matches("^(?=.*[a-z])(?=.*[A-Z])(?=.*\\d)(?=.*[@$!%*?&])[A-Za-z\\d@$!%*?&]{8,}$").WithMessage("Password must be in format");

            RuleFor(x => x.CurrentPassword).NotEmpty().WithMessage("Current Password is required")
                .MaximumLength(30).WithMessage("Confirm Password's length is from 4 - 30 letters").MinimumLength(4).WithMessage("Confirm Password's length is from 4 - 30 letters")
                .Matches("^(?=.*[a-z])(?=.*[A-Z])(?=.*\\d)(?=.*[@$!%*?&])[A-Za-z\\d@$!%*?&]{8,}$").WithMessage("Password must be in format");

            RuleFor(x => x.Email).NotEmpty().WithMessage("Email is required")
                .MaximumLength(30).MinimumLength(4).WithMessage("Email's length is from 4 - 30 letters")
                .Matches("[a-z0-9]+@[a-z]+\\.[a-z]{2,3}").WithMessage("Email must be in format");

            RuleFor(x => x.FirstName).NotEmpty().WithMessage("First Name is required")
                .MaximumLength(30).WithMessage("First Name's length is from 4 - 30 letters").MinimumLength(4).WithMessage("First Name's length is from 4 - 30 letters");

            RuleFor(x => x.LastName).NotEmpty().WithMessage("Last Name is required")
            .MaximumLength(30).WithMessage("Last Name's length is from 4 - 30 letters").MinimumLength(4).WithMessage("Last Name's length is from 4 - 30 letters");

            RuleFor(x => x.Address).NotNull().WithMessage("Address is required")
            .MaximumLength(30).WithMessage("Last Name's length is from 4 - 30 letters").MinimumLength(4).WithMessage("Address length is from 4 - 30 letters");

            RuleFor(x => x.PhoneNumber).NotEmpty().WithMessage("Phone Number is required")
            .MaximumLength(12).WithMessage("Phone number is from 8-12 numbers").MinimumLength(8).WithMessage("Phone number is from 8-12 numbers")
            .Matches("(03|05|07|08|09|01[2|6|8|9])+([0-9]{6})").WithMessage("Phone number can contain only numbers");


        }

    }

}
