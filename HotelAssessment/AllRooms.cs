using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace HotelAssessment {
    class AllRooms {
        public AllRooms()
        {
            
        }
        public void getRooms(DataGridView dgv) {
            using (var context = new HotelDBEntities())
            {
               dgv.DataSource = context.Rooms.ToList();
               dgv.Columns[dgv.ColumnCount-1].Visible = false;//remove last column, reference to the booking of the room. 
            }
        }

        public static Room getRoom(int roomID) {
            using (var context = new HotelDBEntities())
            {
                return context.Rooms.Find(roomID);

            }
        }

        public bool removeRoom(int roomId) {
            using (var context = new HotelDBEntities()) {
                Room room = context.Rooms.Find(roomId);//find entity by primary key
                if (room != null)//just incase!
                {
                    context.Rooms.Remove(room);
                    context.SaveChanges();
                    return true;//return true if succsessful removal of guest
                }
            }
            return false;
        }

        public void removeEverything() {
            using (var context = new HotelDBEntities())
            {
                context.Billings.RemoveRange(context.Billings);
                context.Bookings.RemoveRange(context.Bookings);
                context.Guests.RemoveRange(context.Guests);
                context.Rooms.RemoveRange(context.Rooms);
                context.SaveChanges();
            }
        }
       
        public void addRoom(int numSingleBeds, int numDoubleBeds, int maxGuests, int singlePrice, int doublePrice, int extraPrice, int roomNumber, String extraFeatures) {
            using (var context = new HotelDBEntities()) {
                Room newRoom = new Room { numSingleBeds = numSingleBeds, numDoubleBeds = numDoubleBeds, maxGuests = maxGuests, singePrice = singlePrice, doublePrice = doublePrice, extraPrice = extraPrice, roomNumber = roomNumber, extraFeatures = extraFeatures};
                context.Rooms.Add(newRoom);
                context.SaveChanges();
            }
        }

        public Room[] getRooms(Room[] avRooms, int numGuests, int numSingleBeds, int numDoubleBeds, int maxPrice)
        {
            var filteredRooms = avRooms.Where(r => r.maxGuests >= numGuests);
            if (numSingleBeds > 0)//filter singleBeds
                filteredRooms = filteredRooms.Where(r => r.numSingleBeds >= numSingleBeds);
            if (numDoubleBeds > 0)//filter doubleBeds
                filteredRooms = filteredRooms.Where(r => r.numDoubleBeds >= numDoubleBeds);
            if (maxPrice > 0)
                filteredRooms = filteredRooms.Where(r => calcRoomCost(r, numGuests) <= maxPrice);
            return filteredRooms.ToArray();//from room IEnumerable to room[] 
        }

        //
        public Room[] getAvailiableRooms(DateTime startDate, DateTime endDate)
        {   
            using (var context = new HotelDBEntities())
            {
                //modified to support LINQ to entity but still use LINQs, could be troublsome for massive databases (toArray will return everything)
                var filteredBookings = context.Bookings.ToArray().Where(b => (DateUtil.compare(startDate.ToShortDateString(), b.bookingEnd) && DateUtil.compare(b.bookingStart, endDate.ToShortDateString())));
                int[] bookedRooms = filteredBookings.Select(booking => booking.roomIDFK).ToArray(); //the resulting array of booked room primary keys
                List<Room> avRooms = context.Rooms.ToList();
                var roomsToRemove = from room in avRooms from t in bookedRooms where room.roomID == t select room;//single line nesting. Returns rooms that the meet the condition.
                roomsToRemove = roomsToRemove.ToArray();//more friendly than IEnumerable
                foreach (var room in roomsToRemove) //remove all booked rooms from list of all rooms
                    avRooms.Remove(room);
                    
                return avRooms.ToArray();
            }
        }
        /*calculate room cost
         * guest is charged for all beds currently in the room. 
         * if the number of guests exceeds the current beds, (2 guests to a doublebed)
         * the guest is charged the "extraPrice" of the room for every extra guest
         */
        public int calcRoomCost(Room room, int numGuests) {
            int price = 0;
            price = (room.singePrice * room.numSingleBeds) + (room.numDoubleBeds * room.doublePrice);
            int remainingGuests = (numGuests - room.numSingleBeds) - (room.numDoubleBeds * 2);
            if (remainingGuests > 0)
                price += remainingGuests * (int)room.extraPrice;
            return price;
        }
    }

}
