using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace crudBatteryWFA
{
    public partial class Form1 : Form
    {
        private string connectionString = "Data Source=LAPTOP-TUB4BKLO\\SQLEXPRESS;Initial Catalog=crudBattery; Integrated Security=True";

        public Form1()
        {
            InitializeComponent();
            LoadData();
        }

        private void LoadData()
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                con.Open();
                string query = "SELECT * FROM Coordinates;";
                SqlDataAdapter adapter = new SqlDataAdapter(query, con);
                DataTable dataTable = new DataTable();
                adapter.Fill(dataTable);
                dataGridView1.DataSource = dataTable;

                if (dataTable.Rows.Count > 0)
                {
                    // Assuming MapID is in the first row of the DataTable
                    txtMapID.Text = dataTable.Rows[0]["MapID"].ToString();
                    txtWidth.Text = dataTable.Rows[0]["Width"].ToString();
                    txtHeight.Text = dataTable.Rows[0]["Height"].ToString();
                }

                dataGridView1.DataSource = dataTable;
            }
        }

        private void label1_Click(object sender, EventArgs e) { }

        private void Form1_Load(object sender, EventArgs e)
        {
            txtMapID.Enabled = false; // MapID is non-editable
        }

        private void button1_Click(object sender, EventArgs e) //Add button
        {
            try
            {
                int width = Convert.ToInt32(txtWidth.Text);
                int height = Convert.ToInt32(txtHeight.Text);

                if (width < 2 || height < 2)
                {
                    MessageBox.Show("Width and Height must be at least 2. Please enter valid dimensions.");
                    txtWidth.Clear();
                    txtHeight.Clear();
                    return;
                }

                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    con.Open();
                    string query = "INSERT INTO Coordinates(width, height) VALUES(@width, @height)";
                    SqlCommand cmd = new SqlCommand(query, con);
                    cmd.Parameters.AddWithValue("@width", width);
                    cmd.Parameters.AddWithValue("@height", height);
                    cmd.ExecuteNonQuery();
                }
                MessageBox.Show("Data has been added.");
                LoadData(); // Refresh the DataGridView
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void bModule2_Click(object sender, EventArgs e)
        {
            Form2 module2Form = new Form2();
            module2Form.Show();
            this.Hide();
        }

        private void bRefresh_Click(object sender, EventArgs e)
        {
            readAllData();
        }

        private void readAllData()
        {
            try
            {
                SqlConnection con = new SqlConnection(connectionString);
                con.Open();
                SqlCommand cmd = new SqlCommand("SELECT * FROM Coordinates;", con);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);
                dataGridView1.DataSource = dt;
                con.Close();
            }
            catch (Exception c)
            {
                MessageBox.Show(c.Message);
            }
        }

        private void txtMapID_TextChanged(object sender, EventArgs e) { }

        private void txtWidth_TextChanged(object sender, EventArgs e) { }

        private void txtHeight_TextChanged(object sender, EventArgs e) { }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e) { }
    }
}
