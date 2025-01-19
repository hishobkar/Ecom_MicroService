namespace Example01.Models;
public class UserProfile
{
    public int UserId { get; set; }
    public string FullName { get; set; }
    public string Email { get; set; }
    public string Address { get; set; }
    public string PhoneNumber { get; set; }
    public List<Order> OrderHistory { get; set; }
}