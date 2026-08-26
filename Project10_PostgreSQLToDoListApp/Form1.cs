using Npgsql;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;

namespace Project10_PostgreSQLToDoListApp
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        string connectionString = "Server = localhost; Port = 5432; Database = DbProject10ToDoApp1; User Id = postgres; Password = hkdczg3y";

        void ToDoList()
        {
            NpgsqlConnection connection = new NpgsqlConnection(connectionString);
            connection.Open();
            NpgsqlCommand command = new NpgsqlCommand("select * from todolists", connection);
            NpgsqlDataAdapter dataAdapter = new NpgsqlDataAdapter(command);
            DataTable dataTable = new DataTable();
            dataAdapter.Fill(dataTable);
            dataGridView1.DataSource = dataTable;
            connection.Close();
        }

        void CategoryList()
        {
            NpgsqlConnection connection = new NpgsqlConnection(connectionString);
            connection.Open();
            NpgsqlCommand command = new NpgsqlCommand("select * from categories", connection);
            NpgsqlDataAdapter dataAdapter = new NpgsqlDataAdapter(command);
            DataTable dataTable = new DataTable();
            dataAdapter.Fill(dataTable);
            cmbCategory.DisplayMember = "CategoryName";
            cmbCategory.ValueMember = "CategoryId";
            cmbCategory.DataSource = dataTable;
            connection.Close();
        }

        private void btnCategoryList_Click(object sender, EventArgs e)
        {
            FrmCategory frm = new FrmCategory();
            frm.Show();
        }

        private void btnList_Click(object sender, EventArgs e)
        {
            ToDoList();
        }

        private void btnAllList_Click(object sender, EventArgs e)
        {
            var connection = new NpgsqlConnection(connectionString);
            connection.Open();
            string query = "select id, title, description, priority, status, categoryname from todolists\r\ninner join categories \r\non todolists.categoryid = categories.categoryid";
            var command = new NpgsqlCommand(query, connection);
            var adapter = new NpgsqlDataAdapter(command);
            DataTable dataTable = new DataTable();
            adapter.Fill(dataTable);
            dataGridView1.DataSource = dataTable;
            connection.Close();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            ToDoList();
            CategoryList();
        }

        private void btnCreate_Click(object sender, EventArgs e)
        {
            int categoryId = int.Parse(cmbCategory.SelectedValue.ToString());
            string title = txtTitle.Text;
            string priority = txtPriority.Text;
            string description = txtDescription.Text;
            string status = "B'0'";

            if (rdbCompleted.Checked)
            {
                status = "B'1'";
            }
            if (rdbContunie.Checked)
            {
                status = "B'0'";
            }

            var connection = new NpgsqlConnection(connectionString);
            connection.Open();
            string query = "INSERT INTO todolists (title, description, status, priority, categoryid) VALUES (@title, @description, B'1', @priority, @categoryid)";
            using (var command = new NpgsqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@title", title);
                command.Parameters.AddWithValue("@description", description);
                //command.Parameters.AddWithValue("@status", "B'1'");
                command.Parameters.AddWithValue("@priority", priority);
                command.Parameters.AddWithValue("@categoryid", categoryId);
                command.ExecuteNonQuery();
                MessageBox.Show("Yapılacak işlem eklendi.");
                ToDoList();
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            int id = int.Parse(txtId.Text);

            var connection = new NpgsqlConnection(connectionString);
            connection.Open();
            string query = "DELETE FROM ToDoLists WHERE Id = @id";
            using (var command = new NpgsqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@id", id);
                command.ExecuteNonQuery();
                MessageBox.Show("Veri başarıyla silindi.");
                CategoryList();
            }
            connection.Close();
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            int id = int.Parse(txtId.Text);
            int categoryId = int.Parse(cmbCategory.SelectedValue.ToString());
            string title = txtTitle.Text;
            string priority = txtPriority.Text;
            string description = txtDescription.Text;

            var connection = new NpgsqlConnection(connectionString);
            connection.Open();
            string query = "UPDATE ToDoLists SET Title = @title, Description = @description, Priority = @priority, CategoryId = @categoryId WHERE Id = @id";
            using (var command = new NpgsqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@title", title);
                command.Parameters.AddWithValue("@ID", id);
                command.Parameters.AddWithValue("@description", description);
                command.Parameters.AddWithValue("@priority", priority);
                command.Parameters.AddWithValue("@categoryId", categoryId);
                command.ExecuteNonQuery();
                MessageBox.Show("İşlem başarıyla güncellendi.");
                CategoryList();
            }
            connection.Close();
        }

        private void btnCheckedList_Click(object sender, EventArgs e)
        {
            var connection = new NpgsqlConnection(connectionString);
            connection.Open();
            string query = "Select * From ToDoLists Where Status=B'1'";
            var command = new NpgsqlCommand(query, connection);
            var adapter = new NpgsqlDataAdapter(command);
            DataTable dataTable = new DataTable();
            adapter.Fill(dataTable);
            dataGridView1.DataSource = dataTable;
            connection.Close();
        }

        private void btnCountunieList_Click(object sender, EventArgs e)
        {
            var connection = new NpgsqlConnection(connectionString);
            connection.Open();
            string query = "Select * From ToDoLists Where Status=B'0'";
            var command = new NpgsqlCommand(query, connection);
            var adapter = new NpgsqlDataAdapter(command);
            DataTable dataTable = new DataTable();
            adapter.Fill(dataTable);
            dataGridView1.DataSource = dataTable;
            connection.Close();
        }
    }
}

