# 실습1: Simple ToDo-List

# 1. 앱 기획

다음의 기능을 포함한다:

- 사용자가 할 일을 **입력**
    - **추가** 버튼을 누르면 리스트에 표시
- 사용자가 리스트에 있는 할 일을 삭제 버튼을 통해 **삭제**

# 2. UI 설계 (XAML)

## XAML 코드 전문

```xml
<?xml version="1.0" encoding="utf-8" ?>
    
<ContentPage xmlns="http://schemas.microsoft.com/dotnet/2021/maui"
             xmlns:x="http://schemas.microsoft.com/winfx/2009/xaml"
             xmlns:local="clr-namespace:Exercise1_ToDoList"
             x:Class="Exercise1_ToDoList.MainPage">

    <ContentPage.BindingContext>
        <local:MainViewModel />
    </ContentPage.BindingContext>

    <VerticalStackLayout>
        <!-- 할 일 입력-->
        <Entry x:Name="ToDoEntry" Placeholder="할 일을 입력하세요" Text="{Binding NewToDo}" />
        
        <!-- 할 일 추가 버튼-->
        <Button Text="추가" Command="{Binding AddToDoCommand}" />

        <ListView ItemsSource="{Binding ToDoList}" SelectionMode="None" >
            <ListView.ItemTemplate>
                <DataTemplate>
                    <ViewCell>
                        <StackLayout Orientation="Horizontal" Padding="5">
                            
                            <Label Text="{Binding}"
                                   VerticalOptions="Center" />
                            
                            <Button Text="삭제" 
                                    Command="{ Binding Path=BindingContext.RemoveToDoCommand,
                                                       Source={x:Reference Name=ToDoEntry} }"
                                    CommandParameter="{Binding}" />
                            
                        </StackLayout>
                    </ViewCell>
                </DataTemplate>
            </ListView.ItemTemplate>
        </ListView>
    </VerticalStackLayout>

</ContentPage>

```

## 코드 분석1: `xmlns`와 관련하여

## 1. `xmlns`의 의미

`xmlns`는 **XML 네임스페이스(namespace)** 를 선언하는 것이다.

XAML은 XML 기반이기 때문에, 각 요소가 어느 라이브러리/기능의 것인지 알려줘야 하며, 그 역할을 하는 게 `xmlns`이다.

### 예제:

```xml
xmlns="http://schemas.microsoft.com/dotnet/2021/maui"
```

### 해석(의미):

> "기본 태그(`<ContentPage>`, `<Label>`, `<Button>` 등)는 모두 MAUI의 UI 요소다."
> 

즉, `<Button>`을 사용하면 MAUI의 `Microsoft.Maui.Controls.Button`이라는 클래스를 연결하겠다는 뜻이다.

### `xmlns="..."` 문자열의 정체

이 URL은 **MAUI 프레임워크의 고유 식별자**일 뿐, 웹사이트처럼 들어갈 수 있는 URL은 아니다(접속해도 `404 Not Found`일 수 있음).

이 문자열은 Microsoft가 `.NET MAUI XAML`을 위한 **표준 네임스페이스 주소**로 정해둔 것이고, 컴파일러가 이걸 보고 "아, 이건 MAUI 기본 컨트롤이구나!" 하고 이해하는 것이다.

---

## 2. `xmlns:x`에서 `:x`의 정확한 개념과 쓰임

### 의미:

`xmlns:x="..."`는 `x:`로 시작하는 특수 XAML 문법(키워드들)을 정의하기 위한 네임스페이스다.

### 예제:

```xml
xmlns:x="http://schemas.microsoft.com/winfx/2009/xaml"
```

이 줄의 의미는 다음과 같다:

> "`x`로 시작하는 모든 키워드는 XAML 시스템에서 미리 정의된 특수 기능들을 사용한다."
> 

### 대표적인 `x:` 키워드들:

| 예시 | 설명 |
| --- | --- |
| `x:Class` | 이 XAML이 연결된 C# 클래스의 이름 |
| `x:Name` | C#에서 접근 가능한 변수 이름 지정 |
| `x:Key` | 리소스 딕셔너리에서 키로 사용 |
| `x:TypeArguments` | 제네릭 타입 정의할 때 사용 |

