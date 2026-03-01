namespace Eshop.Domain.Enums;

public static class EnumCartStatus
{
    public const string
        New = "1",
        Done = "2";
}

public static class EnumOrderStatus
{
    public const string
        WaitPayment = "0",
        New = "1",
        Delivered = "2",
        Canceled = "3";
}


public static class EnumPaymentTypes
{
    public const string
        Cash = "1",
        Credit = "2",
        Visa = "3";
}

public static class EnumSectionTypes
{
    public const string
        Normal = "S",
        Museum = "M";
}

public static class EnumChannelCode
{
    public const int
        Online = 1,
        Pos = 2,
        newPos = 3;
}

public static class EnumIssuedTicketActions
{
    [Description("Scan")]
    public const string Scan = "1";

    [Description("Use")]
    public const string Use = "2";

    [Description("Expire")]
    public const string Expire = "3";

    [Description("Cancel")]
    public const string Cancel = "4";

    [Description("Pay")]
    public const string Pay = "5";

    [Description("Delete")]
    public const string Delete = "6";

    [Description("Permenant Delete")]
    public const string ForceDelete = "7";
}

public static class EnumTicketStatus
{
    [Description("New")]
    public const string New = "1";

    [Description("Used")]
    public const string Used = "2";

    [Description("Expired")]
    public const string Expired = "3";

    [Description("Canceled")]
    public const string Canceled = "4";
}

public static class EnumOrderTicketPaymentStatus
{
    [Description("Free")]
    public const int Free = 1;

    [Description("WaitPayment")]
    public const int WaitPayment = 2;

    [Description("WaitResponseFromPayGatway")]
    public const int WaitResponseFromPayGatway = 3;

    [Description("PaymentDone")]
    public const int PaymentDone = 4;
}

public static class EnumPaymentTransStatus
{
    [Description("WaitPayment")]
    public const int WaitPayment = 1;

    [Description("PaymentDone")]
    public const int PaymentDone = 2;

    [Description("Cancelled")]
    public const int Cancelled = 3;

    [Description("Error")]
    public const int Error = -1;
}

public static class EnumPaymentTransDetailItemTypes
{
    [Description("Ticket")]
    public const string Ticket = "T";

    [Description("Product")]
    public const string Product = "P";

    [Description("Gift")]
    public const string Gift = "G";
}

public static class EnumTicketBaseTypes
{/*
  "B_LOCL", "B_AR_RESD", "B_EN_RESD", "B_AR_TORST", "B_EN_TORST"*/
    public static string
        Basic = "B",
        Basic_POS = "B_POS",
        System_PROMO = "SYS_PROMO", // free, like tickets for staff, family, companies ...
        Basic_All = "B_ALL",
        Basic_Local = "B_LOCL",
        Basic_ArResident = "B_AR_RESD",
        Basic_EnResident = "B_EN_RESD",
        Basic_ArTourist = "B_AR_TORST",
        Basic_EnTourist = "B_EN_TORST",
        Basic_TouristGroup = "B_TORST_GP",
        Basic_TouistShurooq = "B_TORST_SHRQ",
        Basic_UnivSchoolGov = "B_UNIV_SCHOOL_GOV",
        Basic_UnivSchoolPriv = "B_UNIV_SCHOOL_PRV",
        Basic_UnivSchoolGov_Wrkshop = "B_UNIV_SCHOOL_GOV_WRKSHOP",
        Basic_UnivSchoolPriv_Wrkshop = "B_UNIV_SCHOOL_PRV_WRKSHOP",
        Basic_PROMO = "B_PROMO", // not free, like buy for 3 visits, get 1 visit free

        VIP = "VIP",
        OfficalVisits = "OFFIC_VISIT",
        IcomMembers = "ICOM_MEMBER",
        Media = "MEDIA",
        Events = "EVENT",
        HOTEL_STAFF = "HOTEL_STAFF",
        Basic_Staff = "B_STAFF",
        Hall_Tenant = "HALL_TENT",
        Hall_Rent = "HALL_RENT",
        Camp = "CAMP",
        Workshop = "WRKSHOP",
        Basic_Voucher = "B_VOUCHER",
        Basic_Club = "B_CLUB",
        Photography = "PHOTOGRAPHY",

        Basic_Other = "B_OTHER",
        Groupon = "GPN",
        BusTour = "BST",
        SharjahExperience = "SHEX";
}

public static class EnumTicketAgeGroups
{
    public const string
        Baby = "0",
        Child = "1",
        Adult = "2",
        Mixed = "3";

