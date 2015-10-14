using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelAssessment {
    class GuestBooking {

        public static Booking getBooking(int bookingID)
        {
            using (var context = new HotelDBEntities())
            {
                return context.Bookings.Find(bookingID);
            }
        }
        public Booking addBooking(int roomPK, string startDate, string endDate, int numGuests)
        {
            using (var context = new HotelDBEntities())
            {
                Booking booking = new Booking {bookingStart = startDate, bookingEnd = endDate, roomIDFK = roomPK, numGuests = numGuests};
                context.Bookings.Add(booking);
                context.SaveChanges();
                return booking;
            }
        }

      
    }
}
