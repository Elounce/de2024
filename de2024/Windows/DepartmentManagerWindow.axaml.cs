using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Templates;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Avalonia.Markup.Xaml.MarkupExtensions;
using Avalonia.Markup.Xaml.Templates;
using de2024.Model;
using de2024.Windows;

namespace de2024;

public partial class DepartmentManagerWindow : Window, INotifyPropertyChanged
{
    public string usersPath = "/users";
    public string shiftsPath = "/shifts"; 
    public ObservableCollection<User>? users { get; set; }
    public ObservableCollection<Shift>? shifts { get; set; }
    public List<string> statusList { get; set; } /*= ["Работает", "Уволен"];*/
    public string defaultStatus = "Работает";
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
    
    private void CheckBox_OnPropertyChanged(object? sender, AvaloniaPropertyChangedEventArgs e)
    {
        
    }

    private void SetShifts_Onclick(object? sender, RoutedEventArgs e)
    {
        AddButton.IsVisible = false;
        OnPropertyChanged("AddButton");
        ListBox.ItemsSource = shifts;
        ListBox.ItemTemplate = (DataTemplate)Resources["ShiftsTemplate"];
        OnPropertyChanged("ListBox");
    }
    
    private void SetUsers_OnClick(object? sender, RoutedEventArgs e)
    {
        AddButton.IsVisible = true;
        OnPropertyChanged("AddButton");
        ListBox.ItemsSource = users;
        ListBox.ItemTemplate = (DataTemplate)Resources["UsersTemplate"];
        OnPropertyChanged("ListBox");
    }
}