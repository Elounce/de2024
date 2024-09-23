using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace de2024.Model;

public class User : INotifyPropertyChanged
{
    public int Userid { get; set; }

    public string Login { get; set; } = null!;

    public string Password { get; set; } = null!;

    public string? Status { get; set; }

    public string Lastname { get; set; } = null!;

    public string Firstname { get; set; } = null!;

    public string? Middlename { get; set; }

    public int? Userroleid { get; set; }
    public event PropertyChangedEventHandler? PropertyChanged;

    protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    protected bool SetField<T>(ref T field, T value, [CallerMemberName] string? propertyName = null)
    {
        if (EqualityComparer<T>.Default.Equals(field, value)) return false;
        field = value;
        OnPropertyChanged(propertyName);
        return true;
    }
}