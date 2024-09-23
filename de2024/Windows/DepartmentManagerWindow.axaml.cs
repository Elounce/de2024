using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using de2024.Model;
using de2024.Windows;

namespace de2024;

public partial class DepartmentManagerWindow : Window, INotifyPropertyChanged
{
    public string usersPath = "/users";
    public string shiftsPath = "/shifts"; 
    public ObservableCollection<User>? users { get; set; }
    public List<string> statusList { get; set; } = ["Работает", "Уволен"];
    public new event PropertyChangedEventHandler? PropertyChanged;

    private readonly Global _global = new Global();
    private readonly NewUserWindow _newUserWindow = new();
    
    public DepartmentManagerWindow()
    {
        /*TestData();*/
        
        InitializeComponent();
        DataContext = this;
        ApiCallback(usersPath);

    }

    private void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    // Запрос юзеров
    private async Task ApiCallback(string path)
    {
        try
        {
            using HttpClient httpClient = new HttpClient();
            users = await httpClient.GetFromJsonAsync<ObservableCollection<User>>(_global._url + path);
            /*ListBox.ItemsSource = users;
            OnPropertyChanged(nameof(ItemsControl));*/
        }
        catch(Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
        
    }

    public void TestData()
    {
        users =
        [
            new User { Firstname = "Иван", Middlename = "Иванович", Lastname = "Иванов"},
            new User { Firstname = "Артём", Middlename = "Артёмович", Lastname = "Артёмов" }
        ];
    }

    public void OpenNewUserWindow(object? sender, RoutedEventArgs e)
    {
        if(!_newUserWindow.IsLoaded)
            _newUserWindow.Show(this);
    }

    private void SetUsers_OnClick(object? sender, RoutedEventArgs e)
    {
        ListBox.ItemsSource = users;
        OnPropertyChanged(nameof(ListBox));
    }

    private void ChangeStatus_OnClick(object? sender, RoutedEventArgs e)
    {
        
    }
}