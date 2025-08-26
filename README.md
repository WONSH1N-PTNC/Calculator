📌 Calculator App (WPF, .NET Framework)

🖥️ 소개

이 프로젝트는 WPF(.NET Framework) 기반의 계산기 애플리케이션입니다.
기본적인 사칙연산 기능뿐만 아니라, 로그인 화면, 계정 관리, MVVM 아키텍처 적용을 포함하여 학습 및 실습 목적으로 제작되었습니다.

🚀 기능
🔢 계산기

사칙연산 (덧셈, 뺄셈, 곱셈, 나눗셈)

연산자 우선순위 적용

C (전체 Clear), CE (최근 입력 Clear) 지원

실수 및 정수 연산 지원 (천 단위 구분, 소수점 4자리까지 표시)

🔐 로그인

로그인 화면 → 계산기 화면으로 이동

사용자 계정 / 관리자 계정 구분

사용자 계정: ID만 표시, 비밀번호는 ****로 마스킹

관리자 계정: ID, Password, Level 표시

ID: 영문 + 숫자 12자리

PW: 8자리 이상, 두 종류 이상의 문자 조합

👥 계정 관리

계정 조회: 등록된 계정 목록 확인

계정 생성: ID, Password, Level 추가 가능

🏗️ 아키텍처

본 프로젝트는 MVVM 패턴을 기반으로 설계되었습니다.

View: XAML UI, 사용자 입력 및 시각적 표시 담당

ViewModel: UI와 Model 사이의 중개자 역할, 데이터 바인딩 처리

Model (CalcMgr): 계산 로직 및 상태 관리, 무한 정밀도 연산 지원

📂 프로젝트 구조
CalculatorApp/
 ├─ Views/           # XAML 화면 (LoginView, CalculatorView 등)
 ├─ ViewModels/      # ViewModel 클래스 (LoginVM, CalculatorVM 등)
 ├─ Models/          # 계산 로직 및 데이터 관리 (CalcMgr 등)
 ├─ Resources/       # 공통 스타일 및 리소스
 └─ App.xaml         # 앱 시작점

🛠️ 개발 환경

IDE: Visual Studio

Framework: .NET Framework (WPF)

언어: C#

패턴: MVVM
