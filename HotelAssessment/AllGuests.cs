using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace HotelAssessment {
    class AllGuests
    {
        public AllGuests()
        {
        }

        public Guest[] getGuests(DataGridView dgv)
        {
            using (var context = new HotelDBEntities())
            {
                var guests = context.Guests;
                dgv.DataSource = guests.ToList();
                dgv.Columns[dgv.ColumnCount - 1].Visible = dgv.Columns[dgv.ColumnCount - 2].Visible = false; //removes links to booking and billing tables
                return guests.ToArray();
            }
        }

        public Guest[] findByName(String name)
        {
            using (var context = new HotelDBEntities())
            {
                return context.Guests.Where(g => g.name == name).ToArray();
            }
        }


        public static Guest getGuest(int guestID)
        {
            using (var context = new HotelDBEntities()) { return context.Guests.Find(guestID); }
        }

        //remove a guest and other tables asscociated with it
        public static bool removeGuest(int guestId)
        {
            using (var context = new HotelDBEntities())
            {
                foreach (var bill in context.Billings)
                {
                    if (bill.guestIDFK == guestId)
                    {
                        context.Billings.Remove(bill);
                        break;
                    }
                }
                Guest g = context.Guests.Find(guestId);
                if (g != null)
                {
                    context.Bookings.Remove(g.Booking);
                    context.Guests.Remove(g);
                    context.SaveChanges();
                    return true;
                }


            }
            return false;
        }

        public bool removeAllGuests()
        {
            using (var context = new HotelDBEntities())
            {   
                context.Billings.RemoveRange(context.Billings.ToArray());
                context.Bookings.RemoveRange(context.Bookings.ToArray());
                context.Guests.RemoveRange(context.Guests.ToArray());
                context.SaveChanges();
                if (context.Guests.Count(g => true) == 0)
                    return true;
            }
            return false;
        }

        public Guest addGuest(string name, int age, int bookingPK,string address, string comment)
        {
            using (var context = new HotelDBEntities())
            {
                Guest newGuest = new Guest {name = name, age = age, bookingIDFK = bookingPK, address = address, comment = comment};
                context.Guests.Add(newGuest);
                context.SaveChanges();
                return newGuest;
            }
        }
         public static bool checkIn(int guestID)
         {   
            using (var context = new HotelDBEntities())
            {
                DateTime date = DateTime.Today;
                Guest guest = context.Guests.Find(guestID);
                if (DateUtil.compare(guest.Booking.bookingStart, date.ToShortDateString()))
                {
                    context.Guests.Find(guestID).checkInDate = date.ToShortDateString();
                    context.SaveChanges();
                    MessageBox.Show("guest checked in");
                    return true;
                }
                    MessageBox.Show("Please come back anytime after: " + guest.Booking.bookingStart);
                    return false;
            }
        }

         public static bool checkOut(int guestID) {
             using (var context = new HotelDBEntities()) {
                 DateTime date = DateTime.Today;
                 Guest guest = context.Guests.Find(guestID);
                 context.Guests.Find(guestID).checkOutDate = date.ToShortDateString();
                 context.Guests.Find(guestID).Booking.bookingEnd = date.ToShortDateString();
                 context.SaveChanges();
                    return true;
             }
         }
    }
}
