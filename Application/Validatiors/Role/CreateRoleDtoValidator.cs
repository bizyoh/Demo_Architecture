using Application.Models.DTO.RoleDto;
using FluentValidation;

namespace Application.Validatiors.Role

{
    public class CreateRoleDtoValidator : AbstractValidator<CreateRoleDto>
    {
        public CreateRoleDtoValidator()
        {
            RuleFor(x => x.Name).NotEmpty().WithMessage("Role'Name is required")
                .MaximumLength(30).WithMessage("Role'Name  length from 2 - 30 letters").MinimumLength(2).WithMessage("Product'Name  length from 2 - 30 letters");

            RuleFor(x => x.Description).NotEmpty().WithMessage("Description is required").MinimumLength(1).WithMessage("Description length from 2 - 30 letters");
        }
    }
}
