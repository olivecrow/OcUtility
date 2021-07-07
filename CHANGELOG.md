# Changelog
All notable changes to this project will be documented in this file.

The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.0.0/),
and this project adheres to [Semantic Versioning](https://semver.org/spec/v2.0.0.html).

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