

using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace MauiAppProducts;

public partial class ProductList : ContentPage

{
    private DBService? dBService; 
    public Category SelectedCategory { get; }
    public ObservableCollection<Product> Products { get; } = new();
    public Product? SelectedProduct { get; set; }




    public ProductList(DBService dBService, Category selectedCategory)
	{
		InitializeComponent();
       
        SelectedCategory = selectedCategory;
        BindingContext = this;
        LoadList();
    

    }
    protected async override void OnAppearing()
    {
        base.OnAppearing();
        await LoadList();
    }
    
    
    public async Task LoadList()
    {
        try
        {
            dBService = await DBService.GetDB();
            Products.Clear();

            var products = await dBService.GetProductsByCategoryAsync(SelectedCategory.Id);

            foreach (var product in products)
            {
                Products.Add(product);
            }

            ListViewProduct.ItemsSource = Products;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ошибка загрузки продуктов: {ex.Message}");

        }
    }

    




    public int DeleteId { get; set; } = 0;
    public Product ProductHere { get; set; } = new Product();

    private async void EditProduct_click(object sender, EventArgs e)
    {
        if (SelectedProduct == null) return;

     
        await Navigation.PushAsync(new AddProduct(dBService, SelectedCategory, SelectedProduct));
        await LoadList(); 
    }




    private async void DeleteProduct_click(object sender, EventArgs e)
    {
        if (SelectedProduct == null) return;

        try
        {
            await dBService.DeleteProductAsync(SelectedProduct.Id);
            await LoadList(); 
        }
        catch
        {
            return;
        }



    }

    private async void AddProduct_click(object sender, EventArgs e)
    {

        await Navigation.PushAsync(new AddProduct(dBService, SelectedCategory));
        await LoadList(); 
    }


    private async void Back_click(object sender, EventArgs e)
    {
        await Navigation.PopAsync();
    }

    
}