




using System.Threading.Tasks;

namespace MauiAppProducts
{
    public partial class MainPage : ContentPage
    {
        
        private readonly DBService dBService = new DBService ();

        
        public Category SelectedCategory { get; set; }

        public MainPage()
        {
            InitializeComponent();
            BindingContext = this;
        }

        
        protected override async void OnAppearing()
        {
            base.OnAppearing();
            LoadList();
 
        }

      private async void LoadList()
        {
            ListViewCategory.ItemsSource = await dBService.GetAllCategoriesAsync();

        }


        private async void  AddCategory_click(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new AddCategory(dBService));
          
        }



        public int DeleteId { get; set; } = 0;
        public Category CategoryHere { get; set; } = new Category();


        private async void EditCategory_click(object sender, EventArgs e)
        {
           await dBService.UpdateCategoryAsync(DeleteId, CategoryHere);
            await Navigation.PushAsync(new AddCategory(dBService));
            LoadList();
        }


        private async void DeleteCategory_click(object sender, EventArgs e)
        {
         

            // 🔹 Проверка: есть ли продукты в категории?
            bool hasProducts = await dBService.IsCategoryHasProductsAsync(SelectedCategory.Id);

            if (hasProducts)
            {
                await DisplayAlert(
                    "Нельзя удалить",
                    $"Категория «{SelectedCategory.CategoryName}» содержит продукты. Удалите их сначала.",
                    "OK"
                );
                return;
            }

            // Подтверждение
            bool confirm = await DisplayAlert(
                "Подтверждение",
                $"Удалить категорию «{SelectedCategory.CategoryName}»?",
                "Да", "Нет"
            );

            if (!confirm) return;
            try
            {
                await dBService.DeleteCategoryAsync(SelectedCategory.Id);
                LoadList();
            }

            catch  
            {
                return;
            }
       
        }

        private async void ToProduct_click(object sender, EventArgs e)
        {
            if (SelectedCategory == null)
            {
                await DisplayAlert("Ошибка", "Выберите категорию!", "ОК");
                return;
            }

            var productList = new ProductList(dBService, SelectedCategory);
            await Navigation.PushAsync(productList);
        }
    }
}