    public static string GetAgeGroupTitle(string? ageGroup)
    {
        return ageGroup switch
        {
            Adult => nameof(Adult),
            Baby => nameof(Baby),
            Child => nameof(Child),
            Mixed => nameof(Mixed),
            _ => string.Empty
        };
    }
}

public static class EnumTicketSource
{
    public static string
        OnlineOrder => "ONR";

    public static string POS => "POS";
    public static string onlinePOS => "O_POS";
    public static string PromotedTicket => "PRM";
}

public enum EnumExceptionCodes : int
{
    [Description("No Error")]
    NoError = 0,

    [Description("Internal Server Error")]
    ServerError = 500,

    [Description("Sql Error")]
    SqlError = 512,

    [Description("Bad Request")]
    BadRequestError = 400,

    [Description("Bad Parameter")]
    BadParameterError = 422,

    [Description("One or more validation failures have occurred.")]
    ValidationError = 422,

    [Description("Entity Not Found")]
    NotFoundError = 404,

    [Description("No Items exists for order.")]
    NoItemForOrder = 10,

    [Description("Some of the cart tickets are not available.")]
    SomeTicketsNotAvailable = 11,

    [Description("The ticket time-attendance is not valid.")]
    TicketTimeAttendanceNotValid = 12,

    [Description("The museum doesn't have available places at this attendance date.")]
    MuseumNotHaveAvailablePlaces = 13,

    [Description("Admin Cannot Delete himself.")]
    AdminCannotDeleteHimself = 20,

    [Description("The Ticket is not found.")]
    TicketNotFound = 30,

    [Description("The Ticket is Belongs To another Museum.")]
    TicketBelongsToAnotherSection = 31,

    [Description("The Ticket is Deleted.")]
    TicketIsDeleted = 32,

    [Description("Invalid age groups counts.")]
    InvalidAgeGroupsCounts = 33,


    [Description("There is no account with this email.")]
    NoAccountWithThisEmail = 40,

    [Description("Ticket Category is not valid.")]
    TicketCategoryInNotValid = 50,

    [Description("Joined museums have not been selected.")]
    JoinedMuseumsNotSelected = 51,

    [Description("Ticket Section is not valid.")]
    TicketSectionInNotValid = 52,

    [Description("Ticket minimun and maximuum count of group members is not valid.")]
    TicketMinMaxGroupCountNotValid = 53,

    [Description("Ticket set as free with price existing.")]
    TicketHasPriceAndIsFree = 54,

    [Description("Invalid email or password")]
    InvalidEmailOrPassword = 60,

    [Description("Please verify your email address")]
    EmailIsNotVerified = 61,

    [Description("Invalid Tahseel Response")]
    TP_InvalidResponse = 70,

    [Description("Pay Transaction not found")]
    TP_PayTransactionNotFound = 71,

    [Description("Payment already done")]
    TP_PaymentAlreadyDone = 72,


    [Description("Authentication Failed")]
    Api_AuthFailed = 100,
    [Description("User Not Exist")]
    Api_UserNotExist = 101,
    [Description("User Not has permisison")]
    Api_UserNotHasPermission = 102,
    [Description("Invalid Section Code")]
    Api_InvalidSectionCode = 103,
    [Description("Invalid attendance date-time")]
    Api_InvalidAttendanceDateTime = 104,
    [Description("No Available qty for selected attendance date-time")]
    Api_QtyNotAvailableForAttendance = 105,
    [Description("Ticket reference nubmer is already exist")]
    Api_TicketRefIsExist = 106,
    [Description("Ticket is not Found")]
    Api_TicketIsNoExist = 107,
    [Description("Ticket is already expired")]
    Api_TicketAlreadyExpired = 108,
    [Description("Ticket need age groups data")]
    Api_TicketNeedAgeGroupsScan = 109,
    [Description("Ticket is expired")]
    Api_TicketIsExpired = 110,
    [Description("Ticket is already used")]
    Api_TicketIsAlreadyUsed = 111,
    [Description("Ticket is canceled")]
    Api_TicketIsCanceled = 112,
    [Description("Booked ticket wait payment")]
    Api_BookedTicketWaitPayment = 113,
    [Description("Invalid date")]
    Api_InvalidDate = 114,
    [Description("Invalid date format")]
    Api_InvalidDateFormat = 115,
    [Description("Invalid date-time format")]
    Api_InvalidDateTimeFormat = 116,
    [Description("Ticket wait payment response from payment gatway.")]
    TicketWaitResponseFromPayGatway = 117,
    [Description("Missing POS tickets codes in online ticket system")]
    Api_MissingPOSTicketsCodes = 118,
    [Description("Referesh Token Not Active")]
    Api_RefereshTokenNotActive = 119,
    [Description("Ticket Is Deleted")]
    Api_TicketIsDeleted = 120,
    [Description("Invalid age groups counts.")]
    Api_InvalidAgeGroupsCounts = 121,
    [Description("Invalid staff id.")]
    Api_InvalidStaffId = 122,

}

