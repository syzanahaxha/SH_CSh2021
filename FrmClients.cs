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
    public partial class FrmClients : Form
    {
        #region Object
        Costumer model = new Costumer();
        #endregion
        #region Constructor

        public FrmClients()
        {
            InitializeComponent();
        }
        #endregion
        #region Events
        private void btnCancel_Click(object sender, EventArgs e)
        {
            Clear();
        }
        private void FrmClients_Load(object sender, EventArgs e)
        {
            Clear();
            FillGrid();
        }
        private void btnSave_Click(object sender, EventArgs e)
        {
            model.FirstName = txtFirstname.Text.Trim();
            model.LastName = txtLastname.Text.Trim();
            model.Addresss = rtbAddress.Text.Trim();
            model.City = comboCity.Text.Trim();
            model.TelNo = txtPhone.Text.Trim();

            using (PDBEntities db = new PDBEntities())
            {
                if (model.CostumerId == 0) //insert
                    db.Costumers.Add(model);
                else //update
                    db.Entry(model).State = EntityState.Modified;
                db.SaveChanges();
            }
            Clear();
            FillGrid();
            MessageBox.Show("Costumer added successfully!");
        }
        private void btnDelete_Click(object sender, EventArgs e)
        {
            if(MessageBox.Show("Are you sure to delete this record?", "Information Delete Alert", MessageBoxButtons.YesNo) == DialogResult.Yes)
                using (PDBEntities db = new PDBEntities())
            {
                var entry = db.Entry(model);
                if (entry.State == EntityState.Detached)
                    db.Costumers.Attach(model);
                db.Costumers.Remove(model);
                db.SaveChanges();
                FillGrid();
                Clear();
                MessageBox.Show("Deleted Successfully");
            }
        }
        private void dgvCostumer_DoubleClick(object sender, EventArgs e)
        {
            if (dgvCostumer.CurrentRow.Index != -1)
            {
                model.CostumerId = Convert.ToInt32(dgvCostumer.CurrentRow.Cells["CustomerId"].Value);
                using (PDBEntities db = new PDBEntities())
                {
                    model = db.Costumers.Where(x => x.CostumerId == model.CostumerId).FirstOrDefault();
                    txtFirstname.Text = model.FirstName;
                    txtLastname.Text = model.LastName;
                    rtbAddress.Text = model.Addresss;
                    txtPhone.Text = model.TelNo;
                    comboCity.Text = model.City;
                }
                btnSave.Text = "Update";
                btnDelete.Enabled = true;

            }
        }
        #endregion
        #region Functions
        void Clear()
        {
            txtFirstname.Text = txtLastname.Text = txtPhone.Text = comboCity.Text = rtbAddress.Text = "";
            btnSave.Text = "save";
            btnDelete.Enabled = false;
            model.CostumerId = 0;
        }

        void FillGrid()
        {
            dgvCostumer.AutoGenerateColumns = false;
            using (PDBEntities db = new PDBEntities())
            {
                dgvCostumer.DataSource = db.Costumers.ToList<Costumer>();
            }

        }
        #endregion

        private void btnExit_Click(object sender, EventArgs e)
        {
            Home home = new Home();
            this.Hide();
            home.Show();
        }
    }
}
