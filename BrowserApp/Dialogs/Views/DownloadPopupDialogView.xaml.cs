using BrowserApp.Dialogs.ViewModels;
using Windows.UI.Xaml.Controls;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace BrowserApp.Dialogs.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class DownloadPopupDialogView : ContentDialog
    {
        public DownloadPopupDialogViewModel ViewModel { get; set; }
        public DownloadPopupDialogView()
        {
            ViewModel = new DownloadPopupDialogViewModel();
            DataContext = ViewModel;
            this.InitializeComponent();
        }
    }
}
