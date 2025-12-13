
using MauiAppProducts;
using System.Text.Json;


public class DBService
{

    private readonly string _categoriesFile;
    private readonly string _productsFile;

    public List<Category> _categories = new();
    public List<Product> _products = new();

    private int _nextProductId = 1;
    private int _nextCategoriesId = 1;
    private List<int> AutoIncr { get; set; } = new List<int> { 1, 1 };
    private List<Task> loadTasks = new List<Task>();
    public DBService()
    {
        InitializeData();
        _categoriesFile = Path.Combine(FileSystem.Current.AppDataDirectory, "dbCategory.json");
        _productsFile = Path.Combine(FileSystem.Current.AppDataDirectory, "dbProduct.json");

    }
    private async void InitializeData()
    {
        //_categories = new List<Category>
        //{
        //    new Category { Id = _nextCategoriesId++, CategoryName = "Название категории", CategoryDescription = "Описание" },
         
        //};
        //_products = new List<Product>
        //{
        //    new Product {Id = _nextProductId++, Name="Название продукта", Description="Описание продукта", CategoryId=1, Price=0,},
         

        //};
        Task loadId = LoadId();
        Task loadCategory = LoadCategory();
        Task loadProduct = LoadProduct();
        loadTasks = new List<Task> { loadId, loadCategory, loadProduct };
        await loadId;
        await loadCategory;
        await loadProduct;
        //SaveId();
        //SaveCategory();
        //SaveProduct();

    }
    public async Task<List<Category>> GetAllCategoriesAsync()
    {
        await Task.Delay(500);
        if (!File.Exists(_categoriesFile)) return new List<Category>();
        var json = await File.ReadAllTextAsync(_categoriesFile);
        return JsonSerializer.Deserialize<List<Category>>(json) ?? new List<Category>();
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
        var all = await GetAllCategoriesAsync();

        // Генерируем новый Id
        category.Id = all.Count > 0 ? all.Max(c => c.Id) + 1 : 1;

        all.Add(category);

        var json = JsonSerializer.Serialize(all, new JsonSerializerOptions { WriteIndented = true });
        await File.WriteAllTextAsync(_categoriesFile, json);
    }
    public async Task UpdateCategoryAsync(int newcategoryid, Category categoryupd)
    {
        await Task.Delay(500);
        foreach (Category category1 in _categories)
        {
            if (category1.Id==newcategoryid)
            {
                category1.CategoryName = categoryupd.CategoryName;
                category1.CategoryDescription = categoryupd.CategoryDescription;
                SaveCategory();
                break;
            }
        }
        
    }
    public async Task DeleteCategoryAsync(int id)
    {
        await Task.Delay(500);
        Category categorydel = new Category();
        foreach (Category category in _categories)
        {
            if (category.Id==id)
            {
                categorydel = category;
            }
        }
        _categories.Remove(categorydel);
        SaveCategory();
    }






    public async Task<List<Product>> GetAllProductsAsync()
    {
        if (!File.Exists(_productsFile)) return new List<Product>();
        var json = await File.ReadAllTextAsync(_productsFile);
        return JsonSerializer.Deserialize<List<Product>>(json) ?? new List<Product>();

    }
    public async Task AddProductAsync(Product product)
    {
        await Task.Delay(500);
        var all = await GetAllProductsAsync();

        // Генерируем ID
        product.Id = all.Count > 0 ? all.Max(p => p.Id) + 1 : 1;

        all.Add(product);

        // 🔑 СОХРАНЯЕМ В ФАЙЛ!
        var json = JsonSerializer.Serialize(all, new JsonSerializerOptions { WriteIndented = true });
        await File.WriteAllTextAsync(_productsFile, json);
    }
    private async Task<List<Product>> GetAllProductsInternalAsync()
    {
        string filepath = Path.Combine(FileSystem.Current.AppDataDirectory, "dbProduct.json");

        if (!File.Exists(filepath))
            return new List<Product>();

        var json = await File.ReadAllTextAsync(filepath);
        return JsonSerializer.Deserialize<List<Product>>(json) ?? new List<Product>();
    }
    public async Task<List<Product>> GetProductsByCategoryAsync(int categoryId)
    {
        var all = await GetAllProductsAsync();
        string filepath = Path.Combine(FileSystem.Current.AppDataDirectory, "dbProduct.json");

        if (!File.Exists(filepath))
            return new List<Product>();

        var json = await File.ReadAllTextAsync(filepath);
        var allProducts = JsonSerializer.Deserialize<List<Product>>(json) ?? new List<Product>();

        // Фильтрация по CategoryId
        var filtered = allProducts.Where(p => p.CategoryId == categoryId).ToList();

        Console.WriteLine($"[DEBUG] Найдено {filtered.Count} продуктов для CategoryId={categoryId}");

        return filtered;
    }

    public async Task<bool> IsCategoryHasProductsAsync(int categoryId)
    {
        var products = await GetProductsByCategoryAsync(categoryId);
        return products.Any();
    }


    public async Task UpdateProductAsync(int productId, Product upproduct)
    {
        await Task.Delay(500);
       foreach(Product product1 in _products)
        {
            if(product1.Id == productId)
            {
                product1.Name = upproduct.Name;
                product1.Description = upproduct.Description;
                product1.Price = upproduct.Price;
                SaveProduct();
                break;
            }
        }
        
        
    }

    public async Task DeleteProductAsync(int id)
    {
        await Task.Delay(500);

        Product productdel = new Product();
        foreach(Product product in _products)
        {
            if (product.Id == id)
            {
                productdel = product;
            }
            _products.Remove(productdel);
            SaveProduct();
        }
    }




    public async Task <List<Category>> GetCategoriesAsync()
    {
        await Task.Delay(500);
        return _categories.ToList();
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