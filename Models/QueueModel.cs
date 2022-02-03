namespace Queue.Models;

public class QueueModel
{
    [Key,DatabaseGenerated(DatabaseGeneratedOption.None)]
    public Guid ID { get; set; } = Guid.NewGuid();
    
    [Required(ErrorMessage = "To'liq ism-sharfini kiritish shart!"),MinLength(2)]
    [Display(Name = "Ismingiz")]
    public string CustomerName { get; set; }
    
    [Required(ErrorMessage = "Telefon raqam kiritish shart!")]
    [RegularExpression(
        @"^[\+]?(998[-\s\.]?)([0-9]{2}[-\s\.]?)([0-9]{3}[-\s\.]?)([0-9]{2}[-\s\.]?)([0-9]{2}[-\s\.]?)$", 
        ErrorMessage = "Telefon raqam formati noto'g'ri.")]
    public string Phone { get; set; }
    
    public DateTimeOffset CreatedAt { get; set; }
    
    public DateTimeOffset ServiceEndTime { get; set; }
    
    public DateTimeOffset ExpirationTime { get; set; }
    
    public bool IsActive { get; set; }

}
