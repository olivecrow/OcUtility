# Changelog

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