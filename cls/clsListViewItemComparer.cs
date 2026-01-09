using System;
using System.Collections;
using System.Windows.Forms;

namespace NetRadio.cls;

internal class CListViewItemComparer : IComparer
{
    private int colToSort = 0;
    private SortOrder orderOfSort = SortOrder.None;
    private int lastOrderCol = -1;

    public int Compare(object? x, object? y)
    {
        if (x == null && y == null) { return 0; }
        if (x == null) { return -1; } // null-Werte werden als "kleiner" betrachtet
        if (y == null) { return 1; }
        var lviX = x as ListViewItem; // 'as' wirft keine Ausnahme, gibt null zurück, wenn Casting fehlschlägt
        var lviY = y as ListViewItem;
        if (lviX == null && lviY == null) { return 0; } // Prüfen, ob die Umwandlung erfolgreich war
        if (lviX == null) { return -1; }
        if (lviY == null) { return 1; }
        try
        {
            int tempOrderCol;
            var compResult = colToSort == 0 ? string.Compare(lviX.Tag?.ToString(), lviY.Tag?.ToString()) : string.Compare(lviX.SubItems[colToSort].Text, lviY.SubItems[colToSort].Text);
            if (compResult == 0) // If equal, sort based on next column
            {
                if (colToSort == 0)
                {
                    tempOrderCol = lastOrderCol == colToSort ? 2 : lastOrderCol;
                    compResult = string.Compare(lviX.SubItems[tempOrderCol].Text, lviY.SubItems[tempOrderCol].Text);
                    if (compResult == 0)
                    {
                        tempOrderCol = tempOrderCol == 2 ? 1 : 2;
                        compResult = string.Compare(lviX.SubItems[tempOrderCol].Text, lviY.SubItems[tempOrderCol].Text);
                    }
                }
                else if (colToSort == 1)
                {
                    compResult = string.Compare(lviX.SubItems[2].Text, lviY.SubItems[2].Text);
                    if (compResult == 0) { compResult = string.Compare(lviX.Tag?.ToString(), lviY.Tag?.ToString()); }
                }
                else if (colToSort == 2)
                {
                    compResult = string.Compare(lviX.Tag?.ToString(), lviY.Tag?.ToString());
                    if (compResult == 0) { compResult = string.Compare(lviX.SubItems[1].Text, lviY.SubItems[1].Text); }
                }
            }
            lastOrderCol = lastOrderCol != colToSort ? colToSort : lastOrderCol;
            if (orderOfSort == SortOrder.Ascending) { return compResult; }
            else if (orderOfSort == SortOrder.Descending) { return -compResult; }
            else { return 0; } // bei Gleichheit (sollte durch compResult schon abgedeckt sein, aber als Fallback)
        }
        catch (Exception ex)
        {
            if (Application.OpenForms.Count > 0) { Utilities.ErrTaskDialog(Application.OpenForms[0]!, ex); }
            return 0;
        }
    }

    public int SortColumn
    {
        set => colToSort = value;
        get => colToSort;
    }

    public SortOrder Order
    {
        set => orderOfSort = value;
        get => orderOfSort;
    }
}