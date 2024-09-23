using System;
using System.Collections.ObjectModel;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using de2024.Model;

namespace de2024;

public partial class TechnicianWindow : Window
{
    public ObservableCollection<Order>? order { get; set; }
    private Global _global = new Global();
    
    public TechnicianWindow()
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
    
    
    // order = [new Order {Orderstatus = "null", Amountguests = 0, Datecreation = DateTime.Now, Equipment = "null", Nameconference = "null"}];
}