using Calculator.Model;
using Calculator.View;
using Calculator.ViewModel;
using DevExpress.Xpf.Core;
using System;
using System.Windows;
using System.Windows.Controls;

namespace Calculator
{
    /// <summary>
    /// MainWindow.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class MainWindow : ThemedWindow
    {
        private Cal_ViewModel _calViewModel;
        private CalNoteViewModel _calNoteViewModel; // CalNote.xaml의 ViewModel
        public MainWindow()
        {

            InitializeComponent();

            _calViewModel = new Cal_ViewModel();
            _calNoteViewModel = new CalNoteViewModel();

            DataContext = _calViewModel;

            // 계산이 끝났을 때 메모 추가
            _calViewModel.OnCalculationFinished += (exp, result) =>
            {
                _calNoteViewModel.AddNote(exp, result);
            };
        }

        // 화면 이동
        private void MenuButton_Click(object sender, RoutedEventArgs e)
        {
            MenuPopup.IsOpen = true;
        }
      
        // 창 토글 기능 추가
        private Account _accountWindow;
        private void OpenAccountWindow()
        {
            if (SessionManager.Authority == "guest")
            {
                MessageBox.Show("게스트는 접근할 수 없습니다.");
                return;
            }

            if (_accountWindow == null || !_accountWindow.IsLoaded)
            {
                _accountWindow = new Account();
                _accountWindow.DataContext = new AccountControl();
                PositionWindowRightOfMain(_accountWindow);
                _accountWindow.Owner = this;
                _accountWindow.Closed += (s, e) => _accountWindow = null; // 닫히면 참조 해제
                _accountWindow.Show();
            }
            else
            {
                _accountWindow.Close(); 
            }
        }
        private CalNote _calNoteWindow;
        private void OpenCalNoteWindow()
        {
            if (SessionManager.Authority == "guest")
            {
                MessageBox.Show("게스트는 접근할 수 없습니다.");
                return;
            }

            if (_calNoteWindow == null || !_calNoteWindow.IsLoaded)
            {
                _calNoteWindow = new CalNote(_calNoteViewModel);
                PositionWindowRightOfMain(_calNoteWindow);
                _calNoteWindow.Owner = this;
                _calNoteWindow.Closed += (s, e) => _calNoteWindow = null; 
                _calNoteWindow.Show();
            }
            else
            {
                _calNoteWindow.Close(); 
            }
        }

        // 메뉴 리스트 
        private void MenuListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.AddedItems.Count == 0)
                return;

            var selected = (e.AddedItems[0] as ListBoxItem)?.Content?.ToString();
            MenuPopup.IsOpen = false;

            switch (selected)
            {
                case "로그인":
                    TryLogin();
                    break;
                case "로그아웃":
                    TryLogout();
                    break;
                case "계정관리":
                    OpenAccountWindow();
                    break;
                case "메모지":
                    OpenCalNoteWindow();
                    break;

            }

         (sender as ListBox).SelectedIndex = -1;
        }
        private void PositionWindowRightOfMain(Window newWindow)
        {
            newWindow.WindowStartupLocation = WindowStartupLocation.Manual;

            var mainLeft = this.Left;
            var mainTop = this.Top;
            var mainWidth = this.ActualWidth;

            var screenWidth = SystemParameters.VirtualScreenWidth;

            // 오른쪽 공간 부족하면 왼쪽에 붙이기
            double desiredLeft = mainLeft + mainWidth;
            if (desiredLeft + newWindow.Width > screenWidth)
                desiredLeft = mainLeft - newWindow.Width;

            newWindow.Left = desiredLeft;
            newWindow.Top = mainTop;
        }

        private bool _isLoggedIn = false;

        private void CheckLoginStatus()
        {
            if (LoginCheck != null)
            {
                LoginCheck.Content = _isLoggedIn ? "로그아웃" : "로그인";
            }
        }
        private void TryLogin()
        {

            if (!_isLoggedIn)
            {
                var loginWindow = new Login();
                loginWindow.Owner = this;
                loginWindow.WindowStartupLocation = WindowStartupLocation.CenterOwner;
                
                bool? result = loginWindow.ShowDialog();

                if (result == true)
                {
                    _isLoggedIn = true; // 로그인 성공 
                }
                else
                {
                    _isLoggedIn = false;
                }
            }
            else
            {
                MessageBox.Show("이미 로그인되어 있습니다.", "알림", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            CheckLoginStatus();
        }
        private void TryLogout()
        {
            if (_isLoggedIn)
            {
                MessageBoxResult result = MessageBox.Show("로그아웃 하시겠습니까?", "로그아웃 확인", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (result == MessageBoxResult.Yes)
                {
                    _isLoggedIn = false; // 로그아웃 처리
                    SessionManager.Authority = "guest"; // 권한 초기화
                    SessionManager.CurrentUserId = null; // 사용자 정보 초기화

                    if (_accountWindow != null && _accountWindow.IsLoaded)
                    {
                        _accountWindow.Close();
                        _accountWindow = null;
                    }
                    _calNoteViewModel.ClearNotes();
                    if (_calNoteWindow != null && _calNoteWindow.IsLoaded)
                    {
                        _calNoteWindow.Close();
                        _calNoteWindow = null;

                    }
                    _calViewModel.ClearCommand.Execute("CA");
                    CheckLoginStatus();
                }
            }
            else
            {
                MessageBox.Show("로그인되어 있지 않습니다.", "알림", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private void btnEqual_Click(object sender, RoutedEventArgs e)
        {
            // 계산 후 메모지에 추가
            //AddToNote();
        }
        private void ViewModel_CalculationCompleted(object sender, EventArgs e)
        {
            // AddToNote(); 
        }
    }
}

