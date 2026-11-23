using Home_Security.Entities.Identity;
using Home_Security.Entities;
using Home_Security.Models.DTOs;
using System.Text.Json;
namespace Home_Security.Models.DTOs;
public class CreatePropertyDto
{
    public string PropertyName { get; set; }
    public JsonDocument? JsonData { get; set; }
}
public class UpdatePropertyDto
{
    public int Id { get; set; }
    public string? PropertyName { get; set; }
    public JsonDocument? JsonData { get; set; }
}
public class GetPropertyDto
{
    public int Id { get; set; }
    public string PropertyName { get; set; }
    public JsonDocument? JsonData { get; set; }
}