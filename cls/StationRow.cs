using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace NetRadio.cls;

/// <summary>
/// Veränderliche Datenzeile für das DGV-DataBinding.
/// Implementiert <see cref="INotifyPropertyChanged"/>, damit das DGV
/// Änderungen sofort reflektiert und <see cref="BindingList{T}.ListChanged"/>
/// automatisch ausgelöst wird.
/// </summary>
public sealed class StationRow : INotifyPropertyChanged
{
    private string _name = string.Empty;
    private string _url = string.Empty;

    /// <summary>Sendername (Rohtext, kann Sonder-Syntax enthalten).</summary>
    public string Name
    {
        get => _name;
        set
        {
            value ??= string.Empty;
            if (_name != value) { _name = value; OnPropertyChanged(); }
        }
    }

    /// <summary>Streaming-URL.</summary>
    public string Url
    {
        get => _url;
        set
        {
            value ??= string.Empty;
            if (_url != value) { _url = value; OnPropertyChanged(); }
        }
    }

    public event PropertyChangedEventHandler? PropertyChanged;

    private void OnPropertyChanged([CallerMemberName] string? name = null)
        => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));

    /// <summary>
    /// Erstellt den unveränderlichen <see cref="Station"/>-Record für die Wiedergabelogik.
    /// </summary>
    public Station ToStation(int number) => new(number, Name, Url);

    /// <summary>Gibt true zurück wenn Name und URL leer sind.</summary>
    public bool IsEmpty => string.IsNullOrWhiteSpace(Name) && string.IsNullOrWhiteSpace(Url);

    /// <summary>Kopiert Name und URL von einer anderen Zeile.</summary>
    public void CopyFrom(StationRow other)
    {
        Name = other.Name; Url = other.Url;
    }

    /// <summary>Setzt Name und URL zurück.</summary>
    public void Clear()
    {
        Name = string.Empty; Url = string.Empty;
    }
}