### `xmlns:x=""` 문자열의 정체:

```xml
http://schemas.microsoft.com/winfx/2009/xaml
```

이것은 .NET에서 **XAML 키워드용 네임스페이스** 로 고정된 주소이며, XAML 기능들을 쓸 때 항상 이걸 사용한다.

---

## 3. `xmlns:local`에서 `:local`의 정확한 개념과 쓰임

### 의미:

`local:`은 **내가 만든 C# 클래스나 뷰모델, 리소스 등을 XAML에서 쓰기 위한 접두어**다.

```xml
xmlns:local="clr-namespace:Exercise1_ToDoList"
```

이렇게 선언하면, C# 코드의 `Exercise1_ToDoList` 네임스페이스 안에 있는 클래스들을
`<local:클래스명 />` 형태로 쓸 수 있게 된다.

### 예제:

```xml
<local:MainViewModel />
```

### `xmlns:local="..."` 문자열의 정체:

```xml
clr-namespace:Exercise1_ToDoList
```

이건 CLR(Common Language Runtime) 기반의 **C# 네임스페이스를 참조하겠다**는 의미이며, 즉, C#에서 만든 클래스를 XAML에 연결해주기 위한 문법이야.

---

## 4. `x:Class`에서 `x`와 `Class`의 개념

### 의미:

```xml
x:Class="Exercise1_ToDoList.MainPage"
```

이건 **XAML과 연결된 C# 코드 비하인드(.xaml.cs) 파일이 어느 클래스인지를 지정하는 것**이야.

> 즉, 이 `XAML`은 `Exercise1_ToDoList.MainPage` 클래스의 UI야.
> 

그래서 C# 쪽에 보면 이렇게 되어 있어:

```csharp
public partial class MainPage : ContentPage
```

그리고 생성자에서 `InitializeComponent()`를 호출하며, 바로 이 `x:Class` 덕분에 **XAML과 C#이 연결**된다.

### `x:Class="..."` 문자열의 정체:

```xml
x:Class = "Exercise1_ToDoList.MainPage"  <!-- "nameSpcae.class" -->
```

문자열로 처리되어 있는 텍스트는 C#의 **클래스 전체 이름 (namespace + class)**이다.

---

## 5. 필수적인가?

XAML의 루트 요소인 `<ContentPage>` 안에 있는 `xmlns`, `x:Class` 등은 **XAML → C# 연결**, **XAML 문법 기능 사용**, **사용자 정의 클래스 연결**에 **필수적인 설정**이다.

따라서 *각각 없어질 경우 어떤 문제가 생기는지* 정리하면 다음과 같다:

---

1. `xmlns="http://schemas.microsoft.com/dotnet/2021/maui"`
    
    
    | ❌ 없으면? | **기본 UI 태그**(`<Label>`, `<Button>`, `<ContentPage>`)를 **인식 못 함** |
    | --- | --- |
    | 이유 | 어떤 UI 요소인지 해석할 네임스페이스가 없어져서 컴파일 에러 발생 |

---

1. `xmlns:x="http://schemas.microsoft.com/winfx/2009/xaml"`
    
    
    | ❌ 없으면? | `x:Class`, `x:Name` 같은 **키워드 사용 시 전부 에러 발생** |
    | --- | --- |
    | 이유 | `x:` 접두어를 어떤 네임스페이스로 해석할지 몰라서 XAML 파서가 죽음 |

---

1. `xmlns:local="clr-namespace:Exercise1_ToDoList"`
    
    
    | ❌ 없으면? | `<local:MainViewModel />` 같은 **사용자 정의 클래스 태그를 못 씀** |
    | --- | --- |
    | 이유 | `local:` 접두어가 어떤 C# 네임스페이스를 가리키는지 모르게 됨 |

---

1. `x:Class="Exercise1_ToDoList.MainPage"`
    
    
    | ❌ 없으면? | `MainPage.xaml.cs`의 C# 코드와 이 XAML이 **연결되지 않음** |
    | --- | --- |
    | 이유 | 컴파일러가 어떤 partial class에 이 XAML UI 코드를 붙여야 할지 몰라서 `InitializeComponent()` 호출도 실패 |

