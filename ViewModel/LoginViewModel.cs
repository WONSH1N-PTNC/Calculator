using Calculator.Model;
using Calculator.View;
using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Calculator.ViewModel
{
    public class LoginViewModel
    {
        private readonly string _connectionString =
            "server=127.0.0.1; port=3306; user=ami; password=protnc; database=caldb;";

        public AccountControl CurrentUser { get; private set; }
        public bool Login(string UserId, string password)
        {
            try
            {
                using (var conn = new MySqlConnection(_connectionString))
                {
                    conn.Open();

                    string sql = "SELECT account, id, password, email, auth, date " +
                             "FROM account WHERE id = @id AND password = @pw;";

                    using (var cmd = new MySqlCommand(sql, conn))
                    {
                        cmd.Parameters.AddWithValue("@id", UserId);
                        cmd.Parameters.AddWithValue("@pw", password);

                        using (var reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                //  로그인 성공 → CurrentUser 채움
                                CurrentUser = new AccountControl
                                {
                                    Account = reader.GetInt32("account"),
                                    Id = reader.GetString("id"),
                                    Password = reader.GetString("password"),
                                    Email = reader.IsDBNull(reader.GetOrdinal("email")) ? "" : reader.GetString("email"),
                                    Authority = reader.GetString("auth"),
                                    Date = reader.GetDateTime("date")
                                };
                                return true;
                            }
                        }
                    }
                }
                return false;
            }
            catch (Exception ex)
            {
                MessageBox.Show("[로그인 오류]\n" + ex.Message, "DB 오류", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
        }
        private void SignUpButton_Click(object sender, RoutedEventArgs e)
        {
            var signUpWindow = new SignUp();
            signUpWindow.Owner = this;
            signUpWindow.ShowDialog();
        }

        public static implicit operator Window(LoginViewModel v)
        {
            throw new NotImplementedException();
        }
    }
}
