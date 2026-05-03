using ByteSentinelApp.ViewModels;

namespace ByteSentinelApp
{
    public partial class MainPage : ContentPage
    {
        private readonly DashboardViewModel _vm = new();

        public MainPage()
        {
            InitializeComponent();
            BindingContext = _vm;
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();

            await _vm.LoadAgents(); // 🔥 AQUI QUE FALTAVA
        }
    }
}