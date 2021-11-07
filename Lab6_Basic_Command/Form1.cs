using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace Lab6_Basic_Command
{
    public partial class Form1 : Form
    {
        SqlConnection connection = null;
        SqlCommand command;

        string connectionString = "Data Source=HONGPHUC\\SQLEXPRESS;Initial Catalog=RestaurantManagement;Integrated Security=True";

        SqlDataAdapter adapter = new SqlDataAdapter();
        DataTable table = new DataTable();
        private object dgv;
        void loaddata()
        {
            command = connection.CreateCommand();
            command.CommandText = "select* dbo.Category";
            adapter.SelectCommand = command;
            table.Clear();
            adapter.Fill(table);
            //dgv.DataSource = table;
        }
        public Form1()
        {
            InitializeComponent();
        }

        private void btnLoad_Click(object sender, EventArgs e)
        {            
            SqlConnection sqlConnection = new SqlConnection(connectionString);
            SqlCommand sqlCommand = sqlConnection.CreateCommand();
            string query = "SELECT ID, Name, Type FROM Category";
            sqlConnection.Open();
            sqlCommand.CommandText = query;
            SqlDataReader sqlDataReader = sqlCommand.ExecuteReader();
            this.DisplayCategory(sqlDataReader);
            sqlConnection.Close();
        }
        private void DisplayCategory(SqlDataReader reader)
        {
            lvCategory.Items.Clear();
            while(reader.Read())
            {
                ListViewItem item = new ListViewItem(reader["ID"].ToString());
                lvCategory.Items.Add(item);
                item.SubItems.Add(reader["Name"].ToString());
                item.SubItems.Add(reader["Type"].ToString());
            }    
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            SqlConnection sqlConnection = new SqlConnection(connectionString);
            SqlCommand sqlCommand = sqlConnection.CreateCommand();
            sqlCommand.CommandText="INSERT INTO Category(Name,[Type]"+") VALUES(N'"+ txtName.Text+"',"+txtType.Text+ ")";
            sqlConnection.Open();
            int numOfRowEffected = sqlCommand.ExecuteNonQuery();
            sqlConnection.Close();
            if(numOfRowEffected==1)
            {
                MessageBox.Show("Thêm nhóm món ăn thành công");
                btnLoad.PerformClick();
                txtName.Text = "";
                txtType.Text = "";
            }
            else
            {
                MessageBox.Show("Đã có lỗi xảy ra.Vui lòng thử lại");
            }    
        }

        private void lvCategory_Click(object sender, EventArgs e)
        {
            ListViewItem item = lvCategory.SelectedItems[0];
            txtID.Text = item.Text;
            txtName.Text = item.SubItems[1].Text;
            txtType.Text = item.SubItems[2].Text == "0" ? "Thức uống" : "Đồ ăn";
            btnUpdate.Enabled = true;
            btnDelete.Enabled = true;
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            SqlConnection sqlConnection = new SqlConnection(connectionString);
            SqlCommand sqlCommand = sqlConnection.CreateCommand();
            int cateType = (txtType.Text == "Thức uống") ? 0 : 1;
            sqlCommand.CommandText = "UPDATE Category SET Name = N'" + txtName.Text + "', Type = " + cateType + " WHERE ID = " + txtID.Text;
            sqlConnection.Open();
            int numOfRowsEffected = sqlCommand.ExecuteNonQuery();
            sqlConnection.Close();
            if (numOfRowsEffected == 1)
            {
                ListViewItem item = lvCategory.SelectedItems[0];
                item.SubItems[1].Text = txtName.Text;
                item.SubItems[2].Text = cateType.ToString();
                txtID.Text = "";
                txtName.Text = "";
                txtType.Text = "";
                btnUpdate.Enabled = false;
                btnDelete.Enabled = false;
                MessageBox.Show("Cập nhật nhóm món ăn thành công");
            }
            else
            {
                MessageBox.Show("Đã có lỗi xảy ra vui lòng thử lại");
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            SqlConnection sqlConnection = new SqlConnection(connectionString);
            SqlCommand sqlCommand = sqlConnection.CreateCommand();
            sqlCommand.CommandText = "DELETE FROM Category" + " WHERE ID =" + txtID.Text;
            sqlConnection.Open();
            int numOfRowsEffected = sqlCommand.ExecuteNonQuery();
            sqlConnection.Close();
            if(numOfRowsEffected==1)
            {
                ListViewItem item = lvCategory.SelectedItems[0];
                lvCategory.Items.Remove(item);
                txtID.Text = "";
                txtName.Text = "";
                txtType.Text = "";
                btnUpdate.Enabled = false;
                btnDelete.Enabled = false;
                MessageBox.Show("Xoá nhóm món ăn thành công");

            }
            else
            {
                MessageBox.Show("Đã có lỗi xảy ra. Vui lòng thử lại");

            }   
        }
    }
}
