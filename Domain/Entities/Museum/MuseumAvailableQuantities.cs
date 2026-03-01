namespace Eshop.Domain.Entities.Museum;

public class MuseumAvailableQuantities : BaseEntity, IId<int>, ISectionEntity
{
    public int Id { get; set; }
    public DateTime AttendDateTime { get; set; }
    public int PersPerHourBooking { get; set; }

    // it may include 1 ticket but 6 booked, it based also on group count if exist.
    public int BookedTickets { get; set; }
    public int BookedPOSTickets { get; set; }
    public int HoldedTickets { get; set; }

    public int Available { get; set; }
    public int Holded { get; set; }
    public int GuestHolded { get; set; }
    public int Booked { get; set; }
    public int BookedPOS { get; set; }
    public int Canceled { get; set; }

    #region Relations

    public int SectionId { get; set; }
    public virtual Sections Section { get; set; }

    #endregion

}