public static class AppExceptionTrans
{
    public static string GetCodeErrorMsg(EnumExceptionCodes errorEnum, string lang = "en")
    {
        var value = errorEnum.ToInt().ToString();
        var key = value;
        var keyExist = false;

        if (!string.IsNullOrEmpty(lang))
        {
            if (Translations.Count > 0 && Translations.ContainsKey(lang))
            {
                var _cultureTrans = Translations[lang];
                if (_cultureTrans.TryGetValue(key, out var tran))
                {
                    value = tran;
                    keyExist = true;
                }
            }

            if (!keyExist) value = "";
        }

        if (!keyExist)
        {
            return errorEnum.GetDescription();
        }

        return value;
    }

    public static Dictionary<string, Dictionary<string, string>> Translations =>
        new()
        {
            {
                "ar", new Dictionary<string, string>{
                    { "31", "التذكره تنتمى لمتحف أخر" },
                    { "32", "تم حذف التذكرة" },
                    { "33", "أعداد غير صحيحة للفئات العمرية" },
                    { "100", "بيانات الربط غير صحيحة" },
                    { "101", "المستخدم غير موجود" },
                    { "102", "لا يملك هذا المستخدم صلاحية الدخول" },
                    { "103", "كود المتحف غير صحيح" },
                    { "104", "موعد الحضور غير صحيح" },
                    { "105", "لا يوجد اماكن لموعد الحضور المحدد" },
                    { "106", "الرقم المرجعي موجود مسبقا" },
                    { "107", "التذكرة غير موجودة" },
                    { "108", "التذكرة منتهية الصحلاحية بالفعل" },
                    { "109", "يجب ادخال بيانات الفئات العمرية" },
                    { "110", "تم انتهاء صلاحية التذكرة" },
                    { "111", "تم استخدام التذكرة بالفعل" },
                    { "112", "تم إلغاء التذكرة" },
                    { "113", "يجب الدفع للتذكرة المحجوزة" },
                    { "114", "التاريخ غير صحيح" },
                    { "115", "خطأ في كتابة التاريخ" },
                    { "116", "خطأ في كتابة التاريخ والوقت" },
                    { "117", "لم يتم اكمال الدفع الالكتروني للتذكرة" },
                    { "118", "يوجد تذاكر لم يتم تسجيلها في نظام التذاكر الالكتروني" },
                    { "119", "Referesh Token Not Active" },
                    { "120", "تم حذف التذكرة" },
                    { "121", "أعداد غير صحيحة للفئات العمرية" },
                    { "122", "يرجي إدخال رقم الموظف بشكل صحيح" },
                }
            },
            {
                "en", new Dictionary<string, string>{
                    { "31", "Ticket belongs to another Museum" },
                    { "32", "Ticket Is Deleted" },
                    { "33", "Invalid age groups counts" },
                    { "100", "Authentication Failed" },
                    { "101", "User Not Exist" },
                    { "102", "User Not has permission" },
                    { "103", "Invalid Section Code" },
                    { "104", "Invalid attendance date-time" },
                    { "105", "No Available qty for selected attendance date-time" },
                    { "106", "Ticket reference number is already exist" },
                    { "107", "Ticket is not Found" },
                    { "108", "Ticket is already expired" },
                    { "109", "Ticket need age groups data" },
                    { "110", "Ticket is expired" },
                    { "111", "Ticket is already used" },
                    { "112", "Ticket is canceled" },
                    { "113", "Booked ticket wait payment" },
                    { "114", "Invalid date" },
                    { "115", "Invalid date format" },
                    { "116", "Invalid date-time format" },
                    { "117", "Ticket wait payment response from payment gatway" },
                    { "118", "Missing POS tickets codes in online ticket system" },
                    { "119", "Referesh Token Not Active" },
                    { "120", "Ticket Is Deleted" },
                    { "121", "Invalid age groups counts" },
                    { "122", "Please enter valid employee number" },

                }
            }
        };
}