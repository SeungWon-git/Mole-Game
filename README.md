# 🕳️🐹🔨 Mole Game (두더지 잡기 게임)

## 🎮 개요
 - 게임 장르: 온라인 아케이드 게임
 - 플랫폼: PC
 - 플레이 인원: 1~4인 (* 전체 접속 가능인원: 20명)
 - 1인 개발
 - **구현 내용**:
   > + **Google Firebase**를 이용한 DB 관리
   > + **Photon PUN 2**(Photon Unity Networking)를 이용하여 최대 4인 멀티 경쟁 [점수 경쟁]
   > + **랜덤한 두더지의 튀어나오는 패턴** 구현 (3가지 패턴) & 두더지 패턴 동기화
   > + 두더지의 애니메이션, 피격 이펙트, 점수 +/- 이미지 효과 직접 제작
 - 개발에 사용된 기술 스택:
   + Unity (2021.2.7f1 ver)
   + Google Firebase Realtime Database
   + FireSharp ver 2.0.4 SDK
   + PUN 2 'FREE ver 2.40.0'
   + Net API - '.Net Standard 2.1'
 - 시연 영상: [▶️ 유튜브 영상 보기](https://www.youtube.com/)
<img src="http://img.youtube.com/vi//0.jpg" width="400">

---

## 📝 구현 내용 상세 설명

### 💾 Firebase 활용
- ‘DataBase.cs’를 만들고 FireSharp 라이브러리를 활용하여 Firebase Realtime DataBase를 활용
- 데이터는 다음과 같이 저장되어 있다 → { 아이디: ● ● ● ● { 비번: ★ ★ ★ ★, 월드 랭크 점수: 1, 로그인 중: false, 인게임 중: false } }
* Firebase RealTime Database 콘솔의 데이터 트리 화면
<img width="235" height="379" alt="image" src="https://github.com/user-attachments/assets/26c8f2a8-d21f-42b4-a723-67b248284128" />

* 로그인 화면
<img width="357" height="268" alt="image" src="https://github.com/user-attachments/assets/22db60ca-3a43-453a-ac54-c0144ae97bc3" />

- 회원가입, 로그인  
  * 회원가입시 중복 아이디 검사를 시행하며, 중복 알림 Text를 아래에 띄운다.
  * 회원가입시 비밀번호 설정을 안 하였다면 오류 메시지를 띄운다.
  * 로그인 시 아이디가 없거나 아이디와 비밀번호가 맞지 않거나 이미 사용 중 (접속 중)인 계정이라면 오류 메시지를 띄운다.
* 로비, 결과 화면 → 모두 Firebase Realtime DB와 연동되어 표시되고 있다.
<img width="382" height="268" alt="image" src="https://github.com/user-attachments/assets/a05d3572-7c54-48c1-ae86-45b3a546e846" />
<img width="357" height="268" alt="image" src="https://github.com/user-attachments/assets/3e6b3958-9d32-4506-9baf-644f106e9207" />

- 로비화면
  *  참가한 플레이어 이름과 월드 랭크 점수가 같이 표기된다.
  *  플레이어가 나가고 들어오는 것을 계속 업데이트한다.
- 결과화면
  * 게임이 종료되고 최종 순위와 최종 점수 그리고 그에 따라 변동된 월드 랭킹 점수도 같이 표기된다.
    * 월드 랭킹 점수는 등수와 함께 플레이한 플레이어 수에 따라 차등 집계된다.
- 실제 구현 코드: 
 [???](https://github.com/2023gamedev/project/blob/SW/Server/Game%20Server/Server/Task.h)

### 🖧 Photon 활용
- ‘LobbyManager’라는 게임 오브젝트와 스크립트를 만들어 연결시켜 로비를 생성 또는 검색하는 기능 구현
- 로비에서 마스터 서버와의 연결 상태를 Text를 이용하여 화면에 표시
- ‘PlayerManager’라는 게임 오브젝트를 만들어 그 안에 RPC 동기화용 스크립트 ‘PlayerManager.cs’ 과 Photon View 컴포넌트를 붙여 네트워크 작업을 수행
- Photon View 의 Synchronization 옵션을 Off 로 설정하여 RPC만을 이용하여 동기화 작업을 처리함
- 대부분의 PunRPC의 사용 방법은 모든 클라이언트에게 원하는 RPC 함수를 실행시키고 인자로 특정 플레이어의 ActorNumber를 넘겨주어 해당 클라이언트만 해당 함수를 실제로 작동시키도록 구현
<img width="178" height="124" alt="image" src="https://github.com/user-attachments/assets/137a1b3a-7b8f-476b-82ed-6acf6d36089e" />
← 인 게임에서 실시간 점수에 따라 점수 판의 순위가 변동됨

- 실제 구현 코드: 
 [???](https://github.com/2023gamedev/project/blob/SW/Server/Game%20Server/Server/Task.h)

### 🐹 두더지 패턴
- 두더지의 패턴은 3가지
  * Clean Up: 두더지가 한번에 튀어나온다.
  * Peeking and Up: 두더지가 머리를 반쯤 내밀고 한번 주춤하고 튀어나온다.
  * Peeking and Down: 두더지가 머리를 반쯤 내밀고 한번 주춤하고 다시 들어간다.
- 게임시간이 지날 수록 두더지의 속도가 빨라진다.
- 패턴 저장 방식
  * 딕셔너리 리스트 형식으로 생성 → List<Dict<int, int>> 형식
  * 예) 
    1번 두더지: [[10:1]-[15:3]-[19:2]-[23:1]-[38:1]-[51:2]-[68:3]-[79:1]-[95:3]-[111:2]] → 여기서 [10:1]은 [튀어나오는 시간:패턴 번호]이다.
- 두더지 패턴 동기화
  * 두더지 패턴 동기화는 랜덤함수에 동일한 '시드 값'을 가지도록 해당 값을 RPC를 이용하여 넘겨줌.
  * 또한, 게임 시작은 해당 시드 값을 모든 플레이어가 받고 나서 준비가 완료되면 같이 시작함.
- 실제 구현 코드: 
 [???](https://github.com/2023gamedev/project/blob/SW/Server/Game%20Server/Server/Task.h)