### **결론:**

이 **네 가지는 모두 필수**이며, 없으면 **작동 자체가 안 되는 경우가 대부분**이다.

---

## 정리 및 학습

## 요약 정리표

| 구문 | 의미 |
| --- | --- |
| `xmlns="..."` | 기본 컨트롤 태그들의 네임스페이스 |
| `xmlns:x="..."` | XAML 내부 키워드(`x:Name`, `x:Class` 등) 정의 |
| `xmlns:local="clr-namespace:..."` | 내가 만든 C# 클래스들을 XAML에서 사용할 때 |
| `x:Class="네임스페이스.클래스"` | 이 XAML과 연결된 C# 클래스 지정 (UI 코드 연결) |

---

## 기억하기 팁

- `xmlns:` → 어떤 **라이브러리/코드를 불러올지** 정한다.
- `x:` → XAML만의 **키워드 기능**을 쓰기 위한 예약어.
- `local:` → **내 프로젝트 안에 만든 클래스**를 XAML에서 사용하려고 붙이는 것.
- `x:Class` → **XAML과 C# 코드 연결고리**

## 코드 분석2: `<ContentPage.BindingContext>`와 관련하여

```xml
<ContentPage.BindingContext>
    <local:MainViewModel />
</ContentPage.BindingContext>
```

### 역할:

이건 **XAML 페이지에서 사용할 ViewModel 객체를 연결**해주는 코드이다.

- XAML에서 `{Binding NewToDo}` 같은 **데이터 바인딩 구문**이 나오면,
- 이 데이터를 어디서 가져올지(어떤 클래스에서 가져올지)를 **BindingContext**가 알려준다.

즉, 어떤 XAML 구문에서 바인딩을 하고자 할 때, 특정 지은 클래스에서 정의된 멤버(변수, 프로퍼티, 메소드 등)을 가져오는 맥락(Context)를 지정하는 것.

따라서 이름도 **Binding Context(바인딩을 위한 컨텍스트)**이다.

위 코드를 기반으로 쉽게 말하자면, **“바인딩할 때의 대상은 MainViewModel 클래스임을 명시”**해주는 것이다.

### 해당 코드가 없다면?

XAML은 `{Binding ...}`을 사용할 수 있지만, 그 값이 어느 클래스의 어떤 프로퍼티인지를 모르기 때문에 `BindingContext`가 없으면 **연결 대상이 없어 오류가 나거나 바인딩이 실패**한다.

### 없으면 어떻게 될까?

- 버튼 눌러도 아무 동작 없음
- Entry에 값 입력해도 리스트에 안 들어감
- `{Binding ...}`이 다 **무시되거나 Null** 처리됨
- 컴파일은 될 수도 있지만 **기능이 안 됨** (정상 동작 안 함)

## 코드 분석3: UI 구조

### 전체 구조

**태그:**

- `<VerticalStackLayout>`
    - 여러 UI 요소를 **위에서 아래 방향으로 정렬**해주는 컨테이너 레이아웃.
    - 내부에 있는 `<Entry>`, `<Button>`, `<ListView>`는 **위→아래 순서대로 배치됨**.
    - MAUI의 `StackLayout`보다 최신이고, 퍼포먼스가 더 나음.

### 할 일 입력 영역

**코드:**

```xml
<Entry x:Name="ToDoEntry" Placeholder="할 일을 입력하세요" Text="{Binding NewToDo}" />
```

**태그:**

- `<Entry />`
    - 사용자로부터 **텍스트 입력을 받을 수 있는 필드 (텍스트 박스)**.
    - iOS에선 텍스트필드, 안드로이드에선 EditText와 비슷함.

**어트리뷰트:**

- `x:Name="ToDoEntry"`
    - C# 코드에서 이 Entry를 사용할 수 있게 **이름을 지정**함.
    - 이후 C# 코드나 바인딩에서 이 이름으로 **참조** 가능.
    - 나중에 `x:Reference`에서 이 이름이 활용됨.
- `Placeholder="할 일을 입력하세요"`
    - 텍스트 필드에 **입력 전 기본 안내 문구**를 보여줌.
