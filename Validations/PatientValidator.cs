using FluentValidation;

namespace RESTfulAPIExample.Validations
{
    public class PatientValidator : AbstractValidator<Patient>
    {
        public PatientValidator()
        {
            RuleFor(x => x.Id).Must(t => t > 0).WithMessage("Id must be greater than 0");
            RuleFor(x => x.FirstName).NotNull();
            RuleFor(x => x.LastName).NotNull();
        }
    }
}
