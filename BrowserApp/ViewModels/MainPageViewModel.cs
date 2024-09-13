using BrowserApp.Helper;
using BrowserApp.Interfaces;
using BrowserApp.PartialClasses;
using BrowserApp.Services;
using Microsoft.UI.Xaml.Controls;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Threading.Tasks;

namespace BrowserApp.ViewModels
{
    public class MainPageViewModel : INotifyPropertyChanged
    {

        public MainPageViewModel()
        {
            _dialogService = AppServices.GetService<IDialogService>();
        }


        #region [Properties]

        private WebView2 _currentWebView;
        private bool _canGoBack;
        private bool _canGoForward;
        private string _faviconUri;
        private TabViewItem _currentTab;

        public WebView2 CurrentWebView
        {
            get { return _currentWebView; }
            set
            {
                _currentWebView = value;
                OnPropertyChanged(nameof(CurrentWebView));

                CanGoBack = _currentWebView?.CanGoBack ?? false;
                CanGoForward = _currentWebView?.CanGoForward ?? false;
            }
        }

        public bool CanGoBack
        {
            get { return _canGoBack; }
            set
            {
                _canGoBack = value;
                OnPropertyChanged(nameof(CanGoBack));
            }
        }

        public bool CanGoForward
        {
            get { return _canGoForward; }
            set
            {
                _canGoForward = value;
                OnPropertyChanged(nameof(CanGoForward));
            }
        }
        private ObservableCollection<History> _historyTabCollection = new ObservableCollection<History>();

        private IDialogService _dialogService;

        public ObservableCollection<History> HistoryTabCollection
        {
            get { return _historyTabCollection; }
            set
            {
                _historyTabCollection = value;
                OnPropertyChanged(nameof(HistoryTabCollection));
            }
        }
        public string FaviconUri
        {
            get { return _faviconUri; }
            set
            {
                _faviconUri = value;
                OnPropertyChanged(nameof(FaviconUri));
            }
        }

        public TabViewItem CurrentTab
        {
            get { return _currentTab; }
            set
            {
                _currentTab = value;
                OnPropertyChanged(nameof(CurrentTab));
            }
        }

        #endregion

        #region [Commands]

        public IAsyncCommand BackCommand => new AsyncCommand(BackCommandExecuteAsync);
        public IAsyncCommand ForwardCommand => new AsyncCommand(ForwardCommandExecuteAsync);
        public IAsyncCommand RefreshCommand => new AsyncCommand(RefreshCommandExecuteAsync);
        public IAsyncCommand DownloadCommand => new AsyncCommand(DownloadCommandExecuteAsync);
        public IAsyncCommand UpdateTabIconCommand => new AsyncCommand(UpdateTabIconCommandExecuteAsync);

        #endregion 


        #region [Methods]

        private async Task BackCommandExecuteAsync()
        {
            _currentWebView?.GoBack();
        }


        private async Task ForwardCommandExecuteAsync()
        {
            _currentWebView?.GoForward();
        }
        private async Task RefreshCommandExecuteAsync()
        {
            _currentWebView.Reload();
        }
        private async Task DownloadCommandExecuteAsync()
        {
            await _dialogService.OpenDownloadPopupAsync();
        }
        public void UpdateNavigationStates()
        {
            CanGoBack = _currentWebView?.CanGoBack ?? false;
            CanGoForward = _currentWebView?.CanGoForward ?? false;

            UpdateTabIconCommandExecuteAsync();
            UpdateHistory();
            ((AsyncCommand)BackCommand).RaiseCanExecuteChanged();
            ((AsyncCommand)ForwardCommand).RaiseCanExecuteChanged();
        }
        private async void UpdateHistory()
        {
            if (CurrentWebView != null)
            {
                var history = new History();
                if (CurrentWebView.CoreWebView2.FaviconUri == string.Empty)
                {
                    history.Icon = "https://www.google.com/favicon.ico";
                }
                else
                {

                    history.Icon = CurrentWebView.CoreWebView2.FaviconUri;
                }
                history.Title = CurrentWebView.CoreWebView2.DocumentTitle;
                history.Host = CurrentWebView.Source.Host;
                history.WebLink = CurrentWebView.CoreWebView2.Source;
                HistoryTabCollection.Add(history);
            }
        }
        private async void UpdateTabIconCommandExecuteAsync()
        {
            if (CurrentWebView != null)
            {
                FaviconUri = CurrentWebView.CoreWebView2.FaviconUri;
                CurrentTab.Header = CurrentWebView.CoreWebView2.DocumentTitle;
                if (CurrentTab != null && !string.IsNullOrEmpty(FaviconUri))
                {
                    CurrentTab.IconSource = new BitmapIconSource
                    {
                        UriSource = new Uri(FaviconUri),
                        ShowAsMonochrome = false
                    };
                }
            }
        }

        #endregion 

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
