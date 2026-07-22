namespace ECommerce.Core.Entities
{
    public class BaseEntity
    {
        public int Id { get; set; }
        public DateTime AddedDate { get; set; }=DateTime.Now;
        public bool IsDeleted { get; set; }
        public DateTime? DeletedDate { get; set; } 
        public DateTime? UpdatedDate { get; set; }
        
    }
}
