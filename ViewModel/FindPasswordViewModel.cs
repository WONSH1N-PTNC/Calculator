using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Calculator.ViewModel
{
    public class FindPasswordViewModel
    {
        private readonly string _connectionString =
        "server=127.0.0.1; port=3306; user=ami; password=protnc; database=caldb;";

        public string GetPasswordByEmail(string email)
        {
            try
            {
                using (var conn = new MySqlConnection(_connectionString))
                {
                    conn.Open();
                    string sql = "SELECT password FROM account WHERE email = @em";
                    using (var cmd = new MySqlCommand(sql, conn))
                    {
                        cmd.Parameters.AddWithValue("@em", email);
                        var result = cmd.ExecuteScalar();
                        return result?.ToString(); // 없으면 null
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("[DB 오류]\n" + ex.Message, "DB 오류", MessageBoxButton.OK, MessageBoxImage.Error);
                return null;
            }
        }
    }
}
