using Newtonsoft.Json;

namespace Eshop.Domain.Common;

public static class EntitiesExtensionMethods
{
    public static void SetVisitCountAsPerDate(this SiteVisits siteVisits, DateTime? forDate = null)
    {
        var date = forDate ?? siteVisits.LastVisitDatePerDay;

        var ts = date - siteVisits.LastCountVisitDate;

        // we will count it as a visit if difference more than one hour
        if (ts.TotalMinutes >= 60)
        {
            siteVisits.VisitsCount++;
            siteVisits.LastCountVisitDate = date;
        }
    }

    public static string GetFullName(this ApplicationUser user)
    {
        return string.Concat(user.FirstName, " ", user.LastName);
    }

    public static decimal CalcTotalAmount(this CartTickets cartTickets, bool setAmount = false)
    {
        decimal result = 0;


        if (cartTickets.UnitPrice > 0)
        {
            if (cartTickets.IsGroup)
            {
                result = (cartTickets.Qty) * (cartTickets.UnitPrice)
                    * ((cartTickets.ChildCount) + (cartTickets.AdultCount));
            }
            else
            {
                result = (cartTickets.Qty) * (cartTickets.UnitPrice);
            }
        }

        if (setAmount) cartTickets.TotalAmount = result;

        return result;
    }

    public static void CalcGroupCount(this CartTickets cartTickets)
    {
        int count;

        if (cartTickets.IsGroup)
        {
            count = (cartTickets.ChildCount)
                    + (cartTickets.AdultCount)
                    + (cartTickets.BabyCount);
        }
        else
        {
            count = 1;
        }

        cartTickets.GroupCount = count;
    }

    public static int GetPersonsCount(this CartTickets cartTickets, bool onlyPaid = false)
    {
        var count = 1;

        if (cartTickets.IsGroup)
        {
            count = (cartTickets.ChildCount) + (cartTickets.AdultCount);
            if (!onlyPaid) count += (cartTickets.BabyCount);

            count = count == 0 ? 1 : count;
        }

        return count;
    }

    public static DateTime? GetAttendanceOrIssueDate(this IssuedTickets issuedTickets)
        => !issuedTickets.IsExternal ? issuedTickets.AttendanceDate : issuedTickets.IssueDate;
    public static bool IsAttendanceDateExpired(this IssuedTickets issuedTickets)
        => issuedTickets.AttendanceDate != null && DateTime.Today > issuedTickets.AttendanceDate;

    public static bool IsFirstUsedExpired(this IssuedTickets issuedTickets) => issuedTickets.FirstUsedAt != null && DateTime.Today > issuedTickets.FirstUsedAt;

    public static bool HasExpiredJoined(this IssuedTickets issuedTickets, bool withAlreadyExpired = false)
    {
        var result = issuedTickets.IsJoinedSections;

        if (issuedTickets is { IsJoinedSections: true, IssuedTicketJoins: not null })
        {
            result = (issuedTickets.IsFirstUsedExpired() && issuedTickets.IssuedTicketJoins.Any(r => r.StatusCode == EnumTicketStatus.New))
                    || (withAlreadyExpired && issuedTickets.IssuedTicketJoins.Any(r => r.StatusCode == EnumTicketStatus.Expired));
        }
        return result;
    }

    public static bool IsExpiredTicket(this IssuedTickets issuedTickets)
    {

        var expired = issuedTickets.StatusCode == EnumTicketStatus.Expired
                       || issuedTickets.IsExpired
                       || issuedTickets.IsAttendanceDateExpired()
                       || (issuedTickets.IsJoinedSections &&
                           (issuedTickets.HasExpiredJoined()
                            || (issuedTickets.IsFirstUsedExpired()
                                && issuedTickets.StatusCode == EnumTicketStatus.New)
                           )
                       );

        return expired;
    }

    public static bool IsJoinedContainsSection(this IssuedTickets issuedTickets, int sectionId)
    {
        var result = false;

        if (issuedTickets.IsJoinedSections)
        {
            result = issuedTickets.IssuedTicketJoins != null
                && issuedTickets.IssuedTicketJoins
                    .Any(r => r.SectionId == sectionId && r is { IsActive: true, DeletedAt: null });
        }

        return result;
    }

