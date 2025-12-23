
namespace MauiAppProducts;

public partial class AddProduct : ContentPage
{
    private DBService? dBService; 
    private readonly Category _category;
    private Product? _productToEdit;
    public Product ProductHere { get; set; } = new Product();
 



public AddProduct(DBService dbService, Category category)
	{
		InitializeComponent();
		dBService = dbService;
        ProductHere.CategoryId = category.Id;

        BindingContext = this;
        LoadList();
	}

    public AddProduct(DBService dbService, Category category, Product productToEdit)
           : this(dbService, category)
    {
        _productToEdit = productToEdit;
        ProductNameEntry.Text = productToEdit.Name;
        ProductDescriptionEntry.Text = productToEdit.Description;
        ProductPriceEntry.Text = productToEdit.Price.ToString();
    }
    private async Task LoadList()
    {
        dBService = await DBService.GetDB();
    }
    public async void Save_click (object sender, EventArgs e)
	{


        if (dBService == null) return;

   
        ProductHere.Name = ProductNameEntry.Text ?? string.Empty;
        ProductHere.Description = ProductDescriptionEntry.Text ?? string.Empty;
        if (decimal.TryParse(ProductPriceEntry.Text, out decimal price))
            ProductHere.Price = price;

        try
        {
            if (_productToEdit != null)
            {
               
                await dBService.UpdateProductAsync(ProductHere.Id, ProductHere);
            }
            else
            {
              
                await dBService.AddProductAsync(ProductHere);
            }

            await Navigation.PopAsync(); 
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ошибка сохранения: {ex.Message}");
           
        }

    }

    private async void Close_click(object sender, EventArgs e)
    {
		await Navigation.PopAsync();
    }

   
}