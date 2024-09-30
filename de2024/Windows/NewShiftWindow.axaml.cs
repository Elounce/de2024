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

public partial class NewShiftWindow : Window
{
    private readonly Global _global = new Global();
    
    public NewShiftWindow()
    {
        InitializeComponent();
    }
    
    public async Task<HttpStatusCode> CreateShift(Shift shift)
    {
        using HttpClient httpClient = new HttpClient();
        var content = JsonContent.Create(shift);
        var request = new HttpRequestMessage(HttpMethod.Post, _global._url + "/shifts")
        {
            Content = content
        };
        var response = await httpClient.SendAsync(request);
        response.EnsureSuccessStatusCode();
        
        return response.StatusCode;
    }

    private async void AddNewShift(object? sender, RoutedEventArgs e)
    {
        Shift shift = new Shift()
        {
            Datestart = Convert.ToDateTime(DateStart.SelectedDate), 
            Dateend = Convert.ToDateTime(DateEnd.SelectedDate),
        };

        if (await CreateShift(shift) == HttpStatusCode.OK)
        {
            Close(this);
        }
        else
        {
            Console.WriteLine("error creating new shift");
        }
    }
}