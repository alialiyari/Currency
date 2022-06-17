using FluentValidation;
using Microsoft.Extensions.Localization;
using System.Text.RegularExpressions;

namespace Userr
{

    public class UserRegisterModel
    {
        public string NationalCode { get; set; }

        public string Mobile { get; set; }
    }

    public class UserRegisterModelValidator : AbstractValidator<UserRegisterModel>
    {
        public UserRegisterModelValidator(IStringLocalizerFactory factory)
        {

            var localizer = factory.Create(typeof(UserRegisterModelValidator));


            RuleFor(x => x.NationalCode).NotEmpty().WithMessage(localizer["NationalIsEmptyError"])
                .Custom((NationalCode, context) => { if (IsValidNationalCode(NationalCode) == false) context.AddFailure(localizer["NationalCodeValidator"]); });


            RuleFor(x => x.Mobile).Custom((mobile, context) =>
            {
                if (string.IsNullOrEmpty(mobile)) { context.AddFailure(localizer["MobileValidator"]); return; }
                if (!new Regex(@"^\d{11}$").Match(mobile).Success) context.AddFailure(localizer["MobileValidator"]);
            });
        }


        public static bool IsValidNationalCode(string input)
        {

            if (string.IsNullOrEmpty(input)) return false;
            if (!Regex.IsMatch(input, @"^\d{10}$")) return false;

            return true;
            //todo: بررسی ساختار کد ملی را برای تست غیر فعال کردم

            //var check = Convert.ToInt32(input.Substring(9, 1));
            //var sum = Enumerable.Range(0, 9).Select(x => Convert.ToInt32(input.Substring(x, 1)) * (10 - x)).Sum() % 11;

            //return (sum < 2 && check == sum) || (sum >= 2 && check + sum == 11);
        }
    }




    public class UserMobileVerifyModel
    {
        public string UserId { get; set; }
        public string NationalCode { get; set; }


        public string Mobile { get; set; }
        public string SMSCode { get; set; }
    }

    public class UserMobileVerifyModelValidator : AbstractValidator<UserMobileVerifyModel>
    {
        public UserMobileVerifyModelValidator()
        {
            RuleFor(x => x.Mobile).Custom((mobile, context) =>
            {
                if (string.IsNullOrEmpty(mobile)) { context.AddFailure("شماره موبایل خالی می باشد"); return; }
                if (!new Regex(@"^\d{11}$").Match(mobile).Success) context.AddFailure("ساختار موبایل اشتباه می باشد");
            });
        }
    }
}
