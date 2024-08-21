using System;
using System.Collections;
using System.Reflection.Metadata;
using System.Windows.Forms;

namespace NetRadio
{
    internal class CListViewItemComparer : IComparer
    {
        private int colToSort = 0;  // Spalte, die sortiert werden soll
        private SortOrder orderOfSort = SortOrder.None; // Sortierfolge (aufsteigend, absteigend, ohne)
        private int lastOrderCol = -1;

        public int Compare(object x, object y)
        {
            ListViewItem lviX, lviY;
            lviX = (ListViewItem)x;
            lviY = (ListViewItem)y;
            //if (lviX.SubItems.Count == -1 || lviY.SubItems.Count == -1) { return 0; }
            try
            {

                int tempOrderCol;
                int compResult = colToSort == 0 ? string.Compare(lviX.Tag.ToString(), lviY.Tag.ToString()) : string.Compare(lviX.SubItems[colToSort].Text, lviY.SubItems[colToSort].Text);
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
                        if (compResult == 0) { compResult = string.Compare(lviX.Tag.ToString(), lviY.Tag.ToString()); }
                    }
                    else if (colToSort == 2)
                    {
                        compResult = string.Compare(lviX.Tag.ToString(), lviY.Tag.ToString());
                        if (compResult == 0) { compResult = string.Compare(lviX.SubItems[1].Text, lviY.SubItems[1].Text); }
                    }
                }
                lastOrderCol = lastOrderCol != colToSort ? colToSort : lastOrderCol;
                if (orderOfSort == SortOrder.Ascending) { return compResult; }
                else if (orderOfSort == SortOrder.Descending) { return (-compResult); }
                else { return 0; } // bei Gleichheit
            }
            catch (Exception ex)
            {
                Utilities.ErrorMsgTaskDlg(Application.OpenForms[0].Handle, ex.Message, Application.ProductName + " - Comparer");
                return 0;
            }
        }

        public int SortColumn { set { colToSort = value; } get { return colToSort; } }  // liest oder schreibt die Nummer der zu sortierenden Spalte 
        public SortOrder Order { set { orderOfSort = value; } get { return orderOfSort; } } // liest oder schreibt die Sortierrichtung

    }
}
