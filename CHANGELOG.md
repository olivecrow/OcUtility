# Changelog

## [1.4.7] -2022-02-23
### Added
- OcPool.Call 메서드에 WakeUp 직전에 초기화용으로 실행할 수 있는 beforeWakeUp 콜백 추가
- MathExtension
  - 벡터의 멤버를 수정하는 NewXZ, NewXY, NewYZ 메서드 추가
  - float, int, double의 제곱을 반환하는 sqr메서드 추가

### Fixed
- SimpleEventTrigger에서 MultipleEvent 레이아웃 개선

## [1.4.6] -2022-02-08
### Fixed
- OcPool Dispose 관련 오류 해결
- 오브젝트 배치 유틸리티 관련 수정
  - 씬의 핸들이 사라지지 않던 문제 해결
  - rotation이 Same으로 지정됐을때, 화살표로 현재 바라보는 방향을 알 수 있도록 함
  - Shape가 변경됐을때, 오브젝트의 중심점에 start 지점과 center 지점이 위치하도록 변경함
  - 씬 오브젝트가 아니면 작동하지 않도록 변경함

## [1.4.5] -2022-02-08
### Added
- Printer.DrawCross 기능 추가
- Printer.DrawDonut에 duration 옵션 추가. 플레이모드에서만 적용됨.

### Fixed
- OcPool 초기화 오류 해결

## [1.4.4] -2022-02-07
### Added
- MathExtension 기능 추가
  - 길이를 지정해서 사용하는 Foreach<T>메서드 추가
  - float, int, double, vector의 절댓값을 반환하는 abs 메서드 추가
  - float, int, double의 부호를 반환하는 Sign 메서드 추가
  - CapsuleCollider의 위, 아래 구체의 중심과 반지름을 반환하는 ToWorldSpaceCapsule 추가
  - CapsuleCollider의 축 방향을 반환하는 DirectionAxis 추가

- EditorComment 기능 추가
  - EditorComment의 내용을 클립보드에 복사할 수 있는 기능 추가
  - Editor Comment Window에서 에셋 생성, 삭제하는 기능 추가

- SimpleEventTrigger에 유니티 이벤트 타이밍에 따라 호출할 수 있는 기능 추가
- 오브젝트 배치 유틸리티에 핸들 추가

### Fixed
- Editor Comment에서 에셋을 생성할때, 게임오브젝트의 씬 이름을 폴더로 생성하도록 변경
- 오브젝트 배치 유틸리티에서 Arc타입 삭제

## [1.4.3] -2022-01-16
### Added
- 소수점 자리수를 지정하여 반올림하는 Round메서드를 Vector에서도 사용 가능하도록 확장함
- Bound 내의 임의의 한 점을 반환하는 Bounds의 확장인 GetRandomPosition 추가

### Fixed
- GUIDrawer 및 Printer의 Label 메서드를 완전히 삭제함.

## [1.4.2] -2022-01-15
### Added
- Editor Comment Asset 및 Editor Comment Window 추가
  - EditorComment를 에셋으로 관리해서, 여러 씬에서도 편집할 수 있게 함.
  - 기존의 코멘트들은 지워지기때문에 업데이트 하기 전에 백업해놓던가 해야함.
  - 기존처럼 게임오브젝트에 EditorComment 스크립트를 부착해서 쓸 수도 있음.
- OcPool에 파라미터가 없는 Call 메서드 추가
- GetMaxIndex, GetMaxElement 등 일부 메서드에 대해서 길이를 지정해서 넣을 수 있도록 시그니처를 추가함.
  - RaycastNonAlloc등과 같이 배열의 길이를 반환하는 메서드와 같이 사용할때 유용함.

### Fixed
- OcPool에서 폴더 게임오브젝트 이름 앞에 타입명을 붙이도록 변경
- PoolMember.Sleep에서 Pool에 null 체크를 추가함.

