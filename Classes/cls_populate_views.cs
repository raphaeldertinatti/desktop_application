using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DesktopApplication
{
    public class cls_populate_views
    {
        public void PopulateListViews(ListView listview, MySqlCommand cmd, int[] columnIndexes)
        {
            using(MySqlDataReader reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    string[] row = new string[columnIndexes.Length];
                    for (int i = 0; i < columnIndexes.Length; i++)
                    {
                        row[i] = reader.GetString(columnIndexes[i]);
                    }
                    var listview_line = new ListViewItem(row);
                    listview.Items.Add(listview_line);
                }
            }
        }

        public void ConstructListView(ListView listview, string[] headers, int[] widths)
        {
            listview.View = View.Details;            
            listview.AllowColumnReorder = true;
            listview.FullRowSelect = true;
            listview.GridLines = true;

            for (int i = 0; i < headers.Length; i++)
            {
                listview.Columns.Add(headers[i], widths[i], HorizontalAlignment.Left);
            }
        }
    }
}
