using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace HotelAssessment {
    public static class DateUtil {
        //return true if date1 <= date2
        public static bool compare(String date1, String date2)
        {
            int[] values1 = toInt(date1);
            int[] values2 = toInt(date2);
            if (values1[0] <= values2[0] && values1[1] <= values2[1])
                return true;
            return false;
        }
        //convert date from string to int array
        public static int[] toInt(String date)
        {
            String[] split = date.Split(' ');//incase date parsed contained time of day etc. convienient
            split = split[0].Split('/');

            return Array.ConvertAll(split, int.Parse);
        }

        
    }
}
