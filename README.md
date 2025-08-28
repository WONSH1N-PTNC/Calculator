# 📌 Calculator App  

## 🖥️ 소개  
이 프로젝트는 **WPF(.NET Framework)** 기반의 계산기 애플리케이션입니다.  
기본적인 사칙연산 기능뿐만 아니라, **로그인 화면, 계정 관리, MVVM 아키텍처 적용**을 포함하여  
학습 및 실습 목적으로 제작되었습니다.  

---

## 🚀 기능  

### 🔢 계산기  
- 사칙연산 (➕ 덧셈, ➖ 뺄셈, ✖ 곱셈, ➗ 나눗셈)  
- 연산자 우선순위 적용  
- C (전체 Clear), CE (최근 입력 Clear) 지원  
- 정수 및 실수 연산 지원  
- 정수: 천 단위 구분 기호 표시  
- 실수: 소수점 4자리까지 표시  

---

### 🔐 로그인  
- 로그인 화면 → 계산기 화면 이동  
- 게스트 / 사용자 계정 / 관리자 계정 구분  
- 비밀번호 찾기 → 비밀번호 찾기 화면 이동  
- 비밀번호 찾기 / 이메일 전송 지원  

---

### 👥 계정 관리  
- **계정 조회**: 등록된 계정 목록 확인  
- **계정 생성**: ID, Password, Email 등록 (기본 권한: User)  
- **사용자 계정**: ID, 비밀번호는 **** 마스킹, Email, 권한, 등록일 표시  
- **관리자 계정**: ID, Password 직접 표시  

---

## 🏗️ 아키텍처  
본 프로젝트는 **MVVM 패턴**을 기반으로 설계되었습니다.  

- **View**: XAML UI, 사용자 입력 및 시각적 표시 담당  
- **ViewModel**: UI와 Model 사이의 중개자 역할, 데이터 바인딩 처리  
- **Model (CalcMgr)**: 계산 로직 및 상태 관리, 무한 정밀도 연산 지원  

---

## 📂 프로젝트 구조  
```plaintext
CalculatorApp/
 ├─ Views/         # XAML 화면 (LoginView, CalculatorView 등)
 ├─ ViewModels/    # ViewModel 클래스 (LoginVM, CalculatorVM 등)
 ├─ Models/        # 계산 로직 및 데이터 관리 (CalcMgr 등)
 ├─ Resources/     # 공통 스타일 및 리소스
 └─ App.xaml       # 앱 시작점

```
---
  
🛠️ 개발 환경

IDE: Visual Studio

Framework: .NET Framework 4.7.2 (WPF)

언어: C# 7.3

패턴: MVVM