    public static string GetUserDeletedTicket(this IssuedTickets issuedTickets, ApplicationUser user = null)
    {
        string result = null;

        if (issuedTickets.DeletedAt != null)
        {

            if (user == null || issuedTickets.DeletedBy == null || issuedTickets.DeletedBy == 0)
            {
                result = "SYSTEM";
            }
            else
            {
                result = user.StaffId ?? "TICKET OWNER";
            }
        }

        return result;
    }


    public static IssuedTickets FillLookupData(this IssuedTickets issuedTickets, SectionTickets sectionTicket)
    {

        if (sectionTicket.Ticket != null)
        {
            var ticket = sectionTicket.Ticket;

            issuedTickets.IsFree = sectionTicket.IsFree;
            issuedTickets.IsManyVisits = ticket.IsManyVisits;
            issuedTickets.VisitsCount = ticket.VisitsCount;
            issuedTickets.ForSenior = ticket.ForSenior;
            issuedTickets.WithCompanion = ticket.WithCompanion;
            issuedTickets.ForDisabilities = ticket.ForDisabilities;
            issuedTickets.IsExternal = ticket.IsExternal;
            issuedTickets.TicketType = issuedTickets.TicketType ?? ticket.TicketType; // for online tickets that need to be categorised
            issuedTickets.MinGroupCount = ticket.MinGroupCount;
            issuedTickets.MaxGroupCount = ticket.MaxGroupCount;

            //this.HasExpiration  = sectionTicket.HasExpiration;

            issuedTickets.ItemTahseelCode = sectionTicket.GetFullTahseelCode();
            issuedTickets.ItemPOSTahseelCode = sectionTicket.GetFullPOSTahseelCode();

            // Note: setting validFrom & validTo based on HasExpiration, PeriodDaysLimit and SameDayUse
            // is done by calling "SetExpriyDate" method, after preparing issuedTicket data.
            issuedTickets.HasPeriodLimit = ticket.HasPeriodLimit;
            issuedTickets.PeriodDaysLimit = ticket.PeriodDaysLimit;
            issuedTickets.SameDayUse = ticket.SameDayUse;
            issuedTickets.IsJoinedSections = ticket.IsJoinedSections;
            issuedTickets.IsGroup = ticket.IsGroup;
            issuedTickets.AgeGroup = ticket.AgeGroup;
            issuedTickets.IsPromoted = ticket.IsPromoted;
            issuedTickets.IsForStaff = ticket.IsForStaff;
        }

        return issuedTickets;
    }

    public static IssuedTickets SetExpriyDate(this IssuedTickets issuedTickets,
        DateTime? fromDate = null, DateTime? toDate = null)
    {

        // this method must be called after you fill the lookup data from by "FillLookupData" Method on the issuedTicket obj
        // => _issuedTicket.FillLookupData(_sectionTicketObj).SetExpriyDate();
        var periodDaysLimit = issuedTickets.PeriodDaysLimit;
        var setByParams = fromDate != null && toDate != null;
        DateTime? _fromDate = fromDate ?? DateTime.Now, _toDate = toDate;


        if (issuedTickets.TicketType == EnumTicketBaseTypes.System_PROMO)
        {
            if (setByParams)
            {
                issuedTickets.ValidFrom = _fromDate;
                issuedTickets.ValidTo = _toDate;
            }

            // System_PROMO has dynamic period.
            var limitDays = (issuedTickets.ValidTo.Value.Date - issuedTickets.ValidFrom.Value.Date).Days;
            issuedTickets.PeriodDaysLimit = limitDays + 1;
            issuedTickets.HasPeriodLimit = true;
            issuedTickets.HasExpiration = true;

            return issuedTickets;
        }

        // if ticket defined as no periodLimit for it.
        if (!issuedTickets.HasPeriodLimit) return issuedTickets;


        if (!setByParams)
        {
            periodDaysLimit = periodDaysLimit != 0 ? periodDaysLimit : 1;
            _fromDate = !issuedTickets.IsExternal ? issuedTickets.AttendanceDate : issuedTickets.IssueDate;
            _fromDate = _fromDate?.Date ?? DateTime.Now.Date;
            _toDate = periodDaysLimit == 1 ? _fromDate : _fromDate.Value.AddDays((periodDaysLimit - 1));
        }

        issuedTickets.ValidFrom = _fromDate;
        issuedTickets.ValidTo = _toDate;
        issuedTickets.HasExpiration = true;

        return issuedTickets;
    }

