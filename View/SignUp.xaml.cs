using Calculator.ViewModel;
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
    /// SignUp.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class SignUp : Window
    {
        private SignUpViewModel _vm = new SignUpViewModel();
        public SignUp()
        {
            InitializeComponent();
        }

        private void SignUpButton_Click(object sender, RoutedEventArgs e)
        {
            string id = IdInput.Text.Trim();
            string pw = PwInput.Password.Trim();
            string pwConfirm = PwConfirmInput.Password.Trim();
            string email = EmailInput.Text.Trim();

            if (string.IsNullOrEmpty(id) || string.IsNullOrEmpty(pw) || string.IsNullOrEmpty(email))
            {
                MessageBox.Show("모든 필드를 입력해주세요.", "오류", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (pw != pwConfirm)
            {
                MessageBox.Show("비밀번호가 일치하지 않습니다.", "오류", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (_vm.Register(id, pw, email))
            {
                MessageBox.Show("회원가입 성공! 로그인 해주세요.", "성공", MessageBoxButton.OK, MessageBoxImage.Information);
                this.Close();
            }
        }

        // ====== 힌트 제어 ======
        private void IdInput_TextChanged(object sender, RoutedEventArgs e)
        {
            IdHint.Visibility = string.IsNullOrEmpty(IdInput.Text) ? Visibility.Visible : Visibility.Hidden;
        }
        private void PwInput_PasswordChanged(object sender, RoutedEventArgs e)
        {
            PwHint.Visibility = string.IsNullOrEmpty(PwInput.Password) ? Visibility.Visible : Visibility.Hidden;
        }

        private void PwConfirmInput_PasswordChanged(object sender, RoutedEventArgs e)
        {
            PwConfirmHint.Visibility = string.IsNullOrEmpty(PwConfirmInput.Password) ? Visibility.Visible : Visibility.Hidden;
        }

        private void EmailInput_TextChanged(object sender, TextChangedEventArgs e)
        {
            EmailHint.Visibility = string.IsNullOrEmpty(EmailInput.Text) ? Visibility.Visible : Visibility.Hidden;
        }
    }
}
