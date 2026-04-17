using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace StockManager.API.Entities.Models.Catalog
{
    public class Product
    {
        public int Id { get; set; }
        public required string ProviderCode { get; set; }
        public required string ProductCode { get; set; }
        public required string Name { get; set; }
        public required decimal Price { get; set; }
        public required int Stock { get; set; }
        public ICollection<Category> Categories { get; set; } = [];
        [Display(Name = "Image")]
        public string? UrlPhoto { get; set; }
        public bool StatusActived { get; set; } = true;
        [JsonIgnore]
        public Provider Provider { get; set; } = null!;
    }
}
