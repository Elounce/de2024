using System;
using System.Collections.ObjectModel;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Media;
using de2024.Model;

namespace de2024.Windows;

public partial class AuthorizationWindow : Window
{
    public readonly string url = "http://localhost:5071/users";
    
    private ObservableCollection<User>? _users { get; set; }
    private readonly DepartmentManagerWindow _departmentManagerWindow = new();
    private readonly TechnicianWindow _technicianWindow =  new();
    private readonly OrganizerWIndow _organizerWindow =  new();

    public AuthorizationWindow()
    {
        InitializeComponent();
        /*_ = ApiCallback();*/
        TestData();
    }

    // Запрос в api
    private async Task ApiCallback()
    {
        try
        {
            using HttpClient httpClient = new HttpClient();
            _users = await httpClient.GetFromJsonAsync<ObservableCollection<User>>(url);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }
    }
    
    private void OpenNewWindow(object? sender, RoutedEventArgs e )
    {
        if (_users == null)
        {
            ErrorWindow();
        }
        
        // Ищет пользователей и открывает соответсвующее для него окно.
        foreach (var u in _users)
        {
            if (PassBox.Text == u.Password && LoginBox.Text == u.Login)
            {
                switch (u.Userroleid)
                {
                    case 1:
                        _departmentManagerWindow.Show();
                        break;
                    case 2:
                        _organizerWindow.Show();
                        break;
                    case 3:
                        _technicianWindow.Show();
                        break;
                }
            }
            else
            {
                ErrorWindow();
            }
        }
    }
    
    public void TestData()
    {
        _users = [new User { Login = "bellum", Password = "ZjXm3JW7RJ", Userroleid = 1}];
    }

    // Окно ошибки
    public void ErrorWindow()
    {
        Window errorWindow = new Window()
        {
            Background = Brushes.Red,
            Height = 300,
            Width = 300,
            Title = "Error",
        };
        errorWindow.Show(this);
    }

}