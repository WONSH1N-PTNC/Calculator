using Calculator.Model;
using MySqlConnector;
using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Calculator.View
{
    /// <summary>
    /// Account.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class Account : Window
    {
        private readonly string _connectionString = "server=127.0.0.1; port=3306; user=ami; password=protnc; database=caldb;";
        public ObservableCollection<AccountControl> Accounts { get; set; } // 모델 연결
        public Account()
        {
            InitializeComponent();
            Accounts = new ObservableCollection<AccountControl>();
            AccountDataGrid.ItemsSource = Accounts;

            if (SessionManager.Authority == "user")
            {
                DeleteButton.IsEnabled = false; // 삭제 버튼 막기
            }
        }
        private void QueryButton_Click(object sender, RoutedEventArgs e)
        {
            Accounts.Clear();

            try
            {
                using (var conn = new MySqlConnection(_connectionString))
                {
                    conn.Open();

                    string sql = "SELECT account, id, password, email, auth, date FROM account;";
                    using (var cmd = new MySqlCommand(sql, conn))
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            string password = reader.GetString("password");

                            if (SessionManager.Authority == "user")
                            {
                                password = new string('*', password.Length);
                            }
                            Accounts.Add(new AccountControl
                            {
                                Account = reader.GetInt32("account"),
                                Id = reader.GetString("id"),
                                Password = password,
                                Email = reader.IsDBNull(reader.GetOrdinal("email")) ? "" : reader.GetString("email"),
                                Authority = reader.GetString("auth"),
                                Date = reader.GetDateTime("date")
                            });
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("[DB 오류]\n" + ex.Message, "오류",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            if (AccountDataGrid.SelectedItem is AccountControl selectedAccount)
            {
                try
                {
                    using (var conn = new MySqlConnection(_connectionString))
                    {
                        conn.Open();

                        string sql = "DELETE FROM account WHERE account = @account;";
                        using (var cmd = new MySqlCommand(sql, conn))
                        {
                            cmd.Parameters.AddWithValue("@account", selectedAccount.Account);
                            cmd.ExecuteNonQuery();
                        }
                    }

                    Accounts.Remove(selectedAccount); // UI에서도 제거
                }
                catch (Exception ex)
                {
                    MessageBox.Show("[DB 오류]\n" + ex.Message, "오류",
                        MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else
            {
                MessageBox.Show("삭제할 계정을 선택하세요.", "알림",
                    MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }
        private void Minimize_Click(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }

        private void Maximize_Click(object sender, RoutedEventArgs e)
        {
            WindowState = (WindowState == WindowState.Maximized) ? WindowState.Normal : WindowState.Maximized;
        }

        private void Close_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        // 상단바 드래그 가능하게
        protected override void OnMouseLeftButtonDown(MouseButtonEventArgs e)
        {
            if (e.ButtonState == MouseButtonState.Pressed)
                DragMove();
        }

    }
}


   


