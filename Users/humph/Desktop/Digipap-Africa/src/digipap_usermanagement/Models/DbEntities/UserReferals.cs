
using Digipap.Models.Identity.DbEntity;

namespace Digipap.Models.DbEntities;

public class UserReferal : BaseModel
{
    // Main Properties  
    public string? ReferalCode { get; set; } 
    public virtual User? User { get; set;}
    
    // Supplementary properties 
    public DateTime? CreatedDate { get; set; }     
}