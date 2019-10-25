using Mobile.IoTMonitor.ViewModels;
using Xamarin.Forms;

namespace Mobile.IoTMonitor
{
    public partial class MainPage : ContentPage
    {
        private MainViewModel _vm;

        public MainPage()
        {
            InitializeComponent();
            BindingContext = Vm;
        }

        internal MainViewModel Vm => _vm ?? (_vm = new MainViewModel());

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            await Vm.Init();
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            Vm.Dispose();
        }
    }
}
