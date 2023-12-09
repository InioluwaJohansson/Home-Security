namespace Home_Security.Models.DTOs;
public class CreateContactCategoryDto
{
    public string Name { get; set; }
    public string Description { get; set; }
    public int personId { get; set; }
}
public class UpdateContactCategoryDto
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public int personId { get; set; }
}
public class GetContactCategoryDto : BaseDefaultDtoOther
{
    public int Id { get; set; }
    public string CategoryId { get; set; }   
    public string Name { get; set; }
    public string Description { get; set; }
    public int personId { get; set; }
}
public class ContactCategoryResponseModel : BaseResponse
{
    public GetContactCategoryDto Data { get; set; }
}
public class ContactCategoriesResponseModel : BaseResponse
{
    public ICollection<GetContactCategoryDto> Data { get; set; } = new HashSet<GetContactCategoryDto>();
}