    public static void FillIssuedTicketData(this IssuedTicketScanLogs issuedTicketScanLogs, IssuedTickets issuedTicket)
    {
        if (issuedTicket != null)
        {
            issuedTicketScanLogs.IssuedTicketId = issuedTicket.Id;
            issuedTicketScanLogs.SectionTicketId = issuedTicket.SectionTicketId;
            issuedTicketScanLogs.RefNumber = issuedTicket.RefNumber;
            issuedTicketScanLogs.ExternalRefNumber = issuedTicket.ExternalRefNumber;
            issuedTicketScanLogs.IsExternal = issuedTicket.IsExternal;
            issuedTicketScanLogs.TicketType = issuedTicket.TicketType;
            issuedTicketScanLogs.TicketSource = issuedTicket.TicketSource;
            issuedTicketScanLogs.TicketReference = issuedTicket.TicketReference;
            issuedTicketScanLogs.IsFree = issuedTicket.IsFree;
            issuedTicketScanLogs.IsJoinedSections = issuedTicket.IsJoinedSections;
            issuedTicketScanLogs.ForManyPersons = issuedTicket.ForManyPersons;
            issuedTicketScanLogs.IsExternal = issuedTicket.IsExternal;
            issuedTicketScanLogs.IsGroup = issuedTicket.IsGroup;
            issuedTicketScanLogs.AgeGroup = issuedTicket.AgeGroup;
            issuedTicketScanLogs.GroupCount = issuedTicket.GroupCount;
            issuedTicketScanLogs.BabyCount = issuedTicket.BabyCount;
            issuedTicketScanLogs.ChildCount = issuedTicket.ChildCount;
            issuedTicketScanLogs.AdultCount = issuedTicket.AdultCount;
            issuedTicketScanLogs.DisabilitiesCount = issuedTicket.DisabilitiesCount;
            issuedTicketScanLogs.SeniorsCount = issuedTicket.SeniorsCount;// - issuedTicket.CompanionsCount;
            issuedTicketScanLogs.CompanionsCount = issuedTicket.CompanionsCount;
        }
    }

    private static int IfMinusBeZero(this MuseumAvailableQuantities museumAvailableQuantities, int value)
    {
        return value >= 0 ? value : 0;
    }

    public static void CalcAvailable(this MuseumAvailableQuantities museumAvailableQuantities)
    {
        var allCount = museumAvailableQuantities.Booked
                       + museumAvailableQuantities.BookedPOS
                       + museumAvailableQuantities.Holded
                       + museumAvailableQuantities.GuestHolded;

        museumAvailableQuantities.Available = museumAvailableQuantities
            .IfMinusBeZero(museumAvailableQuantities.PersPerHourBooking
            - allCount);
    }

    public static void AddHolded(this MuseumAvailableQuantities
        museumAvailableQuantities, int holdedQty, int ticketsCount)
    {
        switch (holdedQty)
        {
            case 0:
                return;
            //qty here it refers to number of persons
            case < 0:
                museumAvailableQuantities.Holded = museumAvailableQuantities.IfMinusBeZero(museumAvailableQuantities.Holded + holdedQty);
                museumAvailableQuantities.HoldedTickets = museumAvailableQuantities.IfMinusBeZero(museumAvailableQuantities.HoldedTickets - ticketsCount);
                break;
            default:
                museumAvailableQuantities.Holded += holdedQty;
                museumAvailableQuantities.HoldedTickets += ticketsCount;
                break;
        }

        museumAvailableQuantities.CalcAvailable();
    }

    public static void AddHoldedToBooked(this MuseumAvailableQuantities museumAvailableQuantities, int holdedToBookedQty, int? holdedToBookedTickets = null)
    {
        //qty here it refers to number of persons
        if (holdedToBookedQty <= 0)
            return;
        var holdedToBookedTicketsVar = holdedToBookedTickets ?? 1;

        museumAvailableQuantities.Booked += holdedToBookedQty;
        museumAvailableQuantities.Holded = museumAvailableQuantities.IfMinusBeZero(museumAvailableQuantities.Holded - holdedToBookedQty);
        museumAvailableQuantities.BookedTickets += holdedToBookedTicketsVar;
        museumAvailableQuantities.HoldedTickets = museumAvailableQuantities.IfMinusBeZero(museumAvailableQuantities.HoldedTickets - holdedToBookedTicketsVar);

        museumAvailableQuantities.CalcAvailable();
    }

