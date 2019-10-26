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
using Dapper;

namespace UsuarisWinFormUi
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        private String connectionString = "Server=127.0.0.1;Database=biblioteca;Uid=usuari;Pwd=seCret_16";
        private void RegisterButton_Click(object sender, EventArgs e)
        {
            if (PasswordTextBox.Text == ConfirmPasswordTextBox.Text)
            {
                MySqlConnection con = new MySqlConnection(connectionString);
                string sql = "SELECT MAX(ID_USR) FROM USUARIS";
                int newId = 0;
                try
                {
                    newId = con.Query<int>(sql).FirstOrDefault(); // FirstOrDefault --> LINQ
                }
                catch(Exception ex)
                {
                    Console.WriteLine("Warning: Exception trhown " + ex.Message);
                }
                // Control Of No Values In Register (In progres)
                /*
                Boolean checkRegister = string.IsNullOrEmpty(NIFTextBox.Text);
                checkRegister = string.IsNullOrEmpty(NameText.Text);
                if (checkRegister == true)
                {
                    MessageBox.Show("You Dindn't Put Any Register Values", "User Manager", MessageBoxButtons.OK);
                }*/
                string sqlInsert = $"INSERT INTO USUARIS (ID_USR, DNI_USR, NOM_USR, LLINATGE1, POB_USR, EMAIL_USR, PASSWORD)" +
                    $"VALUES ({newId + 1}, '{NIFTextBox.Text}', '{NameTextBox.Text}', '{SurnameTextBox.Text}', '{TownComboBox.Text}'" +
                    $", '{EmailTextBox.Text}', '{PasswordTextBox.Text}')";
                try
                {
                    var rowsAffected = con.Execute(sqlInsert);
                    if (rowsAffected == 1)
                    {
                        MessageBox.Show("User registered succesfuly", "User Manager", MessageBoxButtons.OK);
                        NIFTextBox.Text = string.Empty;
                        NameTextBox.Text = string.Empty;
                        SurnameTextBox.Text = string.Empty;
                        TownComboBox.Text = string.Empty;
                        EmailTextBox.Text = string.Empty;
                        PasswordTextBox.Text = string.Empty;
                        ConfirmPasswordTextBox.Text = string.Empty;
                    }
                }
                catch(MySql.Data.MySqlClient.MySqlException ex)
                {
                    Console.WriteLine("Warning: Exception trhown " + ex.Message);
                }
               
                con.Close();
            }
            
        }

        private void FindButton_Click(object sender, EventArgs e)
        {
            string sql = "";
            List<User> users = new List<User>();
            MySqlConnection con = new MySqlConnection(connectionString);
            Boolean name = string.IsNullOrEmpty(SearchNameTextBox.Text);
            Boolean surname = string.IsNullOrEmpty(NameToSearchTextBox.Text);
            Boolean town = string.IsNullOrEmpty(TownTextBox.Text);

            if (name == true && surname == true && town == true)
            {
                MessageBox.Show("You Didn't Introduce Any Kind To Find", "User Manager", MessageBoxButtons.OK);
            }
            else if (name == true && surname == true && town == false)
            {
                sql = $"SELECT ID_USR, NOM_USR, LLINATGE1, POB_USR FROM USUARIS WHERE POB_USR = '{TownTextBox.Text}'";
            }
            else if (name == true && surname == false && town == true)
            {
                sql = $"SELECT ID_USR, NOM_USR, LLINATGE1, POB_USR FROM USUARIS WHERE LLINATGE1 = '{NameToSearchTextBox.Text}'";
            }
            else if (name == true && surname == false && town == false)
            {
                sql = $"SELECT ID_USR, NOM_USR, LLINATGE1, POB_USR FROM USUARIS WHERE LLINATGE1 = '{NameToSearchTextBox.Text}' AND POB_USR = '{TownTextBox.Text}'";
            }
            else if (name == false && surname == true && town == true)
            {
                sql = $"SELECT ID_USR, NOM_USR, LLINATGE1, POB_USR FROM USUARIS WHERE NOM_USR = '{SearchNameTextBox.Text}'";
            }
            else if (name == false && surname == true && town == false)
            {
                sql = $"SELECT ID_USR, NOM_USR, LLINATGE1, POB_USR FROM USUARIS WHERE NOM_USR = '{SearchNameTextBox.Text}' AND POB_USR = '{TownTextBox.Text}'";
            }
            else if (name == false && surname == false && town == true)
            {
                sql = $"SELECT ID_USR, NOM_USR, LLINATGE1, POB_USR FROM USUARIS WHERE NOM_USR = '{SearchNameTextBox.Text}' AND LLINATGE1 = '{NameToSearchTextBox.Text}'";
            }
            else if (name == false && surname == false && town == false)
            {
                sql = $"SELECT ID_USR, NOM_USR, LLINATGE1, POB_USR FROM USUARIS WHERE  NOM_USR = '{SearchNameTextBox.Text}' AND LLINATGE1 = '{NameToSearchTextBox.Text}' AND POB_USR = '{TownTextBox.Text}'";
            }

            users = con.Query<User>(sql).ToList();
            ListUser.DataSource = users;
            ListUser.DisplayMember = "FullInfo";

            SearchNameTextBox.Text = string.Empty;
            NameToSearchTextBox.Text = string.Empty;
            TownTextBox.Text = string.Empty;
            con.Close();
        }
    }
}
