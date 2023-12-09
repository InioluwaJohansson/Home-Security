using Home_Security.Contracts;
using System.Dynamic;

namespace Home_Security.Entities;
public class FingerPrint : AuditableEntity
{
    public Person Person { get; set; }
    public int PersonId { get; set; }
    public string FingerPrintId { get; set; }
    public string FingerPrintEncoding { get; set; }
}