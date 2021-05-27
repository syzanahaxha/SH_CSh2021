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
    public partial class frmProducts : Form
    {
        #region Object
         Pizza pizza = new Pizza();
        #endregion
        #region Constructor
        public frmProducts()
        {
            InitializeComponent();
        }
        #endregion

        #region Events
        private void btnCancel_Click(object sender, EventArgs e)
        {
            Clear();
        }
        private void frmProducts_Load(object sender, EventArgs e)
        {
            Clear();
            FillGrid();
        }
        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Are you sure to delete this record?", "Information Delete Alert", MessageBoxButtons.YesNo) == DialogResult.Yes)
                using (PDBEntities db = new PDBEntities())
                {
                    var entry = db.Entry(pizza);
                    if (entry.State == EntityState.Detached)
                        db.Pizzas.Attach(pizza);
                    db.Pizzas.Remove(pizza);
                    db.SaveChanges();
                    FillGrid();
                    Clear();
                    MessageBox.Show("Deleted Successfully");
                }
        }
        private void btnSave_Click(object sender, EventArgs e)
        {
            pizza.Size = cmbSize.Text.Trim();
            pizza.Name = txtProductName.Text.Trim();
            pizza.QtyStock = int.Parse(this.txtQtyStock.Text);
            pizza.Price = int.Parse(this.txtProductPrice.Text);

            using (PDBEntities db = new PDBEntities())
            {
                if (pizza.PizzaId == 0) //insert
                    db.Pizzas.Add(pizza);
                else //update
                    db.Entry(pizza).State = EntityState.Modified;
                db.SaveChanges();
            }
            Clear();
            FillGrid();
            MessageBox.Show("Pizza added successfully!");
        }
        private void dgvProducts_DoubleClick(object sender, EventArgs e)
        {

        }
        #endregion

        #region Functions
        void Clear()
        {
            txtProductName.Text = cmbSize.Text = txtQtyStock.Text = txtProductPrice.Text = "";
            btnSave.Text = "save";
            btnDelete.Enabled = false;
            pizza.PizzaId = 0;
        }
        void FillGrid()
        {
            dgvProducts.AutoGenerateColumns = false;
            using (PDBEntities db = new PDBEntities())
            {
                dgvProducts.DataSource = db.Pizzas.ToList<Pizza>();
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
