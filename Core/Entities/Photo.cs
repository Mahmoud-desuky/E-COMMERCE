namespace ECommerce.Core.Entities
{
    public class Photo : BaseEntity
    {
        public string ImageName { get; set; }
        public int ProductId { get; set; }
        public Product Product { get; set; }
    }
}