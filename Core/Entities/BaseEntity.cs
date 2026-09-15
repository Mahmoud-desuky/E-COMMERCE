namespace ECommerce.Core.Entities
{
    public class BaseEntity
    {
        public int Id { get; set; }
        public DateTime AddedDate { get; set; }=DateTime.UtcNow;
        public bool IsDeleted { get; set; }
        public DateTime? DeletedDate { get; set; } 
        public DateTime? UpdatedDate { get; set; }
        
    }
}
