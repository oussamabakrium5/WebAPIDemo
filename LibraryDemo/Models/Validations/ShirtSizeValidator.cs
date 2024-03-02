using FluentValidation;

namespace LibraryDemo.Models.Validations
{
    public class ShirtSizeValidator : AbstractValidator<Shirt>
    {
        public ShirtSizeValidator() 
        {
            RuleFor(shirt => shirt.Size).NotNull();
            RuleFor(shirt => shirt.Size)
                    .Must((shirt, size) => IsValidSize(size, shirt.Gender))
                    .WithMessage("Invalid size for the specified gender.");
        }

        private bool IsValidSize(int? size, string? gender)
        {
            if (size.HasValue)
            {
                return (size > 6 && gender == "women") || (size > 8 && gender == "men");
            }
            return false;
        }
    }
}
