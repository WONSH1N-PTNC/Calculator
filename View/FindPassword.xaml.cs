using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;

namespace Calculator.View
{
    /// <summary>
    /// FindPassword.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class FindPassword : Window
    {
        private readonly string _connectionString = "server=127.0.0.1; port=3306; user=ami; password=protnc; database=caldb;";
        public FindPassword()
        {
            InitializeComponent();
        }

        // ====== 힌트 제어 ======
        private void IdInput_TextChanged(object sender, TextChangedEventArgs e)
        {
            IdHint.Visibility = string.IsNullOrEmpty(IdInput.Text) ? Visibility.Visible : Visibility.Hidden;
        }
        private void EmailInput_TextChanged(object sender, TextChangedEventArgs e)
        {
            EmailHint.Visibility = string.IsNullOrEmpty(EmailInput.Text) ? Visibility.Visible : Visibility.Hidden;
        }
        
        // 찾기 버튼 클릭
        private void FindButton_Click(object sender, RoutedEventArgs e)
        {
            string id = IdInput.Text.Trim();
            string email = EmailInput.Text.Trim();

            if (string.IsNullOrEmpty(id) || string.IsNullOrEmpty(email))
            {
                MessageBox.Show("아이디와 이메일을 입력하세요.", "입력 오류",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            try
            {
                using (var conn = new MySqlConnection(_connectionString))
                {
                    conn.Open();

                    // 계정 정보 확인
                    string sql = "SELECT password FROM account WHERE id = @id AND email = @em;";
                    using (var cmd = new MySqlCommand(sql, conn))
                    {
                        cmd.Parameters.AddWithValue("@id", id);
                        cmd.Parameters.AddWithValue("@em", email);

                        var pwObj = cmd.ExecuteScalar();
                        if (pwObj == null || pwObj == DBNull.Value)
                        {
                            MessageBox.Show("해당 계정이 존재하지 않습니다.", "오류",
                                MessageBoxButton.OK, MessageBoxImage.Error);
                            return;
                        }

                        string password = Convert.ToString(pwObj);

                        // 이메일 발송
                        SendPasswordEmail(email, password);

                        MessageBox.Show("비밀번호가 이메일로 전송되었습니다.",
                            "완료", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("[DB 오류]\n" + ex.Message, "오류",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        // 이메일 발송
        private void SendPasswordEmail(string email, string password)
        {
            try
            {
                // TLS 1.2 강제
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

                // (임시) 인증서 검증 무시
                ServicePointManager.ServerCertificateValidationCallback =
                    delegate (object s, X509Certificate certificate,
                              X509Chain chain, SslPolicyErrors sslPolicyErrors)
                    {
                        return true; // 항상 인증서 통과
                    };

                using (var smtp = new SmtpClient("mail.protnc.co.kr", 587))
                {
                    smtp.Credentials = new NetworkCredential("ws.park@protnc.co.kr", "240309kl!@");
                    smtp.EnableSsl = true; // 필요 시 false로 조정

                    string fromAddress = "ws.park@protnc.co.kr";
                    string toAddress = email;

                    using (var mail = new MailMessage(fromAddress, toAddress))
                    {
                        mail.Subject = "[계산기 앱] 비밀번호 찾기";
                        mail.Body = $"요청하신 계정의 비밀번호는 다음과 같습니다:\n\n{password}\n\n(테스트용 기능입니다)";

                        smtp.Send(mail);
                    }
                }
            }
            catch (SmtpException ex)
            {
                MessageBox.Show($"메일 발송 실패: {ex.Message}", "SMTP 오류",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"오류: {ex.Message}", "에러",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}


