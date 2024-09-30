using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Net;
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
    public ObservableCollection<Order>? orders { get; set; }
    public List<string> orderStatuses { get; set; } = ["готов", "готовится"];
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
        orders = await httpClient.GetFromJsonAsync<ObservableCollection<Order>>(path + "/orders");
        ListBox.ItemsSource = orders;
    }

    public async Task<HttpStatusCode> UpdateOrder(string path, Order order)
    {
        using var httpClient = new HttpClient();
        var content = JsonContent.Create(order);
        var request = new HttpRequestMessage(HttpMethod.Put, _global._url + path);
        {
            request.Content = content;
        }
        var response = await httpClient.SendAsync(request);
        response.EnsureSuccessStatusCode();
        
        return response.StatusCode;
    }
    
    
    // order = [new Order {Orderstatus = "null", Amountguests = 0, Datecreation = DateTime.Now, Equipment = "null", Nameconference = "null"}];
    private async void OrderStatus_OnSelectionChanged(object? sender, SelectionChangedEventArgs e)
    {
        Order order = ListBox.SelectedItem as Order;
        
        if(order != null)
            await UpdateOrder("/orders/", order);
    }
}