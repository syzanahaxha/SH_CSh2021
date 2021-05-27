using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Entity;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PizzasSystem
{
    public partial class frmOrders : Form
    {
        #region Object
        OrderTb order = new OrderTb();
        #endregion
        #region Constructor
        public frmOrders()
        {
            InitializeComponent();
        }
        #endregion

        #region Events
        private void btnCancel_Click(object sender, EventArgs e)
        {
            Clear();
        }
        private void frmOrders_Load(object sender, EventArgs e)
        {
            Clear();
            FillGrid();
        }
        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Are you sure to delete this record?", "Information Delete Alert", MessageBoxButtons.YesNo) == DialogResult.Yes)
                using (PDBEntities db = new PDBEntities())
                {
                    var entry = db.Entry(order);
                    if (entry.State == EntityState.Detached)
                        db.OrderTbs.Attach(order);
                    db.OrderTbs.Remove(order);
                    db.SaveChanges();
                    FillGrid();
                    Clear();
                    MessageBox.Show("Deleted Successfully");
                }
        }
        private void btnSave_Click(object sender, EventArgs e)
        {
            order.Payment = cmbPayment.Text.Trim();
            order.ClientId = int.Parse(this.txtClientName.Text);
            order.PizzaId = int.Parse(this.txtPizzaId.Text);
            order.Qty = int.Parse(this.txtQuantity.Text);
            order.Price = int.Parse(this.txtPrice.Text);
            order.AdminId = int.Parse(this.txtAdmin.Text);
            order.Date = Convert.ToDateTime(this.txtDate.Text);

            using (PDBEntities db = new PDBEntities())
            {
                if (order.OrderId == 0) //insert
                    db.OrderTbs.Add(order);
                else //update
                    db.Entry(order).State = EntityState.Modified;
                    db.SaveChanges();
            }
            Clear();
            FillGrid();
            MessageBox.Show("Order added successfully!");
        }
        private void dgvOrder_DoubleClick(object sender, EventArgs e)
        {
            order.OrderId = Convert.ToInt32(dgvOrder.CurrentRow.Cells["OrderId"].Value);
            using (PDBEntities db = new PDBEntities())
            {
                order = db.OrderTbs.Where(x => x.OrderId == order.OrderId).FirstOrDefault();
                txtClientName.Text = order.ClientId.ToString();
                txtPizzaId.Text = order.PizzaId.ToString();
                txtAdmin.Text = order.AdminId.ToString();
                txtPrice.Text = order.Price.ToString();
                txtQuantity.Text = order.Qty.ToString();
                txtDate.Text = order.Date.ToString();
                cmbPayment.Text = order.Payment.ToString();

            }
            btnSave.Text = "Update";
            btnDelete.Enabled = true;
        }
        #endregion

        #region Functions
        public void Clear()
        {
            txtClientName.Text = txtPizzaId.Text = txtQuantity.Text = txtPrice.Text = txtDate.Text = txtAdmin.Text = cmbPayment.Text = "";
            btnSave.Text = "save";
            btnDelete.Enabled = false;
            order.OrderId = 0;
        }

        void FillGrid()
        {
            dgvOrder.AutoGenerateColumns = false;
            using (PDBEntities db = new PDBEntities())
            {
                dgvOrder.DataSource = db.OrderTbs.ToList<OrderTb>();
            }

        }
        #endregion

        private void dgvOrder_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Hide();
            Home home = new Home();
            home.Show();
        }
    }
}
