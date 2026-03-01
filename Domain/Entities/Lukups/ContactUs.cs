namespace Eshop.Domain.Entities.Lukups;

public class ContactUs : BaseEntity, IId<int>
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }
    public string Message { get; set; }
    public string PhoneNumber { get; set; }
}