using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Entity.Core.Common.CommandTrees.ExpressionBuilder;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace HotelAssessment {
    public partial class Form1 : Form
    {
        private Form extraForm = new Form();
        private AllRooms allRooms = new AllRooms();
        private AllGuests allGuests = new AllGuests();
        private HotelBilling hotelBilling = new HotelBilling();
        private GuestBooking guestBooking = new GuestBooking();
        public Form1() {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e) {
            extraForm.FormClosing += new FormClosingEventHandler(onFormClose);
            
            dgvAvRooms.Dock = DockStyle.Fill;//set the control to fill its parent control
            dgvGuests.Dock = DockStyle.Fill;
            dtEndDate.MinDate = dtStartDate.MinDate = DateTime.Today;//set minimum selectable dates to todays date.
            refreshDataGrids();
           /* allRooms.addRoom(1,1,5,170,200, 90, 1,"CS");
            allRooms.addRoom(1,0,3,120,0,90,2,"VS");
            allRooms.addRoom(2,1,7,145,200,85,3,"WS");
            allRooms.addRoom(0,1,3,0,190,90,4,"S");
            allRooms.addRoom(4,0,5,120,0,45,5,"VS");
            allRooms.addRoom(2,2,7,100,195,30,6,"N");//'N' features = none!*/
            //allRooms.removeEverything();// deletes all rooms@!
        }

        //event for putting dataGrid into its own form
        private void cbDataToWindow_CheckedChanged(object sender, EventArgs e)
        {
            if (cbDataToWindow.Checked)
            {
                extraForm.Text = "Available Rooms";
                dgvAvRooms.Dock = DockStyle.Fill;
                dataToWindow(extraForm, dgvAvRooms);
                extraForm.Visible = true;
            } else {
                dataToWindow(pnlAvRooms, dgvAvRooms);
                extraForm.Visible = false;
            }
        }
        //add datagrid to specified control (main form or new resizable form)
        private void dataToWindow(Control toControl, DataGridView source)
        {
            toControl.Controls.Add(source);
            toControl.Show();
        }
        //close event for the extra form that displays dataGrid
        private void onFormClose(object sender, FormClosingEventArgs eventArgs)
        {
            cbDataToWindow.Checked = false;
            dgvAvRooms.Dock = DockStyle.None;
            pnlAvRooms.Controls.Add(dgvAvRooms);
            pnlGuests.Controls.Add(dgvGuests);
            extraForm.Visible = false;
            cbDataToWindow.Checked = false;
            cbMaximizeGuests.Checked = false;
            eventArgs.Cancel = true;//cancel the closing event to prevent the form being disposed
        }

        private void btnAllRooms_Click(object sender, EventArgs e)
        {
            nudPriceFind.Value = nudGuestsFind.Value = nudDoubleBedsFind.Value = nudSingleBedsFind.Value = 0;//reset UI values
            dgvAvRooms.DataSource = dgvAvRooms.DataSource = allRooms.getAvailiableRooms(System.DateTime.Today, System.DateTime.Today); //parameters of todays date returns all rooms availiable today
        }


        private void btnFindRooms_Click(object sender, EventArgs e)
        {
            Room[] avRooms = allRooms.getAvailiableRooms(dtStartDate.Value, dtEndDate.Value);//returned list of available rooms for given date
            dgvAvRooms.DataSource = allRooms.getRooms(avRooms, (int)nudGuestsFind.Value, (int)nudSingleBedsFind.Value, (int)nudDoubleBedsFind.Value, (int)nudPriceFind.Value);//filter avliable rooms by search criteria
        }

        //refreshes the datagrid data and remove unwanted columns
        private void refreshDataGrids()
        {
            dgvAvRooms.DataSource = allRooms.getAvailiableRooms(System.DateTime.Today, System.DateTime.Today);
            dgvAvRooms.Columns[dgvAvRooms.ColumnCount - 1].Visible = false;
            allGuests.getGuests(dgvGuests);
            allRooms.getRooms(dgvConfigRooms);
            allGuests.getGuests(dgvConfigGuests);
        }

        private void btnBookRoom_Click(object sender, EventArgs e) {
            if (tbGuestName.Text.Equals(string.Empty) || tbGuestName.Text.Equals(string.Empty) || tbGuestPhone.Text.Equals(string.Empty))
            {
                MessageBox.Show(@"All guest information (excluding 'Comment') must be entered");
                return;
            }
            int roomPK = int.Parse(DataGridOptions.getRowDataByName(dgvAvRooms.CurrentRow, "roomID"));//get roomID from selected row of DGV
            int bookingPK = guestBooking.addBooking(roomPK, dtStartDate.Value.ToShortDateString(), dtEndDate.Value.ToShortDateString(), (int)nudBookNumGuest.Value).bookingID;//add booking
            int guestID = allGuests.addGuest(tbGuestName.Text, (int)nudGuestAge.Value, bookingPK, tbGuestAddress.Text, tbComment.Text).guestID;//add guest (also returns added guests primary key
            hotelBilling.createBilling(guestID, (int)nudRoomCost.Value); //create billing
            refreshDataGrids();//update UI with new Data
        }

        //checkbox event to add dgv to new form
        private void cbMaximizeGuests_CheckedChanged(object sender, EventArgs e) {
            if (cbMaximizeGuests.Checked) {
                dataToWindow(extraForm, dgvGuests);
                extraForm.Text = "All Guests";
                extraForm.Visible = true;
            } else {
                dataToWindow(pnlGuests, dgvGuests);
                extraForm.Visible = false;
            }
        }

        //remove a guest
        private void btnRemove_Click(object sender, EventArgs e)
        {
            if (dgvGuests.SelectedCells.Count > 0)
            {
                int selectedGuestID = int.Parse(DataGridOptions.getRowDataByName(dgvGuests.SelectedCells[0].OwningRow, "guestID"));
                AllGuests.removeGuest(selectedGuestID);
                tbCheckName.Text = string.Empty;
                tbCheckAddress.Text = string.Empty;
                tbCheckRoomNum.Text = string.Empty;
                tbCheckNumGuests.Text = string.Empty;
                nudGuestAge.Value = 0;
                refreshDataGrids();
            }
        }
        //set fields from selected DGV row
        private void dgvGuests_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            DataGridViewRow row = dgvGuests.CurrentCell.OwningRow;
            int guestID = int.Parse(DataGridOptions.getRowDataByName(row, "guestID"));
            Guest guest = AllGuests.getGuest(guestID);
            tbCheckName.Text = DataGridOptions.getRowDataByName(row, "name");
            tbCheckAddress.Text = DataGridOptions.getRowDataByName(row, "address");
            tbCheckNumGuests.Text = GuestBooking.getBooking(guest.bookingIDFK).numGuests.ToString();
            tbCheckRoomNum.Text = AllRooms.getRoom(GuestBooking.getBooking(guest.bookingIDFK).roomIDFK).roomNumber.ToString();
            nudWifi.Value = nudBar.Value = nudPhone.Value = 0;
            lblCost.Text = HotelBilling.getTotalBill(guestID).ToString();

        }

        private void btnCheckIn_Click(object sender, EventArgs e)
        {
            if (dgvGuests.CurrentCell == null)
                return;//return if no row/cell is currently selected
            DataGridViewRow row = dgvGuests.CurrentCell.OwningRow;//currently selected row
            int guestID = int.Parse(DataGridOptions.getRowDataByName(row, "guestID"));//get guest id from selected DGV row
            AllGuests.checkIn(guestID);//check in guest
            refreshDataGrids();
        }

        //check out selected guest
        private void btnCheckOut_Click(object sender, EventArgs e)
        {
            if (dgvGuests.CurrentCell == null)
                return;
            DataGridViewRow row = dgvGuests.CurrentCell.OwningRow;
            int guestID = int.Parse(DataGridOptions.getRowDataByName(row, "guestID"));
            AllGuests.checkOut(guestID);//mark as checked out in DB
            HotelBilling.payBill(guestID);//Billing/Payment
            lblCost.Text = String.Empty;
            refreshDataGrids();//refresh with new data
        }

        //apply all costs to guest (clicked before payment)
        private void btnCosts_Click(object sender, EventArgs e)
        {
            if (dgvGuests.CurrentCell == null)
                return;
            DataGridViewRow row = dgvGuests.CurrentCell.OwningRow;
            int guestID = int.Parse(DataGridOptions.getRowDataByName(row, "guestID"));
            int extraCosts = (int) (nudBar.Value + nudWifi.Value + nudPhone.Value);
            HotelBilling.addExtras(guestID, extraCosts);
            lblCost.Text = HotelBilling.getTotalBill(guestID).ToString();
        }

        //cell click of avaliable rooms DGV
        private void dgvAvRooms_CellClick(object sender, DataGridViewCellEventArgs e) {
            DataGridViewRow row = ((DataGridView)sender).CurrentCell.OwningRow;
            tbRoomNumber.Text = DataGridOptions.getRowDataByName(row, "roomNumber");
            Room selectedRoom = AllRooms.getRoom(int.Parse(DataGridOptions.getRowDataByName(row, "roomID")));
            nudDiscount.Value =   //ignore whitespace
            nudRoomCost.Value = 0;
            nudRoomCost.Tag = nudRoomCost.Value = allRooms.calcRoomCost(selectedRoom, (int)nudBookNumGuest.Value);
        }

        

        private void nudBookNumGuest_ValueChanged(object sender, EventArgs e)
        {
            if (dgvAvRooms.CurrentCell == null)
                return;
            DataGridViewRow row = dgvAvRooms.CurrentCell.OwningRow;
            Room selectedRoom = AllRooms.getRoom(int.Parse(DataGridOptions.getRowDataByName(row, "roomID")));
            nudRoomCost.Tag = nudRoomCost.Value = allRooms.calcRoomCost(selectedRoom, (int)nudBookNumGuest.Value);
        }

        private void numericUpDown2_ValueChanged(object sender, EventArgs e)
        {
            if (nudRoomCost.Tag == null)
                return;
            int roomCost = int.Parse(nudRoomCost.Tag.ToString());
            nudRoomCost.Value = roomCost*(1 - (nudDiscount.Value/100));
        }

        private void nudDiscount_KeyUp(object sender, KeyEventArgs e) {
            if (nudRoomCost.Tag == null)
                return;
            int roomCost = int.Parse(nudRoomCost.Tag.ToString());
            nudRoomCost.Value = roomCost * (1 - (nudDiscount.Value / 100));
        }

        private void tbSearchName_KeyUp(object sender, KeyEventArgs e) {
            if (e.KeyCode == Keys.Enter)//on enter event filter the DataGridView
            {
                if (tbSearchName.Text == String.Empty)
                {
                    refreshDataGrids();
                    return;
                }
                dgvGuests.DataSource = allGuests.findByName(tbSearchName.Text);
            }
        }

        private void nudSearchID_ValueChanged(object sender, EventArgs e)
        {
            if (nudSearchID.Value < 0) //out of scope value 
            {
                refreshDataGrids();
                return;
            }
            dgvGuests.DataSource = AllGuests.getGuest((int) nudSearchID.Value);
        }

        private void nudSearchID_KeyUp(object sender, KeyEventArgs e) {
            if (nudSearchID.Value < 0)
            {
                refreshDataGrids();
                return;
            }
            Guest[] guest = { AllGuests.getGuest((int)nudSearchID.Value) };//was being un friendly adding guest instance. array works fine
            dgvGuests.DataSource = guest;
            dgvGuests.Columns[dgvGuests.ColumnCount - 1].Visible = false;//remove unwanted DGV columns
            dgvGuests.Columns[dgvGuests.ColumnCount - 2].Visible = false;
        }

    }
}
