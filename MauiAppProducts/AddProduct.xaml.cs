

namespace MauiAppProducts;

public partial class AddProduct : ContentPage
{
	private DBService dBService;
    private readonly Product _product;
    public Product ProductHere { get; set; } = new Product();
 



public AddProduct(DBService dbService, Category category)
	{
		InitializeComponent();
		dBService = dbService;
        ProductHere.CategoryId = category.Id;

        BindingContext = this;

	}
	

	public async void Save_click (object sender, EventArgs e)
	{
    
       
        await dBService.AddProductAsync(ProductHere);
        await Navigation.PopAsync();

    }

    private async void Close_click(object sender, EventArgs e)
    {
		await Navigation.PopAsync();
    }

   
}