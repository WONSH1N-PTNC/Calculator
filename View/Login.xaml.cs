using Calculator.Model;
using System;
using System.Collections.Generic;
using System.Linq;
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

namespace Calculator.View
{
    /// <summary>
    /// Loggin.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class Login : Window
    {
        public Login()
        {
            InitializeComponent();
            CheckPasswordHintVisibility();
        }
        private void ComboBox_GotFocus(object sender, RoutedEventArgs e)
        {
            var comboBox = sender as ComboBox;
            if (comboBox.Text == "Username")
            {
                comboBox.Text = "";
                comboBox.Foreground = Brushes.Gray;
            }
        }

        private void ComboBox_LostFocus(object sender, RoutedEventArgs e)
        {
            var comboBox = sender as ComboBox;
            if (string.IsNullOrWhiteSpace(comboBox.Text))
            {
                comboBox.Text = "Username";
                comboBox.Foreground = Brushes.Gray;
            }
        }
        private void PasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(PasswordInput.Password))
            {
                PasswordHint.Visibility = Visibility.Visible;
            }
            else
            {
                PasswordHint.Visibility = Visibility.Collapsed;
            }
        }
        private void CheckPasswordHintVisibility()
        {
            PasswordHint.Visibility = string.IsNullOrEmpty(PasswordInput.Password)
                ? Visibility.Visible
                : Visibility.Collapsed;
        }
        // 로그인 버튼 클릭 이벤트 핸들러
        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            var viewModel = new ViewModel.LoginViewModel();

            string userId = IdComboBox.Text.Trim();
            string password = PasswordInput.Password;

            if (viewModel.Login(userId, password)) // 로그인 성공
            {
                // 로그인 성공 시, ViewModel이 들고 있는 CurrentUser 사용
                SessionManager.CurrentUserId = viewModel.CurrentUser.Id;
                SessionManager.Authority = viewModel.CurrentUser.Authority;

                DialogResult = true;
                this.Close(); // 로그인 창 닫기
            }
            else
            {
                // 로그인 실패
                MessageBox.Show("로그인 실패! 아이디와 비밀번호를 확인하세요.", "알림", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        // 회원가입 버튼 클릭 이벤트 핸들러
        private void SignUpButton_Click(object sender, RoutedEventArgs e)
        {
            var signUpWindow = new SignUp();
            signUpWindow.Owner = this;
            signUpWindow.ShowDialog();
        }

        // 비밀번호 찾기 버튼 클릭 이벤트 핸들러
        private void FindPassword_Click(object sender, RoutedEventArgs e)
        {
            var findWindow = new FindPassword();
            findWindow.Owner = this;
            findWindow.ShowDialog();
        }

        // 단축키 이벤트 
        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.T && Keyboard.Modifiers == ModifierKeys.Control)
            {
                SessionManager.CurrentUserId = "PTNC";
                SessionManager.Authority = "admin";

                MessageBox.Show("관리자(admin) 권한으로 로그인되었습니다!",
                    "알림", MessageBoxButton.OK, MessageBoxImage.Information);

                DialogResult = true;
                this.Close();
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
