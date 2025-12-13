namespace MauiAppProducts;

public partial class AddCategory : ContentPage
{
    private DBService dBService;
    public Category CategoryHere { get; set; } = new Category();
   
    public AddCategory(DBService dbService)
    {
        InitializeComponent();
        dBService = dbService;
        BindingContext = this;

    }
   

    public async void Save_click(object sender, EventArgs e)
    {
        await dBService.AddCategoryAsync(CategoryHere);
        await Navigation.PopAsync(); 
 
    }

    private async void Close_click(object sender, EventArgs e)
    {
        await Navigation.PopAsync();
    }

   
}