//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace HotelAssessment
{
    using System;
    using System.Collections.Generic;
    
    public partial class Room
    {
        public Room()
        {
            this.Bookings = new HashSet<Booking>();
        }
    
        public int roomID { get; set; }
        public int numSingleBeds { get; set; }
        public int numDoubleBeds { get; set; }
        public int maxGuests { get; set; }
        public int singePrice { get; set; }
        public int doublePrice { get; set; }
        public string extraFeatures { get; set; }
        public int roomNumber { get; set; }
        public int extraPrice { get; set; }
    
        public virtual ICollection<Booking> Bookings { get; set; }
    }
}
