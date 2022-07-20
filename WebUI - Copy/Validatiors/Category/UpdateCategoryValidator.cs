using Application.Models.DTO.Category;
using FluentValidation;


namespace TanPhucShopApi.Validatiors.Category

{
    public class UpdateCategoryValidator : AbstractValidator<UpdateCategoryDto>
    {
        public UpdateCategoryValidator()
        {
            RuleFor(x => x.Name).NotEmpty().WithMessage("Category'name is required")
                  .MaximumLength(20).WithMessage("Category'name length from 2 - 20 letters").MinimumLength(2).WithMessage("Category'name length from 2 - 20 letters");

        }

    }

}
