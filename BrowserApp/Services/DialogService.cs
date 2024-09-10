using BrowserApp.Dialogs.Views;
using BrowserApp.Interfaces;
using System;
using System.Threading.Tasks;

namespace BrowserApp.Services
{
    public class DialogService : IDialogService
    {
        public async Task OpenDownloadPopupAsync()
        {
            var dialog = new DownloadPopupDialogView();
            await dialog.ShowAsync();
        }
    }
}
