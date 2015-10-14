using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelAssessment {
    class DateUtils {
        //return true if date1 < date2
        public DateUtils() {

        }
        public static bool compare(String date1, String date2) {
            int[] values1 = toInt(date1);
            int[] values2 = toInt(date2);

            if (values1[1] < values2[1] && values1[0] <= values2[0])
                return true;
            return false;
        }

        public static int[] toInt(String date) {
            String[] split = date.Split(' ');
            split = split[0].Split('/');

            return Array.ConvertAll(split, int.Parse);
        }
    }
}
