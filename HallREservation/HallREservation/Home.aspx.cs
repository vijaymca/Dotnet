using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;


namespace HallREservation
{
    public partial class Home : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btnClr_Click(object sender, EventArgs e)
        {
            txtOrganization.Text = string.Empty;
            txtEmail.Text = string.Empty;
            txtCr.Text = string.Empty;
            txtCalendar.Text = string.Empty;
            txtPhone.Text = string.Empty;
            txtResevation.Text = string.Empty;

            drpRooms.SelectedIndex = 0;
            chknonProfitOrg.Checked = false;


            lblStatus.Text = "Cancelled transaction..";
        }

        protected void btnCal_Click(object sender, EventArgs e)
        {

            int room = Convert.ToInt32(drpRooms.SelectedItem.Value);
            int days = Convert.ToInt32(txtResevation.Text);
            double total=0;

            switch (room)
            {
                case 1:
                    total = days * 4600;
                    break;
                case 2:
                    total = days * 4200;
                    break;
                case 3:
                    total = days * 800;
                    break;
                case 4:
                    total = days * 1000;
                    break;
                case 5:
                    total = days * 450;
                    break;
            }

            bool IsNonProfit = chknonProfitOrg.Checked;

            if (IsNonProfit) {
                total =total-(total * 0.15);
            }

            lblStatus.Text = "<b>Reservation Summery</b></br>Organization Name: "+txtOrganization.Text + "</br>Room: " + drpRooms.SelectedItem.Text + "</br>Days: "+days.ToString()+"</br>Charge: "+total.ToString();
        }
    }
}