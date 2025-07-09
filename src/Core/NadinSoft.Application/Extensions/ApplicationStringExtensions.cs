using System.ComponentModel.DataAnnotations;

namespace NadinSoft.Application.Extensions;

public static class ApplicationStringExtensions
{
    public static bool IsEmail(this string str)
    {
        var emailValidation = new EmailAddressAttribute();
        return emailValidation.IsValid(str);
    }
    
}