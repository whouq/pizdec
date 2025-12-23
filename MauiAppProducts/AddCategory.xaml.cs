

namespace MauiAppProducts;

public partial class AddCategory : ContentPage
{
    private DBService? dBService;
    private Category? _categoryToEdit;
    public Category CategoryHere { get; set; } = new Category();
   
    public AddCategory(DBService dbService)
    {
        InitializeComponent();
        dBService = dbService;
        BindingContext = this;
        LoadList();
    }

    public AddCategory(Category category)
    {
        InitializeComponent();
        _categoryToEdit = category;

        NameEntry.Text = category.CategoryName;
        DescriptionEntry.Text = category.CategoryDescription;

        LoadList();
    }

    private async Task LoadList()
    {
        dBService = await DBService.GetDB();
    }
    public async void Save_click(object sender, EventArgs e)
    {
        if (dBService == null) return;

        try
        {
            string name = NameEntry.Text?.Trim() ?? "";
            string description = DescriptionEntry.Text?.Trim() ?? "";

            if (string.IsNullOrEmpty(name))
            {
                await DisplayAlert("Ошибка", "Введите название категории", "ОК");
                return;
            }

            if (_categoryToEdit != null)
            {
                // Редактируем существующую
                _categoryToEdit.CategoryName = name;
                _categoryToEdit.CategoryDescription = description;

                await dBService.UpdateCategoryAsync(CategoryHere.Id, CategoryHere);
            }
            else
            {
                // Создаём новую
                var newCategory = new Category
                {
                    CategoryName = name,
                    CategoryDescription = description
                };

                await dBService.AddCategoryAsync(CategoryHere);
            }

            await Navigation.PopAsync(); // Возвращаемся
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ошибка сохранения категории: {ex.Message}");
            await DisplayAlert("Ошибка", "Не удалось сохранить категорию", "ОК");
        }

    }

    private async void Close_click(object sender, EventArgs e)
    {
        await Navigation.PopAsync();
    }

   
}