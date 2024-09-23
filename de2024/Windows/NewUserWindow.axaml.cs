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

public partial class NewUserWindow : Window
{
    public User user;
    public ObservableCollection<Userrole>? userroles { get; set; }
    
    private List<string> _genderList = ["Мужской","Женский","Другой"];
    private List<string> _roleList { get; set; }
    private readonly Global _global = new Global();
    private readonly MessageBox _messageBox = new();
    
    public NewUserWindow()
    {
        InitializeComponent();
        _ = GetRoles();
        Gender.ItemsSource = _genderList;
        
        /*if (userroles == null)
        {
            _messageBox.TextBox.Text = "userrole = null";
            Close(this);
        }*/
        
        /*foreach (var role in userroles)
        {
            Role.Items.Add(role.Namerole);
        }*/
    }

    private async Task GetRoles()
    {
        using HttpClient httpClient = new HttpClient();
        userroles = await httpClient.GetFromJsonAsync<ObservableCollection<Userrole>>(_global._url + "/roles");
        Roles.ItemsSource = userroles;
    }

    public async Task<HttpStatusCode> ApiPost(User user)
    {
        using HttpClient httpClient = new HttpClient();
        var content = JsonContent.Create(user);
        var request = new HttpRequestMessage(HttpMethod.Post, _global._url + "/users")
        {
            Content = content
        };
        var response = await httpClient.SendAsync(request);
        response.EnsureSuccessStatusCode();
        
        return response.StatusCode;
    }

    public async void AddNewUser(object sender, RoutedEventArgs e)
    {
        user = new()
        {
            Middlename = MiddleName.Text,
            Firstname = FirstName.Text,
            Lastname = LastName.Text,
            Userroleid = Roles.SelectedIndex + 1,
            Login = Login.Text,
            Password = Password.Text
        };

        if (await ApiPost(user) == HttpStatusCode.OK)
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