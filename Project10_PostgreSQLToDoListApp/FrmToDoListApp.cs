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

namespace Project10_PostgreSQLToDoListApp
{
    public partial class FrmToDoListApp : Form
    {
        public FrmToDoListApp()
        {
            InitializeComponent();
        }
        string connectionString = "Server = localhost; Port = 5432; Database = DbProject10ToDoApp1; User Id = postgres; Password = hkdczg3y";
        private void FrmToDoListApp_Load(object sender, EventArgs e)
        {
            //NpgsqlConnection connection = new NpgsqlConnection ("");

            var connection = new NpgsqlConnection(connectionString);
            connection.Open();

            string query = "SELECT * FROM MyTable1";
            var command = new NpgsqlCommand(query, connection);
            var adapter = new NpgsqlDataAdapter(command);
            DataTable dataSet = new DataTable();
            adapter.Fill(dataSet);
            dataGridView1.DataSource = dataSet;
            connection.Close();
        }
    }
}
