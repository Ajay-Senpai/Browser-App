using BrowserApp.ViewModels;
using Microsoft.UI.Xaml.Controls;
using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using NavigationViewItem = Windows.UI.Xaml.Controls.NavigationViewItem;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace BrowserApp.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPageView : Page
    {
        public MainPageViewModel ViewModel { get; set; }
        public MainPageView()
        {
            ViewModel = new MainPageViewModel();
            this.DataContext = ViewModel;
            this.InitializeComponent();
        }
        private async void TabView_Loaded(object sender, RoutedEventArgs e)
        {
            await CreateNewTab();
        }

        private async void TabView_AddButtonClick(TabView sender, object args)
        {
            await CreateNewTab();
        }

        private void TabView_TabCloseRequested(TabView sender, TabViewTabCloseRequestedEventArgs args)
        {
            sender.TabItems.Remove(args.Tab);
        }

        private async Task<TabViewItem> CreateNewTab()
        {
            try
            {
                TabViewItem newItem = new TabViewItem();

                newItem.Header = "New Tab";
                newItem.IconSource = new Microsoft.UI.Xaml.Controls.SymbolIconSource() { Symbol = Symbol.Document };

                var newWebView = new WebView2();

                await newWebView.EnsureCoreWebView2Async();
                newWebView.Source = new Uri("https://www.google.com/");

                newItem.Content = newWebView;
                MyTabView.TabItems.Add(newItem);
                MyTabView.SelectedItem = newItem;

                return newItem;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error creating new tab: {ex.Message}");
                throw;
            }
        }

        private void NavigationView_SelectionChanged(Windows.UI.Xaml.Controls.NavigationView sender, Windows.UI.Xaml.Controls.NavigationViewSelectionChangedEventArgs args)
        {
            if (args.SelectedItemContainer != null)
            {
                var selectedItem = (NavigationViewItem)args.SelectedItemContainer;
                var selectedTag = selectedItem.Tag.ToString();

                HomeGrid.Visibility = Visibility.Collapsed;
                BrowserGrid.Visibility = Visibility.Collapsed;
                HistoryGrid.Visibility = Visibility.Collapsed;
                BookmarksGrid.Visibility = Visibility.Collapsed;
                SettingsGrid.Visibility = Visibility.Collapsed;

                switch (selectedTag)
                {
                    case "HomeGrid":
                        HomeGrid.Visibility = Visibility.Visible;
                        break;

                    case "BrowserGrid":
                        BrowserGrid.Visibility = Visibility.Visible;
                        break;

                    case "HistoryGrid":
                        HistoryGrid.Visibility = Visibility.Visible;
                        break;

                    case "BookmarksGrid":
                        BookmarksGrid.Visibility = Visibility.Visible;
                        break;

                    case "SettingsGrid":
                        SettingsGrid.Visibility = Visibility.Visible;
                        break;
                }
            }
        }
    }
}
