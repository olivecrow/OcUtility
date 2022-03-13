## README.md

이건 유니티로 게임 제작을 할때 매번 작성하게 되는 유틸리티 및 C#확장을 미리 정의해놓은 커스텀 패키지임.

## OcUtility v1.4.8

### 작성된 유니티 버전 - 2021.2.15
### 파이프라인 버전 - URP 12.1.5

### Dependencies

[UMP]
- 에디터 코루틴 / com.unity.editorcoroutines

[Custom Package]
- 오딘 인스펙터 / Odin Inspector

## 설치방법

"com.olivecrow.ocutility": "https://github.com/olivecrow/OcUtility.git"

을 Packages/manifest.json에 붙여넣은 후, 계정에 액세스 하기.

후자의 버전 넘버의 경우, 릴리즈에 맞는 넘버링을 넣으면 됨.


## OcPool 사용 방법
- 프리팹 혹은 소스가 될 오브젝트를 OcPool&lt;T&gt;.MakePool(source, 00) 메서드를 통해 풀 생성.
- 이후 OcPool&lt;T&gt;.Call(source, in position, in rotation)으로 호출.
- 혹은 MakePool에서 반환되는 풀을 캐싱한 후, 풀에서 직접 pool.Call(in position, in rotation)을 호출.

- 풀 오브젝트는 PoolMember&lt;T&gt;혹은 IPoolMember&lt;T&gt;를 상속받아서 WakeUp과 Sleep으로 기능해야함.
- ex1) <code>public class MyClass : PoolMember&lt;MyClass&gt;</code>
- ex2) <code>public class MyClass : MonoBehaviour, IPoolMember&lt;MyClass&gt;</code>