- `Text="{Binding NewToDo}"`
    - 사용자가 입력한 텍스트가 **ViewModel의 `NewToDo` 속성**에 **바인딩**됨.
    - 즉, `NewToDo` ↔ Entry 간 **양방향 데이터 흐름**이 발생함.

### 할 일 추가 버튼

**코드:**

```xml
<Button Text="추가" Command="{Binding AddToDoCommand}" />
```

**태그:** 

- `<Button />`
    - 기본적인 클릭 가능한 버튼.
    

**어트리뷰트:**

- `Text="추가"`
    - 버튼에 표시될 글자.
- `Command="{Binding AddToDoCommand}"`
    - ViewModel에 있는 `AddToDoCommand`가 실행됨.
    - 버튼 클릭 → `AddToDoCommand.Execute()` 호출
    

> `AddToDoCommand`는 `ICommand` 타입이고, `NewToDo`를 `ToDoList`에 추가하는 로직이 들어 가 있음.
> 

### 할 일 리스트 표시

**코드:**

```xml
<ListView ItemsSource="{Binding ToDoList}" SelectionMode="None">
    ...
</ListView>
```

**태그:**

- `<ListView />`
    - 항목을 여러 개 표시해주는 **리스트 뷰**.
    - MAUI에서 흔히 사용되는 스크롤 가능한 리스트.
    

**어트리뷰트:**

- `ItemsSource="{Binding ToDoList}"`
    - ViewModel의 `ToDoList` (`ObservableCollection<string>` 타입)를 바인딩해서 항목으로 표시.
    - `ToDoList`에 항목이 추가되거나 삭제되면 UI가 자동으로 갱신됨.
- `SelectionMode="None"`
    - 리스트 항목을 **선택할 수 없도록 설정** (선택 강조 효과 없음)

### 리스트 아이템 템플릿

**코드:**

```xml
<ListView.ItemTemplate>
    <DataTemplate>
        <ViewCell>
            <StackLayout Orientation="Horizontal" Padding="5">
                ...
            </StackLayout>
        </ViewCell>
    </DataTemplate>
</ListView.ItemTemplate>
```

이 부분은 **리스트 뷰(ListView)의 각 항목(=하나의 할 일 데이터)을 어떤 UI 형태로 표시할지** 정의하는 곳이다.

**태그:**

- `<ListView.ItemTemplate>`
    - ListView의 각 항목이 **어떻게 보일지를 정의**하는 속성.
    - 내부에는 `DataTemplate`이 들어감.
- `<DataTemplate>`
    - 데이터 소스의 각 항목마다 **UI 구성 요소를 생성**해주는 구조.
    - 바인딩된 데이터(`ToDoList`의 각 문자열)에 대해 이 UI를 반복 적용함.
- `<ViewCell>`
    - ListView의 **개별 셀 단위 UI 컨테이너**.
    - 하나의 리스트 항목은 하나의 ViewCell로 구성됨.
- `<StackLayout Orientation="Horizontal" Padding="5">`
    - ViewCell 안의 UI 요소를 **가로로 정렬**해줌.
    - 각 할 일과 "삭제" 버튼이 **나란히 배치**됨.

**어트리뷰트:**

- `Orientation="Horizontal"`
    - StackLayout이 **수평 방향으로 자식 요소를 배치**함.
    - 기본값은 Vertical이지만, 여기선 Label과 Button을 옆으로 배치하고 싶기 때문에 수평 설정.
- `Padding="5"`
    - StackLayout의 **안쪽 여백**을 5만큼 설정.
    - Label과 Button 주변에 살짝 여백을 줘서 보기 좋게 함.

### 리스트 항목 내부 구성 - 라벨

**코드:**

```xml
<Label Text="{Binding}" VerticalOptions="Center" />
```

태그:

- `<Label>`
    - **텍스트를 출력**해주는 UI 컴포넌트.
    - ListView의 각 항목(문자열)을 화면에 표시하는 역할.
    

어트리뷰트:

