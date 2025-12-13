using System.Collections.ObjectModel;
using System.Text.Json.Serialization;

namespace MauiAppProducts
{
    public class Category
    {
        public int Id { get; set; }
        public string CategoryName { get; set; }
        public string CategoryDescription { get; set; }
        [JsonIgnore]
        public ObservableCollection<Product> Products { get; set; }
       
       
    }
}
