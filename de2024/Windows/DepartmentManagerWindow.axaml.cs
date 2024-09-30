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
using Avalonia.Markup.Xaml.Templates;
using de2024.Model;

namespace de2024.Windows;

public partial class DepartmentManagerWindow : Window, INotifyPropertyChanged
{
    public string usersPath = "/users";
    public string shiftsPath = "/shifts";
    public int selectedList = 0; // 0 = users, 2 = shifts
    public ObservableCollection<User>? users { get; set; }
    public ObservableCollection<Shift>? shifts { get; set; }
    public List<string> statusList { get; set; } = ["Работает", "Уволен"];
    public new event PropertyChangedEventHandler? PropertyChanged;

    private readonly Global _global = new Global();
    private readonly NewUserWindow _newUserWindow = new();
    
    /// <summary>
    /// TODO: Доделать список смен и изменение статуса у сотрудников.
    /// </summary>
  
    public DepartmentManagerWindow()
    {
        TestData();
        
        InitializeComponent();
        DataContext = this;
        GetUsers(usersPath);
        GetShifts(shiftsPath);
        
    }

    private void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    // Получение списка пользователей
    private async Task GetUsers(string path)
    {
        try
        {
            using HttpClient httpClient = new HttpClient();
            users = await httpClient.GetFromJsonAsync<ObservableCollection<User>>(_global._url + path);
            ListBox.ItemsSource = users;
        }
        catch(Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
    }
    
    // Получение списка смен
    private async Task GetShifts(string path)
    {
        try
        {
            using HttpClient httpClient = new HttpClient();
            shifts = await httpClient.GetFromJsonAsync<ObservableCollection<Shift>>(_global._url + path);
        }
        catch(Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
    }

    private async Task<HttpStatusCode> UpdateUser(string path, User user)
    {
        try
        {
            using HttpClient httpClient = new HttpClient();
            var content = JsonContent.Create(user);
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
        switch (selectedList)
        {
            case 0:
                _newUserWindow.Show(this);
                break;
            case 2:
                break;
        }
        
    }
    

    private void SetShifts_Onclick(object? sender, RoutedEventArgs e)
    {
        selectedList = 2;
        ListBox.ItemsSource = shifts;
        ListBox.ItemTemplate = (DataTemplate)Resources["ShiftsTemplate"];
        OnPropertyChanged("ListBox");
    }
    
    private void SetUsers_OnClick(object? sender, RoutedEventArgs e)
    {
        selectedList = 0;
        ListBox.ItemsSource = users;
        ListBox.ItemTemplate = (DataTemplate)Resources["UsersTemplate"];
        OnPropertyChanged("ListBox");
    }

    private void StatusChanged(object? sender, SelectionChangedEventArgs e)
    {
        User user = ListBox.SelectedItem as User;

        if (user != null)
            UpdateUser(usersPath, user);
    }

    private void UpdateData_OnClick(object? sender, RoutedEventArgs e)
    {
        GetUsers(usersPath);
        GetShifts(shiftsPath);
    }
}