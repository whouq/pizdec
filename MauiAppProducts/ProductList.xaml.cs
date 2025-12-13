




using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace MauiAppProducts;

public partial class ProductList : ContentPage

{
    private readonly DBService dBService;
    public Category SelectedCategory { get; }
    public ObservableCollection<Product> Products { get; } = new();
    public Product? SelectedProduct { get; set; }


  

    public ProductList(DBService dBService, Category selectedCategory)
	{
		InitializeComponent();
        this.dBService = dBService;
        SelectedCategory = selectedCategory;
        BindingContext = this;
        LoadList();
    

    }
    protected async override void OnAppearing()
    {
        base.OnAppearing();
        LoadProducts();
    }
    public async Task LoadProducts()
    {
        try
        {
            Products.Clear();

            var categoryId = SelectedCategory?.Id ?? 0;
            Console.WriteLine($"[DEBUG] Загружаем продукты для CategoryId = {categoryId}");

            var products = await dBService.GetProductsByCategoryAsync(categoryId);

            Console.WriteLine($"[DEBUG] Найдено {products.Count} продуктов");

            foreach (var p in products)
            {
                Console.WriteLine($"  - '{p.Name}' (CategoryId: {p.CategoryId}, Id: {p.Id})");
                Products.Add(p);
            }

            Title = $"Продукты: {SelectedCategory?.CategoryName ?? "Неизвестно"}";
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[ERROR] Ошибка загрузки: {ex}");
            await DisplayAlert("Ошибка", ex.Message, "OK");
        }
    }
    
    public async void LoadList()
    {
        ListViewProduct.ItemsSource = await dBService.GetAllProductsAsync();
        Products.Clear();

        // Получаем ТОЛЬКО продукты текущей категории
        var products = await dBService.GetProductsByCategoryAsync(SelectedCategory.Id);

        foreach (var p in products)
            Products.Add(p);
    }

    




    public int DeleteId { get; set; } = 0;
    public Product ProductHere { get; set; } = new Product();

    private async void EditProduct_click(object sender, EventArgs e)
    {
        await dBService.UpdateProductAsync(DeleteId, ProductHere);
        await Navigation.PushAsync(new AddProduct(dBService, SelectedCategory));
        LoadList();
    }




    private async void DeleteProduct_click(object sender, EventArgs e)
    {
        if (SelectedProduct == null) return;
        try
        {
         await dBService.DeleteProductAsync(SelectedProduct.Id);
         LoadList();

        }
        catch 
        {
            return;
        }
        
        

    }

    private async void AddProduct_click(object sender, EventArgs e)
    {
     
        await Navigation.PushAsync(new AddProduct(dBService, SelectedCategory));
    }


    private async void Back_click(object sender, EventArgs e)
    {
        await Navigation.PopAsync();
    }

    
}