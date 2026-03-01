namespace Eshop.Domain.Entities.Museum;

/// <summary>
/// not a table
/// stored procedure
/// </summary>
public class MuseumAvailableDayTimes
{
    public int MuseumID { get; set; }
    
    public DateTime AttendDateTime { get; set; }
    
    public string DayCode { get; set; }
    
    public TimeSpan From { get; set; }
    
    public TimeSpan To { get; set; }

    public int PersPerHourBooking { get; set; }
}
