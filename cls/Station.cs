namespace NetRadio.cls;

public sealed record Station(int Number, string Name, string Url)  // unveränderliches Domänenobjekt für einen Radiosender
{
    public bool IsValid => !string.IsNullOrWhiteSpace(Url);  // Station ist spielbar (URL nicht leer)
    public string LongName => Utilities.StationLong(Name);  // Bereinigter Langname für Labels und ComboBox
    public string ShortName => Utilities.StationShort(Name, button: true);  // Kurzname für RadioButton-Beschriftung (&&-escaped für Akzeleratoren)
    public string LabelName => Utilities.StationLong(Name, button: true);  // Langname mit &&-Escaping für lblD1
    public static Station Empty(int number) => new(number, string.Empty, string.Empty);  // Leerer Platzhalter-Sender für Slot <paramref name="number"/>
}
