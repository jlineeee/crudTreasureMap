using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace crudBatteryWFA
{
    public partial class Form2 : Form
    {
        private string connectionString = "Data Source=LAPTOP-TUB4BKLO\\SQLEXPRESS;Initial Catalog=crudBattery; Integrated Security=True";

        public Form2()
        {
            InitializeComponent();
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

        private void bUpdate_Click(object sender, EventArgs e)
        {
            updateRecord();
        }

        private void updateRecord()
        {
            try
            {
                SqlConnection con = new SqlConnection(connectionString);
                con.Open();
                int mapID = Int32.Parse(txtMapID.Text);
                int width = Int32.Parse(txtWidth.Text);
                int height = Int32.Parse(txtHeight.Text);
                string Query = "UPDATE Coordinates SET width = " + width + ", height = " + height + " WHERE mapID = " + mapID + ";";
                SqlCommand cmd = new SqlCommand(Query, con);
                cmd.ExecuteNonQuery();
                con.Close();
                MessageBox.Show("Data has been updated.");
            }
            catch (Exception c)
            {
                MessageBox.Show(c.Message);
            }
        }

        private void bDelete_Click(object sender, EventArgs e)
        {
            deleteRecord();
        }

        private void deleteRecord()
        {
            try
            {
                SqlConnection con = new SqlConnection(connectionString);
                con.Open();
                int mapID = Int32.Parse(txtMapID.Text);
                string Query = "DELETE FROM Coordinates WHERE mapID = " + mapID + ";";
                SqlCommand cmd = new SqlCommand(Query, con);
                cmd.ExecuteNonQuery();
                con.Close();
                MessageBox.Show("Data has been deleted.");
            }
            catch (Exception c)
            {
                MessageBox.Show(c.Message);
            }
        }

        private void bSearch_Click(object sender, EventArgs e)
        {
            searchOneRecord();
        }

        private void searchOneRecord()
        {
            try
            {
                SqlConnection con = new SqlConnection(connectionString);
                con.Open();
                int mapID = Int32.Parse(txtMapID.Text);
                SqlCommand cmd = new SqlCommand("SELECT * FROM Coordinates WHERE mapID = " + mapID, con);
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

        private void bModule1_Click(object sender, EventArgs e)
        {
            Form1 form1 = new Form1();
            form1.Show();
            this.Hide();
        }

        private void bCalculate_Click(object sender, EventArgs e)
        {
            try
            {
                int width = Convert.ToInt32(txtWidth.Text);
                int height = Convert.ToInt32(txtHeight.Text);

                // Generate mine field
                int[,] mineField = GenerateMineField(width, height);

                // Count treasures
                int treasureCount = CountTreasures(mineField, out bool[,] treasureCells);

                // Display mine field in DataGridView
                DisplayMineField(mineField, treasureCells);

                MessageBox.Show($"There are {treasureCount} hidden treasures in the map.");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private int[,] GenerateMineField(int width, int height)
        {
            int[,] mineField = new int[height, width];
            Random random = new Random();
            for (int i = 0; i < height; i++)
            {
                for (int j = 0; j < width; j++)
                {
                    mineField[i, j] = random.Next(10); // Random values from 0 to 9
                }
            }
            return mineField;
        }

        private void DisplayMineField(int[,] mineField, bool[,] treasureCells)
        {
            DataTable dt = new DataTable();

            for (int j = 0; j < mineField.GetLength(1); j++)
            {
                dt.Columns.Add(j.ToString(), typeof(int));
            }

            for (int i = 0; i < mineField.GetLength(0); i++)
            {
                DataRow row = dt.NewRow();
                for (int j = 0; j < mineField.GetLength(1); j++)
                {
                    row[j] = mineField[i, j];
                }
                dt.Rows.Add(row);
            }

            dataGridView1.DataSource = dt;
            dataGridView1.CellFormatting += (sender, e) =>
            {
                if (e.RowIndex >= 0 && e.ColumnIndex >= 0)
                {
                    if (e.RowIndex < treasureCells.GetLength(0) && e.ColumnIndex < treasureCells.GetLength(1))
                    {
                        if (treasureCells[e.RowIndex, e.ColumnIndex])
                        {
                            e.CellStyle.BackColor = Color.Yellow;
                        }
                    }
                }
            };
        }

        private int CountTreasures(int[,] mineField, out bool[,] treasureCells)
        {
            int treasureCount = 0;
            int height = mineField.GetLength(0);
            int width = mineField.GetLength(1);
            treasureCells = new bool[height, width];

            for (int i = 0; i < height - 1; i++)
            {
                for (int j = 0; j < width - 1; j++)
                {
                    int sum = mineField[i, j] + mineField[i, j + 1] + mineField[i + 1, j] + mineField[i + 1, j + 1];
                    if (sum % 4 == 0)
                    {
                        treasureCount++;
                        treasureCells[i, j] = true;
                        treasureCells[i, j + 1] = true;
                        treasureCells[i + 1, j] = true;
                        treasureCells[i + 1, j + 1] = true;
                    }
                }
            }
            return treasureCount;
        }
    }
}
