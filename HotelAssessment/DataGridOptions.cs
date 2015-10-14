using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace HotelAssessment {
    static class DataGridOptions {
        //convienient methods for getting data. re-useable
        public static String[] getRowInfo(DataGridViewRow selectedRow)
        {
            String[] rowData = new string[selectedRow.Cells.Count];
            
            for (int i = 0; i < rowData.Length; i++)
                rowData[i] = selectedRow.Cells[i].Value.ToString();
            return rowData;
        }
        //get row data by column name
        public static string getRowDataByName(DataGridViewRow dataRow, String columnName)
         {
             if (dataRow == null)
                 return "";
             if (dataRow.Cells.Count == 0)
                 return "";
            for(int i = 0; i < dataRow.Cells.Count; i++)
                if (dataRow.Cells[i].OwningColumn.Name == columnName)
                    return dataRow.Cells[i].Value.ToString();
             return String.Empty;
        }
    }
}
