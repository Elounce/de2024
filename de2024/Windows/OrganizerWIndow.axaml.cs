using System.Collections.ObjectModel;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using de2024.Model;

namespace de2024.Windows;

public partial class OrganizerWIndow : Window
{
    public ObservableCollection<Order>? order { get; set; }
    private Global _global = new Global();
    private NewUserWindow _newUserWindow;
    
    public OrganizerWIndow()
    {
        DataContext = this;
        InitializeComponent();
        _ = GetOrders(_global._url);
    }
    
    public async Task GetOrders(string path)
    {
        using var httpClient = new HttpClient();
        order = await httpClient.GetFromJsonAsync<ObservableCollection<Order>>(path + "/orders");
        ListBox.ItemsSource = order;
    }

    private void Button_OnClick(object? sender, RoutedEventArgs e)
    {
        throw new System.NotImplementedException();
    }
}