    public static void AddBooked(this MuseumAvailableQuantities museumAvailableQuantities, int bookedQty, int? bookedTickets = null)
    {
        var bookedTicketsVar = bookedTickets ?? 1;

        //qty here it refere to number of persons
        if (bookedQty < 0)
        {
            museumAvailableQuantities.Booked = museumAvailableQuantities.IfMinusBeZero(museumAvailableQuantities.Booked + bookedQty);
            museumAvailableQuantities.BookedTickets = museumAvailableQuantities.IfMinusBeZero(museumAvailableQuantities.BookedTickets - bookedTicketsVar);
        }
        else
        {
            museumAvailableQuantities.Booked += bookedQty;
            museumAvailableQuantities.BookedTickets += bookedTicketsVar;
        }

        museumAvailableQuantities.CalcAvailable();
    }

    public static void AddBookedPOS(this MuseumAvailableQuantities museumAvailableQuantities, int bookedQty, int? bookedTickets = null)
    {
        //qty here it refere to number of persons
        var bookedTicketsVar = bookedTickets ?? 1;

        if (bookedQty < 0)
        {
            museumAvailableQuantities.BookedPOS = museumAvailableQuantities.IfMinusBeZero(museumAvailableQuantities.BookedPOS + bookedQty);
            museumAvailableQuantities.BookedPOSTickets = museumAvailableQuantities.IfMinusBeZero(museumAvailableQuantities.BookedPOSTickets - bookedTicketsVar);
        }
        else
        {
            museumAvailableQuantities.BookedPOS += bookedQty;
            museumAvailableQuantities.BookedPOSTickets += bookedTicketsVar;
        }

        museumAvailableQuantities.CalcAvailable();
    }
    public static decimal CalcTotalAmount(this Orders orders, bool setAmount = false)
    {
        decimal subTotal = 0;
        decimal totalVat = 0;

        if (orders.OrderTickets is { Count: > 0 })
        {
            subTotal = orders.OrderTickets.Sum(ct => ct.SubTotal);
            totalVat = orders.OrderTickets.Sum(ct => ct.TotalVat);
        }

        if (setAmount)
        {
            orders.SubTotal = subTotal;
            if (subTotal > 0)//must be here because if order is 0 in free tickets it causes a problem
                orders.Vat = totalVat;
            orders.ExtraFees = subTotal >= 50 && orders.PaymentTypeCode == EnumPaymentTypes.Cash ? 10 : 0; 
            // visa payment(  we don't calculate ExtraFees fees as it calculated in tahseel)
            orders.TotalAmount = subTotal + orders.Vat + orders.ExtraFees;
            orders.ReturnsValue = orders.PaidValue - orders.TotalAmount;
        }

        return orders.TotalAmount;
    }

    /// <summary>
    /// done
    /// </summary>
    /// <param name="orderTickets"></param>
    /// <param name="setAmount"></param>
    /// <returns></returns>
    public static decimal CalcTotalAmount(this OrderTickets orderTickets, bool setAmount = false)
    {
        decimal result = 0;

        if (orderTickets.IsGroup)
            result = (orderTickets.GroupCount ?? orderTickets.Qty) * (orderTickets.UnitPrice);

        if (orderTickets is { UnitPrice: > 0, IsGroup: false })
            result = orderTickets.Qty * orderTickets.UnitPrice;

        if (setAmount)
        {
            orderTickets.TotalVat = orderTickets.Qty * orderTickets.UnitVat;
            orderTickets.SubTotal = result;
        }

        return result;
    }

    public static string GetTahseelSubDepartmentCode(this SectionTickets sectionTickets)
    {
        return sectionTickets.Section.TahseelCode;
    }

    public static string GetTahseelMainServiceCode(this SectionTickets sectionTickets)
    {
        return sectionTickets.Ticket.TicketCategory.TahseelCode;
    }

