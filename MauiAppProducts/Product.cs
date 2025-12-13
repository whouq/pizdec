using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace MauiAppProducts
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int CategoryId { get; set; }

        public decimal Price { get; set; }
        [JsonIgnore] // ← Не сериализовать, чтобы избежать циклов
        public Category? Category { get; set; }
    }
}
