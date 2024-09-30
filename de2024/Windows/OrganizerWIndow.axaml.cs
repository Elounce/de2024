using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Net;
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
    public List<string> paymentStatus { get; set; } = ["принят", "не принят"];
    private Global _global = new Global();
    private NewOrderWindow _newOrderWindow = new NewOrderWindow();

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

    public async Task<HttpStatusCode> UpdateOrder(string path, Order order)
    {
        try
        {
            using HttpClient httpClient = new HttpClient();
            var content = JsonContent.Create(order);
            var request = new HttpRequestMessage(HttpMethod.Put, _global._url + path);
            {
                request.Content = content;
            }
            var response = await httpClient.SendAsync(request);
            response.EnsureSuccessStatusCode();
            
            return response.StatusCode;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    private void Button_OnClick(object? sender, RoutedEventArgs e)
    {
        _newOrderWindow.Show(this);
    }

    private async void PayStatusComboBox_OnSelectionChanged(object? sender, SelectionChangedEventArgs e)
    {
        Order order = ListBox.SelectedItem as Order;

        if (order != null)
            await UpdateOrder("/orders/", order);
    }
}