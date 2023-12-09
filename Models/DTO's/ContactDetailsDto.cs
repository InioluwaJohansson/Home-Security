namespace Home_Security.Models.DTOs;
public class CreateContactDetailsDto
{
    public int ContactId { get; set; }
    public int PersonDetailsId { get; set; }
    public string PhoneNumber { get; set; }
    public string Email { get; set; }
}
public class UpdateContactDetailsDto
{
    public int Id { get; set; }
    public int ContactId { get; set; }
    public int PersonDetailsId { get; set; }
    public string PhoneNumber { get; set; }
    public string Email { get; set; }
}
public class GetContactDetailsDto
{
    public int Id { get; set; }
    public int ContactId { get; set; }
    public int PersonDetailsId { get; set; }
    public string PhoneNumber { get; set; }
    public string Email { get; set; }
}