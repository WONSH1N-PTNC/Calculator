using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Calculator.ViewModel
{
    public class SignUpViewModel
    {
        private readonly string _connectionString = "server=127.0.0.1; port=3306; user=ami; password=protnc; database=caldb;";

        public bool Register(string id, string password, string email)
        {
            try
            {
                using (var conn = new MySqlConnection(_connectionString))
                {
                    conn.Open();

                    string checkSql = "SELECT COUNT(*) FROM account WHERE id=@id OR email=@em";
                    using (var checkCmd = new MySqlCommand(checkSql, conn))
                    {
                        checkCmd.Parameters.AddWithValue("@id", id);
                        checkCmd.Parameters.AddWithValue("@em", email);
                        int exists = Convert.ToInt32(checkCmd.ExecuteScalar());
                        if (exists > 0)
                        {
                            MessageBox.Show("이미 사용중인 아이디 또는 이메일입니다.",
                                "회원가입 오류", MessageBoxButton.OK, MessageBoxImage.Warning);
                            return false;
                        }
                    }

                    string sql = "INSERT INTO account (id, password, email, auth, date) VALUES (@id, @pw, @em, 'user', NOW())";
                    using (var cmd = new MySqlCommand(sql, conn))
                    {
                        cmd.Parameters.AddWithValue("@id", id);
                        cmd.Parameters.AddWithValue("@pw", password);
                        cmd.Parameters.AddWithValue("@em", email);

                        int rows = cmd.ExecuteNonQuery();
                        return rows > 0;
                    }
                }
            }
            catch (Exception ex) 
            {
                MessageBox.Show("[회원가입 오류]\n" +  ex.Message, "DB 오류",
                    MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
        }
    }
}
