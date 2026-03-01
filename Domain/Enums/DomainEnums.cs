namespace Eshop.Domain.Enums;

public static class EnumSystemMeta
{
    public const int
        SystemUserID = 0;
}

public static class EnumAppSettingsKeys
{
    public const string
        TicketEmailEnquiries = "TicketEmailEnquiries",
        OpenDrawer = "OpenDrawer";
}

public static class EnumOrderConfig
{
    public static string OrderRefPrefix => "ORD";
    public static string TicketRefPrefix => "TIK";
    public static string PayTransRefPrefix => "PAY";
    public static string AnnualMembershipPrefix => "SMA";
    public static string PromotedTicketRefPrefix => "Promo";
    public static string MainDepTahseelCode => "41";
}

public static class EnumTahseelPayResultCodes
{
    public const string
        PaymentDoneSuccessfully = "0",
        WrongEncryption = "-1",
        InvalidServiceAmount = "-2",
        InvalidCredentialsToTypedAccount = "-3",
        PaymentAlreadyDone = "-4",
        NoEnoughBalance = "-5",
        TahseelAccountIsInactive = "-6",
        UnexpectedErrorOccurredDuringPayment = "-10",
        UserCanceledTheProccess = "-11",
        InvalidReferenceNumber = "-12",
        BankRefusedThePayment = "14";
}
public enum ResultCodeEnum
{
    None,
    ValidationError,
    TwoFactor,
    Exception,
    ConfirmEmail,
    ConfirmSms,
    CodeNotSendBeforeSendItAgain,
    CodeDurationEnded,
    Validation
}
public enum TahseelInitiateMPosPayResultCodes
{
    ThePaymentInitiatedSuccessfully = 0,
    ErrorInvalidTerminalNumber = -1,
    ErrorInvalidServiceAmount = -2,
    ErrorProblemDuringInitiatePaymentOnDevice = -3,
    ErrorInvalidReferenceNumber = -4,
    ErrorInvalidRequest = -5,
    NoResult = -6,
    InvalidServiceInformation = -7,
    PaidBefore = -8,
    ErrorCreateTransaction = -9,
    AmountmismatchWithPreviousRequestForSameReferenceNumber = -10,
    ErrorPaidBeforeWithDifferentServiceAmount = -11,
    OperationisNotAllowed = -100,
    InvalidAccount = -101,
    MerchantNotRecognized = -102,
    MerchantNotActive = -103,
    AccountisNotAuthorizedOnThatOperation = -104,
    PermitOnTheOperationIsNotActive = -105,
    InternalServiceError = -200,
}
public enum TahseelCheckMPosStatusResultCodes
{
    PaidSuccessfully = 0,
    InvalidTransactionNumber = -1,
    WaitResponseFromDevice = -2,
    ProblemDuringPaymentonDevice = -3,
    InvalidRequest = -4,
    PaidBefore = -5,
    ErrorPaidBankSuccefulyWithErrorInCommit = -7,
    TransactionCancelledFromDevice = -8,
    CanceledTransaction = -9,
    OperationisNotAllowed = -100,
    UnexpectedErrorOccurredDuringPayment = -101,
    MerchantNotRecognized = -102,
    MerchantNotActive = -103,
    AccountisNotAuthorizedOnThatOperation = -104,
    PermitOnTheOperationIsNotActive = -105,
    InternalServiceError = -200

}
public enum TahseelCancelPaymentResultCodes
{
    TransactionCanceledSuccessfully = 0,
    TransactionNotFound = -1,
    FailedToReturnMoneyToTheCardholder = -2,
    MismatchTransactinAmount = -3,
    TransactionCanceledBefore = -4,
    ErrorInvalidRequest = -5,
    NoResult = -6,
    ErrorInvalidTransactionStatus = -7,
    ErrorInvalidTerminalData = -8,
    ErrorTransactionCancelOverDue = -9,
    OperationIsNotAllowed = -100,
    InvalidAccount = -101,
    MerchantNotRecognized = -102,
    MerchantNotActive = -103,
    AccountIsNotAuthorizedOnThatOperation = -104,
    PermitOnTheOperationIsNotActive = -105,
    InternalServiceError = -200
}
public enum TahseelRefundResultCodes
{
    TransactionCanceledSuccessfully = 0,
    OperationIsNotAllowed = -100,
    InvalidAccount = -101,
    MerchantNotRecognized = -102,
    MerchantNotActive = -103,
    AccountIsNotAuthorizedOnThatOperation = -104,
    PermitOnTheOperationIsNotActive = -105,
    DifferentCaller = -106,
    InvalidRequest = -107,
    InvalidTransactionDetails = -108,
    MismatchReceiptNumber = -109,
    FailtoRegisterRequest = -110,
    NoResult = -111,
    TransactionAlreadyRefunded = -112,
    InvalidTransactionStatus = -113,
    PaymentMethodNotSupported = -114,
    TransactionHasPendingRequests = -115,
    InvalidRequestNumber = -116,
    InternalServiceError = -200

}

public static class EnumOrderChannels
{
    public const int
        OnlineWebsite = 1,
        POS = 2,
        POSOnline = 3;
}

public static class EnumTransactionChannels
{
    public const int
        OnlineWebsite = 1,
        POS = 2,
        POSOnline = 3;

}

public static class EnumCartItemTypes
{
    public const string
        Ticket = "T",
        Product = "P",
        Gift = "G";
}

public static class EnumTicketUsage
{
    public const int
        None = 0,
        ForOnline = 1,
        ForPOS = 2,
        ForBoth = 3;
}

public enum EnumAuditType
{
    None = 0,
    Create = 1,
    Update = 2,
    Delete = 3
}

public static class EnumPOS
{
    public static readonly List<string> GovFeesItemsIds = ["209935", "209936", "209937", "209938"];
}
//=>
public enum EnumVatPayer
{
    SMA,
    Client
}