## [1.4.1] -2022-01-08
### Added
- MathExtension에 IEnumerable<T> 확장 메서드 추가
  - 멤버의 인덱스를 알 수 있는 IndexOf(T), IndexOf(Predicate<T>)
- 로그를 클리어 할 수 있는 단축키 추가
  - Printer.ClearLogs
  - Ctrl + Alt + [
- 게임 중 OcUtility의 단축키 사용을 위한 ShortcutListener추가
  - Printer.Divider 추가.
  - Printer.ClearLogs추가.
- 직렬화 순간에도 호출 가능한 무작위 색상인 ColorExtension.SystemRandom 메서드 추가
- 캡슐 콜라이더의 위, 아래 구체의 중심 위치를 구할 수 있는 메서드 추가
  - GetWorldTopHemiPoint, GetWorldBottmHemiPoint
### Fixed
- OcPool에서 초기화 시점의 풀 개수를 최적화함
- Printer.Divider의 단축키를 Shift에서 Alt로 변경
- Printer.DrawDonut을 쉽게 쓸 수 있도록 시그니처 여러개 추가.
- HierarchyIconDrawer에서 GUI색상이 잘못 출력되던 오류 수정

## [1.4.0] -2022-01-03
### Added 
- OcPool에 유틸리티 메서드 추가
  - FindMaxMember / FindMinMember : 풀 멤버의 필드, 혹은 계산식에서 최소, 최댓값을 가지는 멤버를 반환함
  - Sleeping, Active 멤버들만 구하는 바리에이션도 있음
  - 각 멤버를 초기화할때 적용할 수 있는 intializer 델리게이트

- 콘솔 로그에 구분자 출력을 위한 메서드와 단축키 추가
  - Shift + [  => 콘솔창에 ==========0========== 출력 / 가운데 0은 1씩 늘어나는 카운트
  - Shift + ]  => 위와 같은 로그를 랜덤 색상으로 출력 

- ColorExtension에 디버그용 리치텍스트를 위한 string.DRT 메서드 추가
- ColorExtension에 각 채널의 합을 구하는 SumRGB, SumRGBA 메서드 추가
- -1, 혹은 1을 50% 확률로 곱하는 RandomSign메서드 추가 (float, int 확장)

### Fixed
- EditorComment를 플레이 중에도 편집 가능하게 함

## [1.3.2] -2021-12-18
### Added
- OcPool의 모든 활성화 멤버를 Sleep상태로 전환하는 SleepAll 메서드 추가

### Fixed
- OcPool의 Foreach 방식 변경
  - 기존 방식은 활성 리스트와 비활성 큐에서 각각 반복문을 돌렸음
  - 하지만 그렇게 하면 Foreach도중에 실행하는 WakeUp, Sleep에서 멤버 변경이 있기때문에 의도치 않은 결과가 발생했음
  - 그래서 모든 멤버를 임시 리스트에 집어넣고, 그 리스트에서 작업을 실행하도록 변경함
- OcPool의 Dispose방식 변경
  - 기존에는 GlobalPool과 PoolDisposer에서 캐싱된 풀을 제거하지 않아서 디버그용 로그 출력시 오류가 발생했음
- PoolDisposer를 internal로 변경

## [1.3.1] -2021-12-15
### Fixed
- OcPool의 생성자를 private로 변경
- OcPool.MakePool의 Transform 파라미터를 통합
- PoolDisposer 초기화 방식 변경
- OcPool 디버그 기능 추가
  - Utility/OcPool 에서 현재 생성된 오브젝트 풀에 대한 로그를 출력할 수 있음

## [1.3.0] -2021-12-15
### Added
- IPool<T>, IPoolMember<T> 인터페이스 추가
- OcPool에 멤버 관리 메서드 추가
  - 활성 상태의 멤버를 찾을 수 있는 FindActiveMember 메서드 추가
  - 비활성 상태의 멤버를 찾을 수 있는 FindSleepingMember 메서드 추가
    - 비활성 상태의 멤버는 큐에 저장되어있어서, 내부적으로 배열로 변환한 후 검사하기 때문에 성능상의 이유로 매 프레임 호출하지 않는 것이 좋음
  - 모든 멤버에 대해 특정 작업의 반복실행이 가능한 Foreach 메서드 추가
- OcPool을 생성할때 폴더 트랜스폼을 미리 할당할 수 있게 함
  - UI를 위한 풀을 만들때 해당 트랜스폼의 자식으로 하는 등의 방식으로 사용하면 됨.
- 모든 풀을 한 번에 리셋할 수 있는 PoolDisposer 클래스 추가

### Fixed
- OcPool이 IPool<T>, PoolMember가 IPoolMember<T>를 구현하도록 변경
  - OcPool을 사용할 땐 OcPool<T>.MakePool() 과 같이 사용
  - PoolMember를 사용할 땐, MyClass : PoolMember<MyClass>와 같이 상속받아서 사용

## [1.2.0] - 2021-12-14
### Fixed
- HierarchyIconDrawer가 2020 이하버전과 호환되지 않던 문제 해결
- 버전 넘버링 걍 올림

## [1.1.26] - 2021-12-14
### Added
- MathExtension에서 열거형 타입의 변수를 더하거나 곱하는 Sum, Multiply 확장 메서드 추가

### Fixed
- HierarchyIconDrawer 최적화
  - IHierarchyIconDrawable에서 DistanceToText, IconPath 프로퍼티 삭제
  - 아이콘을 오버라이드 하지 않으면, 기본적으로 사용하고 있는 스크립트의 아이콘을 사용함
  - 아이콘에서 텍스트로의 거리가 자동으로 지정되도록 변경
- 네이밍 유틸리티에서 insert할때 메인 에셋인 경우, 이름이 두 번 추가되던 문제 해결
- GUIDrawer의 OnDrawGizmos삭제

## [1.1.25] - 2021-12-08
### Fixed
- MathExtension의 Remap에서 beforeRange의 범위가 0일 경우(분모가 0으로 나눠질 경우) tagetRangeMin값을 반환하도록 변경


## [1.1.24] - 2021-12-07
### Fixed
- Printer.DrawDonut에서 도넛 간에 ZTest를 활성화함
- GUIDrawer에 hideflag 설정이 안 되어있던 문제 해결

## [1.1.23] - 2021-12-07
### Added
- Printer에 DrawDonut메서드 추가.
  - 각도와 거리 범위를 도넛 형태의 기즈모로 표현 가능

### Fixed
- Printer.WorldGUI를 Obsolete표기하고, Printer.Label을 새로 만듬
  - 기존 GUI.Label 및 Handles.Label과 시그니처를 비슷하게 함

## [1.1.22] - 2021-12-04
### Fixed
- GUIDrawer때문에 플레이모드 진입 시, 다른 프로그램을 유니티 위로 띄우지 못하던 버그 수정
- Printer.WorldGUI 메서드 시그니처를 1개로 줄임
  - Color 인수를 없애고 RichText로만 작동하게 함
- ColorExtension의 ToRichText를 Obsolete 표기하고 같은 기능의 Rich 메서드를 추가함
  - 길이가 길어서 걍 줄인거.

## [1.1.21] - 2021-12-04
### Added
- Printer에 월드 위치 GUI를 표시할 수 있는 WorldGUI 메서드 추가
  - GUIDrawer라는 오브젝트를 플레이 모드 진입시 생성함
  - 빌드, 에디터 모두에서 사용 가능
  - Development 빌드가 아닌 Release 빌드에선 자동으로 해제됨
  - SceneView에서도 같은 gui를 볼 수 있으나, 기본적으로 비활성화 되어있고, 위치가 조금 달라짐
- OcDictionary에 Sort, Clear 기능 추가

### Fixed
- ObjectPlaceUtility에서 Circle 정렬 시, Center 위치를 지정할 수 있도록 수정함

## [1.1.20] - 2021-12-03
### Added
- 거리 계산에 마지막 포인트와 씬 뷰 카메라까지의 거리를 표시함
- ObjectPlaceUtility에서 루트 오브젝트를 하위 오브젝트들의 중간 위치로 옮기는 메서드를 추가함
  - Hierarchy에서 루트 오브젝트를 우클릭하여 사용할 수 있음
  - Undo 가능
- OcPool에서, 생성됐던 풀이 파괴되어 참조가 사라진 경우 풀을 재생성하는 과정을 추가함

## [1.1.19] - 2021-11-25
### Added
- OcDictionary에 Count 프로퍼티 추가
- Printer에 Ray 및 Line 메서드 추가

## [1.1.18] - 2021-11-25
### Fixed
- 네이밍 유틸리티의 C# 호환성 문제 수정.

## [1.1.17] - 2021-11-25

### Added
- 네이밍 유틸리티에 클론 넘버 없애는 기능 추가.
- OcDictionary에 Remove 및 RemoveAll 추가.

### Fixed
- 네이밍 유틸리티 Undo 가능하도록 수정.


## [1.1.16] - 2021-09-24
### Added
- SceneViewController에 RaycastHit을 최대 50개까지 반환할 수 있는 RaycastSceneViewAll 메서드 추가.
- MathExtension에서 여러 타입의 확장 지원 및 기능 추가
  - VectorInt확장 추가
  - Reman, IsInRange의 int, double 확장 추가
  - Vector 확장 (Sum, SelfMultiply) 추가
- OcDictionary를 일반 Dictionary로 변환해주는 ToDictionary메서드 추가.

### Fixed
- DistanceCalulator에서 포인트 설정시, 제일 높은 표면을 가리키도록 변경
- OcDictionary에서 Value가 UnityEngine.Object인 경우, 해당 객체의 프리뷰로 렌더링함.

## [1.1.15] - 2021-09-08
### Added
- 값이 변경될 때마다 이벤트를 호출하는 AlarmVar<T> 추가.

### Fixed
- ObjectPlaceUtility에 작업할 때만 위치를 업데이트 할 수 있다록 isActive 변수 추가

## [1.1.14] - 2021-08-29
### Added
- 거리 계산을 해주는 DistanceCalculator 추가.
  - Hierarchy창에서 오른쪽 클릭 후, 거리 계산을 눌러서 실행함.
- SceneView카메라와 월드의 특정 위치까지의 거리를 반환하는 DistanceFromSceneViewCam 메서드를  
SceneViewController에 추가.

## [1.1.13] - 2021-08-27
### Added
- SceneViewController에 마우스 포인터 위치에 레이캐스트를 하는 RaycastSceneView 메서드 추가
- 여러 오브젝트의 월드 위치 정렬을 해주는 ObjectPlaceUtility 추가

## [1.1.12] - 2021-08-27
### Added
- 네이밍 유틸리티에 Insert기능 추가

### Fixed
- IHierarchyIconDrawer 빌드 안되는 오류 해결
- 네이밍 유틸리티에서 에셋 이름까지 변경되도록 변경


## [1.1.11] - 2021-08-04
### Fixed
- PoolMember의 WakeUp과 Sleep을 virtual로 변경
- ReadMe에 OcPool에 대한 설명 추가.

## [1.1.10] - 2021-07-31
### Added
- ColorExtension에 시드값으로 무작위 색상을 출력하는 Color(int) 메서드 추가
- MathExtension에 Remap 각 종류별로 float만 사용하는 범위 지정 추가.

## [1.1.9] - 2021-07-30
### Fixed
- HierarcyIconDrawer를 Hierarchy창의 왼쪽을 기준으로 하도록 변경.
  - IHierarchyIconDrawable의 주석 변경
  - EditorComment의 아이콘 거리 변경
  
### Added
- MathExtension에 float의 자리수 반올림 기능인 Round(int decimal) 추가.
- MathExtension에 10의 거듭제곱을 곧바로 반환하는 Pow10(int pow) 추가
- ColorExtension에 색상 반전 기능인 Invert() 추가.
- NamingUtility 추가.

## [1.1.8] - 2021-07-28
### Added
- wait 클래스에 fixedFrame 추가

### Fixed
- MonoHierarchyIconDrawer에서 텍스쳐가 있을때 아이콘 패스 필드가 드러나던 문제 해결

## [1.1.7] - 2021-07-24

### Fixed
- ColorExtension의 Ranbow()메서드의 파라미터를 색상 변화 속도로 바꿈. 알파값은 1로 고정.
- EditorComment에서 OnGizmosSelected에서 무지개색 구체를 그리도록 변경.

### Added
- MathExtension에 IsInRange 메서드를 float으로만 이루어진 시그니처 추가.
- HierarchyIconDrawer에 Texture를 직접 할당해서 아이콘을 지정할 수 있는 기능 추가.
- ColorExtension에 Random 프로퍼티 추가. 임의의 한 색상이 풀력됨.

## [1.1.6] - 2021-07-20
### Added
- 디버그 로그용 클래스 및 매서드 Printer.Print() 추가.
- ColorExtension에 Darken 및 Gray 추가.

## [1.1.5] - 2021-07-16
### Added
- 간단한 SerializableDictionary 만듬 (OcDictionary)
  - 활용성이 얼마나 있을지는 모르겠다.
  
- 빌드 오류 없도록 Editor asmdef 및 일부 전처리기 수정함.

## [1.1.4] - 2021-07-01

### Added
- MathExtention에 각종 수식 확장 및 열거형 확장 추가.
  - Approximately, IsApproximately
  - GetMinElementIndex, GetMinElement
  - GetMaxElementIndex, GetMaxElement

## [1.1.3] - 2021-07-08

### Added
- ColorExtension에 스트링에서 곧바로 리치 텍스트 컬러를 적용하는 ToRichText 메서드 추가
- MathExtention에 2차원 평면을 3차원의 수평 평면으로 전환하는 (XY -> XZ) ToXZ 메서드 추가

## [1.1.2] - 2021-06-29

### Fixed
- HierarchyIconDrawer가 여러개의 아이콘을 그릴 수 있도록 변경.
- HierarchyIconDrawer에서 아이콘 변경점을 즉시 반영하도록 변경.


## [1.1.1] - 2021-06-27

### Fixed
- HierarchyIconDrawer 내부 리스트에 아이콘이 없는 오브젝트에서 null로 렌더링 되던것을 리스트에 미포함하는 것으로 변경.
- OcPool이 Enter PlayMode에서 오류가 나던 것을 수정.

### Added
- HierarchyIconDrawer에 아이콘 색조 기능 추가.
- 단일 컴포넌트로 사용 가능한 MonoHierarchyIconDrawer 추가.
  
## [1.1.0] - 2021-06-27

### Added
- 간단한 오브젝트 풀 기능 추가 
  - pool = OcPool.
  - member = PoolMember


## [1.0.2] - 2021-06-21

### Added
- ColorExtension에서 무지개 효과를 내는 Rainbow() 매서드 추가.

### Fixed
- asmdef 이름 변경
- EditorComment 선택시, WireCube와 WireSphere가 합쳐진 모습의 기즈모가 그리는 것으로 변경.

## [1.0.1] - 2021-06-14
### Fixed
- README 설치방법 수정
- EditorComment 아이콘이 나오지 않던 문제 해결

## [1.0.0] - 2021-06-13
### Added
- 에셋 임포트 포스트프로세스
- Hierarchy Icon
- 숫자패드 씬 뷰 컨트롤러
- 에디터 코멘트 스크립트
- 심플 이벤트 트리거
- Color 구조체 확장 (각 채널 값 변경)
- 수학 확장 (Vector 각 채널 값 변경, 범위체크, 리맵)
- asmdef 포함