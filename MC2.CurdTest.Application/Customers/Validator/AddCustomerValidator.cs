using FluentValidation;
using MC2.CurdTest.Application.Customers.Commands;
using PhoneNumbers;

namespace MC2.CurdTest.Application.Customers.Validator
{
    public class AddCustomerValidator : AbstractValidator<AddCustomerCommand>
    {
        public AddCustomerValidator()
        {
            RuleFor(x => x.FirstName).NotEmpty();
            RuleFor(x => x.LastName).NotEmpty();
            RuleFor(x => x.Email).EmailAddress();
            RuleFor(x => x.PhoneNumber).NotEmpty().Must(BeValidMobileNumber).WithMessage("Please enter a valid mobile number");
        }
        private bool BeValidMobileNumber(string phoneNumber)
        {
            var phoneNumberUtil = PhoneNumberUtil.GetInstance();
            var parsedPhoneNumber = phoneNumberUtil.Parse(phoneNumber, null);
            return phoneNumberUtil.IsValidNumberForRegion(parsedPhoneNumber, "IR");
        }
    }
}
