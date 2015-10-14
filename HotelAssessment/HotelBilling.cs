using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace HotelAssessment {
    class HotelBilling {
        public HotelBilling()
        {
            
        }

        public static int getTotalBill(int guestPK)
        {
            using (var context = new HotelDBEntities())
            {
                foreach (var billing in context.Billings)
                    if (billing.guestIDFK == guestPK)
                    {
                        Billing bill = context.Billings.Find(billing.billingID);
                        return bill.roomCharge + bill.billingExtras;
                    }
            }
            MessageBox.Show("error calculating bill");
            return 0;
        }

        public static void addExtras(int guestPK, int extraCharges)
        {
            using (var context = new HotelDBEntities())
            {
                int billID = -1;//out of range default initilization
                foreach (var billing in context.Billings)
                    if (billing.guestIDFK == guestPK)
                    {
                        billID = billing.billingID;
                        break;
                    }
                context.Billings.Find(billID).billingExtras = extraCharges;
                context.SaveChanges();
            }
        }

        public static void payBill(int guestPK)
        {
            using (var context = new HotelDBEntities()) {
                int billID = -1;
                foreach (var billing in context.Billings)
                    if (billing.guestIDFK == guestPK) {
                        billID = billing.billingID;
                        break;
                    }
                context.Billings.Find(billID).paid = true;
                context.SaveChanges();
            }
        }

        public void createBilling(int guestPK, int roomCharge)
        {
            using (var context = new HotelDBEntities())
            {
                Billing guestBilling = new Billing{guestIDFK = guestPK, roomCharge = roomCharge};
                context.Billings.Add(guestBilling);
                context.SaveChanges();
            }
        }
    }
}