    public static string GetTahseelSubServiceCode(this SectionTickets sectionTickets)
    {
        return sectionTickets.Ticket.TahseelCode;
    }

    public static string GetFullTahseelCode(this SectionTickets sectionTickets)
    {
        return $"{EnumOrderConfig.MainDepTahseelCode}#{sectionTickets.GetTahseelSubDepartmentCode()}#{sectionTickets.GetTahseelMainServiceCode()}#{sectionTickets.GetTahseelSubServiceCode()}";
    }

    // POS Tahseel Code
    public static string GetPOSTahseelSubDepartmentCode(this SectionTickets sectionTickets)
    {
        return sectionTickets.Section.POSTahseelCode;
    }

    public static string GetPOSTahseelMainServiceCode(this SectionTickets sectionTickets)
    {
        return sectionTickets.Ticket.TicketCategory.POSTahseelCode;
    }

    public static string GetPOSTahseelSubServiceCode(this SectionTickets sectionTickets)
    {
        return sectionTickets.Ticket.POSTahseelCode;
    }

    public static string GetFullPOSTahseelCode(this SectionTickets sectionTickets)
    {
        return $"{EnumOrderConfig.MainDepTahseelCode}#{sectionTickets.GetPOSTahseelSubDepartmentCode()}#{sectionTickets.GetPOSTahseelMainServiceCode()}#{sectionTickets.GetPOSTahseelSubServiceCode()}";
    }

    public static void SetVisitCountAsPerDate(this SiteVisitTickets siteVisitTickets, DateTime forDate)
    {
        var ts = forDate - siteVisitTickets.LastCountVisitDate;

        // we will count it as a visit if difference more than one hour
        if (ts.TotalMinutes >= 60)
        {
            siteVisitTickets.VisitsCount++;

            siteVisitTickets.LastCountVisitDate = forDate;
        }
    }

    public static TahseelPayTransLogs AsTahseelPayTransLogs(this TahseelPayTrans tahseelTran,
        long paymentTransactionId)
    {
        return new TahseelPayTransLogs
        {
            PaymentTransId = paymentTransactionId,
            IsSuccess = tahseelTran.IsSuccess,
            CreateAt = tahseelTran.CreateAt,
            TP_RefNo = tahseelTran.TP_RefNo,
            TP_ResultCode = tahseelTran.TP_ResultCode,
            TP_SecHash = tahseelTran.TP_SecHash,
            UserId = tahseelTran.UserId,
            TotalAmount = tahseelTran.TotalAmount,
            TP_ExtraFees = tahseelTran.TP_ExtraFees,
            TP_TaxFees = tahseelTran.TP_TaxFees,
            TP_PaymentDate = tahseelTran.TP_PaymentDate,
            TP_PayMethod = tahseelTran.TP_PayMethod,
            TP_ReceiptNo = tahseelTran.TP_ReceiptNo,
        };
    }

    public static Audit ToAudit(this AuditEntry auditEntry)
    {
        var jsonSerializeSetting = new JsonSerializerSettings
        {
            ReferenceLoopHandling = ReferenceLoopHandling.Ignore
        };
        var audit = new Audit
        {
            UserId = auditEntry.UserId,
            Type = auditEntry.AuditType.ToString(),
            TableName = auditEntry.TableName,
            DateTime = DateTime.Now,
            PrimaryKey = JsonConvert.SerializeObject(auditEntry.KeyValues, Formatting.None, jsonSerializeSetting),
            Foreignkey = JsonConvert.SerializeObject(auditEntry.ForeignkeyValues, Formatting.None, jsonSerializeSetting),
            OldValues = auditEntry.OldValues.Count == 0 ? null : JsonConvert.SerializeObject(auditEntry.OldValues, Formatting.None, jsonSerializeSetting),
            NewValues = auditEntry.NewValues.Count == 0 ? null : JsonConvert.SerializeObject(auditEntry.NewValues, Formatting.None, jsonSerializeSetting),
            AffectedColumns = auditEntry.ChangedColumns.Count == 0 ? null : JsonConvert.SerializeObject(auditEntry.ChangedColumns, Formatting.None, jsonSerializeSetting)
        };
        return audit;
    }
}