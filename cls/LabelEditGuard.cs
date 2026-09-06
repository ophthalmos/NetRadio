using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;

namespace NetRadio.cls;

/// <summary>Workaround für dotnet/winforms#14657 (.NET 10; der Fix aus PR 14663 kommt erst mit .NET 12):
/// Beim Bearbeiten einer Beschriftung in TreeView/ListView registriert WinForms einen WinEvent-Hook, dessen
/// Delegat im internen Objekt <c>_labelEdit</c> (TreeView-/ListViewLabelEditNativeWindow) lebt. Endet die
/// Bearbeitung, bleibt der Hook registriert; sobald der Garbage Collector das Objekt eingesammelt hat, beendet
/// der nächste Hook-Aufruf – ausgelöst durch jedes SetWindowText im Prozess, z.B. TextBox.Text im nächsten
/// Dialog – das Programm per FailFast („callback was made on a garbage collected delegate“).
/// <see cref="Pin"/> hält das Objekt dauerhaft fest; der verwaiste Hook läuft dann ins Leere statt in
/// freigegebenen Speicher. Ändern sich die WinForms-Interna (Feld nicht gefunden), passiert schlicht nichts.
/// Bei einem Umstieg auf .NET 12 kann die Klasse entfallen.</summary>
internal static class LabelEditGuard
{
    private static readonly List<object> pinned = [];
    private static readonly Dictionary<Type, FieldInfo?> fieldCache = [];

    /// <summary>Beim Start einer Beschriftungsbearbeitung aufrufen (BeforeLabelEdit). Das interne
    /// Bearbeitungsfenster entsteht erst nach dem Ereignis, daher greift Pin per BeginInvoke darauf zu.</summary>
    public static void Pin(Control control)
    {
        var field = FindLabelEditField(control.GetType());
        if (field == null || !control.IsHandleCreated) { return; }
        control.BeginInvoke(new Action(() =>
        {
            var window = field.GetValue(control);
            if (window != null && !pinned.Contains(window)) { pinned.Add(window); }
        }));
    }

    /// <summary>Das private Feld, das das LabelEditNativeWindow hält (TreeView._labelEdit, ListView._labelEdit) –
    /// gesucht über den Typ (NativeWindow-Abkömmling mit „LabelEdit“ im Namen), nicht über den Feldnamen.</summary>
    private static FieldInfo? FindLabelEditField(Type controlType)
    {
        lock (fieldCache)
        {
            if (fieldCache.TryGetValue(controlType, out var cached)) { return cached; }
            FieldInfo? found = null;
            for (var type = controlType; type != null && found == null; type = type.BaseType)
            {
                found = type.GetFields(BindingFlags.NonPublic | BindingFlags.Instance)
                    .FirstOrDefault(f => typeof(NativeWindow).IsAssignableFrom(f.FieldType) && f.FieldType.Name.Contains("LabelEdit", StringComparison.Ordinal));
            }
            fieldCache[controlType] = found;
            return found;
        }
    }
}
