namespace Home_Security.Entities;
public class External : BaseDefault
{
    public string ExternalName { get; set; }
    public string ExternalId { get; set; }
    public bool IsTriggered { get; set; }
    public List<int>? ActionIds { get; set; }
}
