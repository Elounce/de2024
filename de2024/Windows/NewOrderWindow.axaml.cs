using System;
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

public partial class NewOrderWindow : Window
{
    private Global _global = new Global();
    private MessageBox _messageBox = new MessageBox();
    
    public NewOrderWindow()
    {
        InitializeComponent();
    }
    
    public async Task<HttpStatusCode> ApiPost(Order order)
    {
        using HttpClient httpClient = new HttpClient();
        var content = JsonContent.Create(order);
        var request = new HttpRequestMessage(HttpMethod.Post, _global._url + "/order")
        {
            Content = content
        };
        var response = await httpClient.SendAsync(request);
        response.EnsureSuccessStatusCode();
        
        return response.StatusCode;
    }

    private async void AddNewOrder(object? sender, RoutedEventArgs e)
    {
        Order order = new Order()
        {
            Nameconference = NameConferance.Text,
            Amountguests = Convert.ToInt32(AmountGuests.Text),
            Datecreation = Convert.ToDateTime(Time.SelectedDate),
            Equipment = Equipment.Text,
            Paymentstatus = PaymentStatus.Text
        };
        
        if (await ApiPost(order) == HttpStatusCode.OK)
        {
            Close(this);
        }
        else
        {
            _messageBox.TextBox.Text = "Чёт с не так при добавлении юзера";
            _messageBox.Show();
        }
    }
}