- `Text="{Binding}"`
    - 현재 리스트 항목(예: `"할 일 1"`, `"할 일 2"` 등)을 그대로 출력함.
    - `ToDoList`의 각 요소가 문자열이기 때문에, `{Binding}`만 써도 그 값을 사용함.
- `VerticalOptions="Center"`
    - Label이 포함된 StackLayout 안에서 **수직 방향 중앙 정렬**.
    - Button과 높이가 다를 경우 **정렬을 맞추기 위해 사용**.

### 리스트 항목 내부 구성 - 버튼

**코드:**

```xml
<Button Text="삭제" 
        Command="{ Binding Path=BindingContext.RemoveToDoCommand,
                           Source={x:Reference Name=ToDoEntry} }"
        CommandParameter="{Binding}" />
```

**태그:**

- `<Button>`
    - **사용자 클릭 동작**을 처리하는 버튼.
    - 여기서는 `삭제` 텍스트가 표시되고, 클릭 시 할 일을 삭제함.

**어트리뷰트:**

- `Text="삭제"`
    - 버튼에 표시되는 텍스트.
- `Command="{Binding ...}"`
    - 버튼 클릭 시 실행할 **명령(커맨드)을 ViewModel에서 바인딩**함.
    - 여기서는 `RemoveToDoCommand`를 실행.

**세부 분석:**

```xml
Command="{ Binding Path=BindingContext.RemoveToDoCommand,
                   Source={x:Reference Name=ToDoEntry} }"
                   
CommandParameter="{Binding}" 
```

- `Path=BindingContext.RemoveToDoCommand`
    - Entry(ToDoEntry)의 바인딩 컨텍스트를 기준으로 `RemoveToDoCommand` 속성을 찾음
    - 즉, **현재 페이지의 ViewModel에서 RemoveToDoCommand 찾기**
- `Source={x:Reference Name=ToDoEntry}`
    - **ToDoEntry를 기준으로 바인딩 소스를 지정**
    - ListView 내부의 Button은 기본적으로 자기 항목 하나만 바인딩 대상이기 때문에, 전체 ViewModel에 접근하기 위해 Entry를 "경유"해서 우회적으로 ViewModel 참조
- `CommandParameter="{Binding}"`
    - `ToDoList`의 현재 항목 값을 `RemoveToDoCommand`에 인자로 전달.
    - 예: "할 일 1"이라는 문자열이 명령 메서드의 파라미터로 넘어감.

# 3. 기능 구현 (C#)

## C# 코드 전문

```csharp
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;

namespace Exercise1_ToDoList
{
    public class MainViewModel /*: INotifyCollectionChanged*/
    {
        public ObservableCollection<string> ToDoList { get; set; } = new ObservableCollection<string>();

        private string _newToDo;
        public string NewToDo
        {
            get => _newToDo;
            set
            {
                _newToDo = value;
                OnPropertyChanged(nameof(NewToDo));
            }
        }

        public ICommand AddToDoCommand { get; }
        public ICommand RemoveToDoCommand { get; }

        public MainViewModel()
        {
            AddToDoCommand = new Command(AddToDo);
            RemoveToDoCommand = new Command<string>(RemoveToDo);
        }

        private void AddToDo()
        {
            if (!string.IsNullOrWhiteSpace(NewToDo))
            {
                ToDoList.Add(NewToDo);
                NewToDo = string.Empty;
            }
        }

        private void RemoveToDo(string item)
        {
            if (ToDoList.Contains(item))
            {
                ToDoList.Remove(item);
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}

```

## 코드 분석1: 클래스 선언부

코드:

```csharp
public class MainViewModel
```

- **ViewModel 역할**을 하는 클래스.
- 우리가 만든 UI(`MainPage.xaml`)의 `BindingContext`로 연결돼서 데이터와 커맨드를 제공하는 핵심 객체.

## 코드 분석2: 네임스페이스 및 인터페이스 관련

**네임스페이스:**

```csharp
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;
```

- `System.Collections.ObjectModel`
    - `ObservableCollection<T>`: 컬렉션 변경(추가/삭제 등)을 UI에 자동 반영할 수 있게 도와주는 클래스.
- `System.ComponentModel`
- `System.Windows.Input`

**인터페이스:**

