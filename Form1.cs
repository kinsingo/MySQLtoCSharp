using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace MySqlCRUD
{
    public partial class Form1 : Form
    {
        const string connectionString = "Server=localhost;Database=bookDB;Uid=root;Pwd=!jonghyun04;";
        int bookID = 0;
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            Clear();
            DGVFill();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            using(MySqlConnection mysqlCon = new MySqlConnection(connectionString))
            {
                mysqlCon.Open();
                MySqlCommand mysqlcmd = new MySqlCommand("BookAddOrEdit", mysqlCon);
                mysqlcmd.CommandType = CommandType.StoredProcedure;
                mysqlcmd.Parameters.AddWithValue("_BookID", bookID);
                mysqlcmd.Parameters.AddWithValue("_BookName", txtBookName.Text.Trim());
                mysqlcmd.Parameters.AddWithValue("_Author", txtAuthor.Text.Trim());
                mysqlcmd.Parameters.AddWithValue("_Description", txtDescription.Text.Trim());
                mysqlcmd.ExecuteNonQuery();
                MessageBox.Show("Submitted Successfully");
                Clear();
                DGVFill();
            }
        }

        private void DGVFill()
        {
            using (MySqlConnection mysqlCon = new MySqlConnection(connectionString))
            {
                mysqlCon.Open();
                MySqlDataAdapter sqlDA = new MySqlDataAdapter("BookViewAll", mysqlCon);
                sqlDA.SelectCommand.CommandType = CommandType.StoredProcedure;
                DataTable datatable = new DataTable();
                sqlDA.Fill(datatable);
                dgvBook.DataSource = datatable;
                //dgvBook.Columns[0].Visible = false;//BookID should not be shown.
            }
        }

        private void Clear()
        {
            txtBookName.Clear();
            txtAuthor.Clear();
            txtDescription.Clear();
            txtSearch.Clear();
            bookID = 0;
            btnSave.Text = "Save";
            btnDelete.Enabled = false;
        }

        private void dgvBook_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dgvBook.CurrentRow.Index != -1)
            {
                bookID = Convert.ToInt32(dgvBook.CurrentRow.Cells[0].Value.ToString());
                txtBookName.Text = dgvBook.CurrentRow.Cells[1].Value.ToString();
                txtAuthor.Text = dgvBook.CurrentRow.Cells[2].Value.ToString();
                txtDescription.Text = dgvBook.CurrentRow.Cells[3].Value.ToString();
                btnSave.Text = "Update";
                btnDelete.Enabled = true;
            }
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            using (MySqlConnection mysqlCon = new MySqlConnection(connectionString))
            {
                mysqlCon.Open();
                MySqlDataAdapter sqlDA = new MySqlDataAdapter("booksearchByValues", mysqlCon);
                sqlDA.SelectCommand.CommandType = CommandType.StoredProcedure;
                sqlDA.SelectCommand.Parameters.AddWithValue("_SearchValue", txtSearch.Text.Trim());
                DataTable datatable = new DataTable();
                sqlDA.Fill(datatable);
                dgvBook.DataSource = datatable;
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            Clear();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            string sql_cmd = "CREATE TABLE " + textBox_TableName.Text + 
               @"(BookID INT NOT NULL AUTO_INCREMENT,
               BookName VARCHAR(45) NULL,
               Author VARCHAR(45) NULL,
              Description VARCHAR(250) NULL,
              PRIMARY KEY(`BookID`))";

            using (MySqlConnection mysqlCon = new MySqlConnection(connectionString))
            {
                mysqlCon.Open();
                MySqlCommand mysqlcmd = new MySqlCommand(sql_cmd.ToString(), mysqlCon);
                mysqlcmd.CommandType = CommandType.Text;
                mysqlcmd.ExecuteNonQuery();

                MessageBox.Show(textBox_TableName.Text + " has been created");
            }
        }

        private void button_Show_Tables_Click(object sender, EventArgs e)
        {
            using (MySqlConnection mysqlCon = new MySqlConnection(connectionString))
            {
                mysqlCon.Open();
               
                //List<string> QueryResult = new List<string>();
                string ans = "";

                MySqlCommand mysqlcmd = new MySqlCommand("show tables", mysqlCon);
                MySqlDataReader reader = mysqlcmd.ExecuteReader();
                while (reader.Read())
                {
                    //QueryResult.Add(reader.GetString(0));
                    ans += (reader.GetString(0) + "\n");

                }
                reader.Close();

                MessageBox.Show(ans);
            }


             
        }
    }
}
