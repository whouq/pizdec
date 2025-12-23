

using MauiAppProducts;
using System.Text.Json;


public class DBService
{
    private static DBService _instance;
    public List<Category> _categories = new();
    public List<Product> _products = new();

    private int _nextProductId = 1;
    private int _nextCategoriesId = 1;

    private List<int> AutoIncr { get; set; } = new List<int> { 1, 1 };
    private List<Task> loadTasks = new List<Task>();
    public DBService()
    {
        InitializeData();


    }
    private async void InitializeData()
    {
        
        Task loadId = LoadId();
        Task loadCategory = LoadCategory();
        Task loadProduct = LoadProduct();
        loadTasks = new List<Task> { loadId, loadCategory, loadProduct };
        await loadId;
        await loadCategory;
        await loadProduct;
       

    }
    public static async Task<DBService> GetDB()
    {
        if (_instance == null)
        {
            _instance = new DBService();
            await _instance.InitializeAsync(); 
        }
        return _instance;
    }
    private async Task InitializeAsync()
    {
        await Task.Delay(500); 
    }
    public async Task<List<Category>> GetAllCategoriesAsync()
    {
        await Task.Delay(500);
        return _categories.ToList();
    }
    public async void SaveCategory()
    {
        await Task.Delay(500);
        string filepath = Path.Combine(FileSystem.Current.AppDataDirectory, "dbCategory.json");
        using FileStream fileStream=File.Create(filepath);
        JsonSerializer.Serialize(fileStream, _categories);
    }
    public async void SaveProduct()
    {
        await Task.Delay(500);
        string filepath = Path.Combine(FileSystem.Current.AppDataDirectory, "dbProduct.json");
        using FileStream fileStream = File.Create(filepath);
        JsonSerializer.Serialize(fileStream, _products);
    }
    public async Task<List<Product>> LoadProduct()
    {
        string filepath = Path.Combine(FileSystem.Current.AppDataDirectory, "dbProduct.json");
        if (!File.Exists(filepath))
        {
            _products = new List<Product>();
            return new List<Product>();
        }
        var data1 = await File.ReadAllTextAsync(filepath);
       _products = JsonSerializer.Deserialize<List<Product>>(data1);
        return new List<Product>(_products);
    }
    public async Task<List<Category>> LoadCategory()
    {
        string filepath = Path.Combine(FileSystem.Current.AppDataDirectory, "dbCategory.json");
        if (!File.Exists(filepath)) { _categories = new List<Category>();
            return new List<Category>();
        }
        var data1 = await File.ReadAllTextAsync(filepath);
        _categories = JsonSerializer.Deserialize<List<Category>>(data1);
        return new List<Category>(_categories);
    }

    public async Task AddCategoryAsync(Category category)
    {
        await Task.Delay(500);
        category.Id = _categories.Count > 0 ? _categories.Max(c => c.Id) + 1 : 1;
        _categories.Add(category);
    }
    public async Task UpdateCategoryAsync(int categoryId, Category updated)
    {
        await Task.Delay(500);
        var cat = _categories.FirstOrDefault(c => c.Id == categoryId);
        if (cat != null)
        {
            cat.CategoryName = updated.CategoryName;
            cat.CategoryDescription = updated.CategoryDescription;
        }

    }
    public async Task DeleteCategoryAsync(int categoryId)
    {
        await Task.Delay(500);
        _categories.RemoveAll(c => c.Id == categoryId);
        _products.RemoveAll(p => p.CategoryId == categoryId);
    }






    public async Task<List<Product>> GetAllProductsAsync()
    {
        await Task.Delay(500);
        return _products.ToList();

    }
    public async Task AddProductAsync(Product product)
    {
        await Task.Delay(500);
        product.Id = _products.Count > 0 ? _products.Max(p => p.Id) + 1 : 1;
        _products.Add(product);
    }
   
    public async Task<List<Product>> GetProductsByCategoryAsync(int categoryId)
    {
        await Task.Delay(500);
        return _products.Where(p => p.CategoryId == categoryId).ToList();
    }

    public async Task<bool> IsCategoryHasProductsAsync(int categoryId)
    {
        await Task.Delay(500);
        return _products.Any(p => p.CategoryId == categoryId);
    }


    public async Task UpdateProductAsync(int productId, Product updated)
    {
        await Task.Delay(500);
        var prod = _products.FirstOrDefault(p => p.Id == productId);
        if (prod != null)
        {
            prod.Name = updated.Name;
            prod.Description = updated.Description;
            prod.Price = updated.Price;
            prod.CategoryId = updated.CategoryId;
        }


    }

    public async Task DeleteProductAsync(int id)
    {
        await Task.Delay(500);
        _products.RemoveAll(p => p.Id == id);
    }


    public async Task<Category> GetAllCategoryId(int Id)
    {
        await Task.Delay(500);
        return _categories.FirstOrDefault(c => c.Id == Id);

    }

    public async Task<Product> GetAllProductId(int Id)
    {
        await Task.Delay(500);
        return _products.FirstOrDefault(p => p.Id == Id);

    }
  
  
    public async void SaveId()
    {
        string filepath = Path.Combine(FileSystem.Current.AppDataDirectory, "dbAutoIncr.json");
        using FileStream fileStream = File.Create(filepath);
        JsonSerializer.Serialize(fileStream, AutoIncr);
    }
    public async Task LoadId()
    {
        string filepath = Path.Combine(FileSystem.Current.AppDataDirectory, "dbAutoIncr.json");
        if (!File.Exists(filepath))
        {
            AutoIncr = new List<int> { 1, 1 };
            return;
        }
        var data1= await File.ReadAllTextAsync(filepath);
        AutoIncr = JsonSerializer.Deserialize<List<int>>(data1);
    }

    

    private async Task Save()
    {

       string _categoriesFile = Path.Combine(FileSystem.Current.AppDataDirectory, "categoriesFile.db");
       string _productsFile = Path.Combine(FileSystem.Current.AppDataDirectory, "productsFile.db");

        string json = JsonSerializer.Serialize(_categories);
        await File.WriteAllTextAsync(_categoriesFile, json);

        string json1 = JsonSerializer.Serialize(_products);
        await File.WriteAllTextAsync(_productsFile, json);

    }

    private async Task LoadAsync()
    {
        await Task.Run(() => LoadAsync());
        
    }
}