- `INotifyPropertyChanged`: 속성이 변경될 때 UI에 알려주는 인터페이스.
- `ICommand`: 버튼 클릭 같은 UI 상호작용 이벤트를 ViewModel로 전달하는 기능을 위한 인터페이스.

<aside>
💡

**참고**

---

주석 처리된 `INotifyCollectionChanged`는 `ObservableCollection` 내부에 이미 구현되어 있기 때문에 따로 쓸 필요 없음.

</aside>

## 코드 분석3: 프로퍼티 - `ToDoList`

**코드:**

```csharp
public ObservableCollection<string> ToDoList { get; set; } = new ObservableCollection<string>();
```

- **역할**: 할 일 목록 데이터를 저장하는 리스트.
- `ObservableCollection`을 쓰는 이유는, 데이터가 추가되거나 삭제될 때 자동으로 UI(`<ListView>`)에 반영되게 하기 위함.
- 일반 `List<string>`을 쓰면 UI에 반영이 안 되기 때문에 MVVM에서 자주 쓰임.

## 코드 분석4: 프로퍼티 - `NewToDo` (입력값)

**코드:**

```csharp
private string _newToDo;
public string NewToDo
{
    get => _newToDo;
    set
    {
        _newToDo = value;
        OnPropertyChanged(nameof(NewToDo));
    }
}
```

- **역할**: 사용자가 입력창(Entry)에 적은 문자열을 저장.
- `Text="{Binding NewToDo}"`와 연결되어 있어서, Entry 입력이 ViewModel로 전달됨.
- `set`에서 `OnPropertyChanged`를 호출하여 UI에 변경 사항을 알림.

## 코드 분석5: 인터페이스 - `ICommand`

**코드:**

```csharp
public ICommand AddToDoCommand { get; }

```

- **UI 버튼**에서 `Command="{Binding AddToDoCommand}"`로 연결됨.
- `Command`는 UI 이벤트(버튼 클릭)를 ViewModel의 메서드로 전달하는 역할.
- 아래 생성자에서 연결함.

```csharp
public ICommand RemoveToDoCommand { get; }
```

- 리스트 항목의 "삭제" 버튼과 연결됨.
- 리스트의 각 항목을 삭제하는 기능 담당.

## 코드 분석6: 생성자 - 커맨드 초기화

**코드:**

```csharp
public MainViewModel()
{
    AddToDoCommand = new Command(AddToDo);
    RemoveToDoCommand = new Command<string>(RemoveToDo);
}
```

- MAUI의 `Command` 객체를 통해 UI와 ViewModel의 메서드를 연결함.
- `AddToDoCommand`는 파라미터 없이 `AddToDo()` 호출.
- `RemoveToDoCommand`는 리스트 항목(string) 하나를 파라미터로 받아서 `RemoveToDo(string)` 실행.

## 코드 분석7: 메서드 - `AddToDo()`, `RemoveToDo()`

**코드:**

```csharp
private void AddToDo()
{
    if (!string.IsNullOrWhiteSpace(NewToDo))
    {
        ToDoList.Add(NewToDo);
        NewToDo = string.Empty;
    }
}
```

- `NewToDo`에 값이 있을 경우 `ToDoList`에 추가.
- 추가 후, 입력창을 비워주는 효과를 위해 `NewToDo = ""` 수행.

**코드:**

```csharp
private void RemoveToDo(string item)
{
    if (ToDoList.Contains(item))
    {
        ToDoList.Remove(item);
    }
}
```

- 특정 문자열을 리스트에서 삭제.
- 리스트 내부에 해당 문자열이 있는 경우에만 삭제 수행.

## 코드 분석8: `INotifyPropertyChanged` 관련

**코드:**

```csharp
public event PropertyChangedEventHandler PropertyChanged;
protected void OnPropertyChanged(string propertyName)
{
    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
}
```

- UI 바인딩 대상 속성(`NewToDo`)이 변경되었음을 UI에 알려주는 메커니즘.
- MAUI에서 UI와 ViewModel을 동기화하려면 이 인터페이스가 꼭 필요.
- 이 이벤트가 없으면 ViewModel의 데이터 변경이 UI에 반영되지 않음

---