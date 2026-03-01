namespace Eshop.Domain.Enums;

[Flags]
public enum ProjectTypeEnum
{
    None,
    EshopWeb,
    EshopApi,
    PosWeb
}

public enum TahseelPaymentMethodEnum
{
    Eshop = 1,
    Pos = 5
}
public enum EmailLogForEnum
{
    None,
    ConfirmEmail,
    ForgetPassword,
}

public enum TokenPurposeEnum
{
    None,
    ResetPassword
}