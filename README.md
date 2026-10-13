# 组态监控系统 · WPF 学习项目

> **状态：✅ 已完成** ｜ **定位：WPF 常用控件与核心机制的练习项目**

一个工业风「组态监控系统」界面，用于系统性地练习 WPF / XAML：

**布局 → 样式 → 控件模板 → 图形绘制 → 动画 → 自定义控件 → 依赖属性 → 数据绑定（MVVM）**

---

## 一、项目简介

| 项目 | 说明 |
|------|------|
| 名称 | 组态监控系统（Configration） |
| 类型 | WPF 桌面应用（**学习 / 练手项目**） |
| 技术栈 | .NET 10 + WPF + MVVM |
| 目标框架 | `net10.0-windows` |
| 第三方库 | `LiveCharts.Wpf` 0.9.7（图表） |
| 视觉风格 | 深色工业风：深藏青渐变背景 + 半透明网格纹理 + 青色 / 橙色点缀 |
| 开发环境 | Visual Studio 2026 Community |
| 学习目标 | 熟练常用控件，掌握 WPF 核心机制 |

**界面结构示意**：

```
┌───────────────────────────────────────────────────────────────────┐
│ [logo] 智慧工厂       设备介绍  系统设置       20:00:00   退出系统  │ ← 无边框标题栏
├─────────────┬─────────────────────────────────────────────────────┤
│ 设备配置详情 │  ┌──────────────── 工艺流程图 ────────────────┐     │
│  79 设备负载 │  │  [水箱]─[阀门]─[过滤器]─[搅拌罐]─[阀门]     │     │ ← Canvas
│  ▬▬▬▬▬▬     │  │    ●报警灯  ●报警灯  ●报警灯               │     │   自绘 + 组件
│  图标+指标×4 │  └────────────────────────────────────────────┘     │
│             │  ┌──── 设备运行数据统计 ────┐ ┌── 运行时信息 ──┐     │
│ 机械设备清单 │  │  折线图 │ 柱形图 │ 折线图 │ │ 报警消息…      │     │
│ 编号 时间 功率│  └──────────────────────────┘ └───────────────┘     │
│             │                                                      │
│ 设备处理状态 │                                                      │
└─────────────┴─────────────────────────────────────────────────────┘
   左侧 250px                        右侧自适应
```

---

## 二、项目结构

```
Configration/                        ← 仓库根目录
├── Configration.slnx                解决方案
├── .gitignore
├── README.md                        本文档
└── Configration/                    ← 项目本体
    ├── App.xaml / App.xaml.cs       应用入口 + 全局字体资源
    ├── AssemblyInfo.cs
    ├── Configration.csproj          项目文件（资源 / 包引用）
    │
    ├── Asserts/                     ⚠️ 注意拼写是 Asserts（不是 Assets）
    │   ├── Fonts/                   字体：DingTalk-JinBuTi.ttf、iconfont.ttf
    │   ├── Images/                  图片素材：logo、图标 a1~a4 / b1~b3 / d1~d4
    │   └── Styles/                  （预留）
    │
    ├── Components/                  ★ 自定义组件（UserControl）
    │   ├── Tank.xaml / .cs          储罐 —— 液位动画
    │   ├── Valve.xaml / .cs         阀门 —— 开关旋转
    │   ├── WarningLight.xaml / .cs  报警灯 —— 多状态切换
    │   ├── Can.xaml / .cs           罐体 —— 静态图形
    │   └── Feed.xaml / .cs          过滤器 —— 静态图形
    │
    ├── Models/                      数据模型
    │   ├── DeviceModel.cs           设备清单项
    │   ├── DeviceStateModel.cs      设备状态项
    │   └── MessageModel.cs          消息项
    │
    ├── ViewModels/
    │   └── MainViewModel.cs         主视图数据源（模拟数据）
    │
    └── Views/
        ├── MainView.xaml            主界面（约 1150 行）
        └── MainView.xaml.cs         设置 DataContext
```

---

## 三、界面结构与涉及技术

### 头部（第 0 行，高 40）

| 区块 | 技术 |
|------|------|
| 无边框窗体 | `WindowChrome` + `IsHitTestVisibleInChrome` |
| 深色渐变背景 | `LinearGradientBrush` |
| 网格纹理 | `VisualBrush` 平铺 |
| logo + 标题 | `Image` + `StackPanel` + 自定义字体 |
| 装饰弧线 | `Path`（自定义 `Data`）+ 渐变描边 |
| 菜单按钮 | `MenuButtonStyle`（`ControlTemplate` + `Trigger`） |
| 时间 / 退出 | `StackPanel` + `ExitButtonStyle` |

### 主体（第 1 行，分两列）

**左列（固定 250px）—— 三个 `GroupBox`，外层 `UniformGrid` 等分**

| GroupBox | 涉及技术 |
|----------|----------|
| 设备配置详情 | 大字号数值 + `ProgressBar` + `UniformGrid` 指标卡片（`Image` + 文字） |
| 机械设备清单 | 单选标签（`RadioButton`）+ 表头 `Grid` + `ItemsControl` 表格 |
| 设备处理状态统计 | `ItemsControl` + `ItemsPanel` + `DataTemplate`（图标 + 文字 + 进度条） |

**右列（自适应）**

| 区块 | 涉及技术 |
|------|----------|
| 工艺流程图 | `Canvas` + **5 个自定义组件** + `Path` 管道 + 控制按钮 |
| 设备运行数据统计 | `GroupBox` + **LiveCharts** 折线图 / 柱形图（共 4 个图表） |
| 运行时信息 | `ItemsControl` 绑定 `ObservableCollection` |

---

## 四、自定义组件（本项目重点）★

`Components/` 下的 5 个组件，是"用基础控件组合出可复用控件"的实践。

| 组件 | 依赖属性 | 类型 | 属性变化时的动作 |
|------|----------|------|------------------|
| **Tank** 储罐 | `Level` | `double` | 动画平移液体高度（`TranslateTransform.Y`） |
| **Valve** 阀门 | `IsOpen` | `bool` | 旋转阀芯 0° / 90°（`RotateTransform.Angle`） |
| **WarningLight** 报警灯 | `State` | `LightState` 枚举 | 切换红 / 黄 / 绿三个灯的 `Visibility` |
| **Can** 罐体 | — | — | 纯静态图形（无属性） |
| **Feed** 过滤器 | — | — | 纯静态图形（无属性） |

### 核心模式：依赖属性"三步走"

三个带状态的组件都是同一套写法：

```csharp
public partial class Valve : UserControl
{
    // ① 对外属性（CLR 包装）
    public bool IsOpen
    {
        get { return (bool)GetValue(IsOpenProperty); }
        set { SetValue(IsOpenProperty, value); }
    }

    // ② 注册依赖属性（名称、类型、所属类、默认值 + 变更回调）
    public static readonly DependencyProperty IsOpenProperty =
        DependencyProperty.Register("IsOpen", typeof(bool), typeof(Valve),
            new PropertyMetadata(false, new PropertyChangedCallback(OnIsOpenChanged)));

    // ③ 值变化时改变外观
    private static void OnIsOpenChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        var valve = d as Valve;
        valve.rt.Angle = valve.IsOpen ? 0 : 90;   // rt = XAML 里的 RotateTransform
    }
}
```

配套的 XAML 里要给目标元素起名（`x:Name`）：

```xml
<RotateTransform x:Name="rt" Angle="90"/>
```

**为什么用依赖属性而不是普通属性？**

| 能力 | 普通属性 | 依赖属性 |
|------|:--------:|:--------:|
| 数据绑定 `{Binding}` | ❌ | ✅ |
| 属性动画 | ❌ | ✅ |
| 样式 / 触发器修改 | ❌ | ✅ |
| 属性值继承 | ❌ | ✅ |

> 依赖属性是 WPF 属性系统的基石——**自定义控件只要想被绑定或动画驱动，就必须用依赖属性**。

### 在界面中使用

```xml
<!-- 先声明命名空间 -->
xmlns:c="clr-namespace:Configration.Components"

<!-- 在绘图区直接使用 -->
<c:Tank Width="100" Level="{Binding ...}"/>
<c:Valve Width="40" IsOpen="True"/>
<c:WarningLight Width="15" State="Run"/>
<c:Feed Width="50"/>
```

---

## 五、知识点清单

### 1. XAML 基础语法

**注释**

```xml
<!-- 注释 -->
```

- ❌ 不能写在标签内部：`<Window <!-- 错 --> Width="100">`
- ❌ 不能嵌套、内容里不能有 `--`

**属性语法 vs 属性元素语法**

> 值简单、能用字符串表达 → 写进 `<>` 里；值是复杂对象 → 用 `<标签.属性>` 包起来。

```xml
<TextBlock Text="你好" FontSize="12"/>          <!-- 属性语法 -->

<Window.Background>                             <!-- 属性元素语法 -->
    <LinearGradientBrush>...</LinearGradientBrush>
</Window.Background>
```

**附加属性（Attached Property）**

```xml
<Button Grid.Row="0" Grid.Column="1"/>                                  <!-- Grid 提供 -->
<Setter Property="WindowChrome.IsHitTestVisibleInChrome" Value="True"/> <!-- WindowChrome 提供 -->
```

- `Grid.Row` / `Grid.Column` 默认都是 `0`，多个元素不写会**重叠**
- 附加属性可**继承**：`<Grid TextBlock.FontSize="11">` 里所有 TextBlock 自动继承

---

### 2. 颜色格式

| 格式 | 例子 | 说明 |
|------|------|------|
| `#RGB` | `#F00` | 简写 |
| **`#ARGB`** | `#3FFF` | 带透明度（项目里大量使用） |
| `#RRGGBB` | `#15212F` | 标准 |
| `#AARRGGBB` | `#44477CD3` | 带透明度 |

> `#3FFF` = 透明度 3 + 白色；项目用 `#3FFF` ~ `#8FFF` 做"由暗到亮"的层次。

---

### 3. 画刷（Brush）

**LinearGradientBrush —— 线性渐变**

```xml
<LinearGradientBrush StartPoint="0,0" EndPoint="0,1">
    <GradientStop Color="#15212F" Offset="0"/>
    <GradientStop Color="#1D2D3E" Offset="0.6"/>
    <GradientStop Color="#15212F" Offset="1"/>
</LinearGradientBrush>
```

- `StartPoint → EndPoint`：方向（`0,0→0,1` 竖向；`0,0→1,0` 横向）
- `GradientStop`：`Color` + `Offset`(0~1)，中间自动插值

**RadialGradientBrush —— 径向渐变**

```xml
<RadialGradientBrush Center="0.5,1" GradientOrigin="0.5,1">
    <GradientStop Color="#385FB0" Offset="0"/>
    <GradientStop Color="#152230" Offset="1"/>
</RadialGradientBrush>
```

`0.5,1` = 底边中点 → 从底部中心向外辐射。

**VisualBrush —— 用 UI 元素当图案平铺**

```xml
<VisualBrush Viewport="0,0,10,10" ViewportUnits="Absolute" TileMode="Tile" Opacity="0.3">
    <VisualBrush.Visual>
        <Border Width="10" Height="10" BorderThickness="1,1,0,0" BorderBrush="#1FFF"/>
    </VisualBrush.Visual>
</VisualBrush>
```

**技巧**：只画左、上两条边（`1,1,0,0`），平铺拼起来才是**均匀网格**，不会重复叠线。

---

### 4. 形状与图形

**Path —— 画任意矢量图形**

```xml
<Path Data="M0,0 13 25 C20,38 20,38 40,38 L390 38 ..." StrokeThickness="1" Stretch="Fill"/>
```

| 命令 | 含义 |
|------|------|
| `M x,y` | Move：移动到某点（不画线） |
| `L x,y` | Line：直线 |
| `C c1 c2 end` | Cubic：三次贝塞尔曲线 |
| `A rx ry 旋转 大弧 扫掠 x,y` | Arc：椭圆弧 |
| `Z` | Close：闭合 |

- `Stroke` = 描边，`Fill` = 填充；只设 `Stroke` = 空心轮廓
- **交替 `A` 命令的扫掠标志（0/1）可以拼出波浪线**（`Tank` 组件的液面就是这么画的）

**其他形状**：`Ellipse`、`Line`、`Polyline`、`Polygon`

---

### 5. 布局容器

| 容器 | 特点 |
|------|------|
| **`Grid`** | 自由表格：`RowDefinitions` / `ColumnDefinitions`；列宽 **固定 / `auto` / `*`**；`Grid.RowSpan` 跨行列 |
| **`StackPanel`** | 一行或一列排列；`Orientation` 管子元素，`Alignment` 管自身位置 |
| **`UniformGrid`** | 等分网格；`Rows` / `Columns`；**不负责间距**（要加 `Margin`） |
| **`Border`** | **单子元素容器 + 装饰器**（背景 + 边框 + 圆角 + 内边距）；只设 `Background` = 纯色块 |
| **`Canvas`** | 绝对定位画布，适合工艺流程图的自由摆放 |
| **`Viewbox`** | 内容整体等比缩放（`Tank` 用它自适应尺寸） |
| **`ScrollViewer`** | 内容超出时可滚动 |

---

### 6. 常用控件

**GroupBox —— 带标题的分组框**

```xml
<GroupBox Header="设备配置详情">
    <Grid>...</Grid>
</GroupBox>
```

继承自 `HeaderedContentControl`，有 `Header`（标题）+ `Content`（内容）。

**ProgressBar —— 进度条**

```xml
<ProgressBar Minimum="0" Maximum="100" Value="79"
             Foreground="Orange" Background="#2000" Height="3"/>
```

> ⚠️ **`Foreground` 才是"进度"颜色，`Background` 是轨道底色**——和一般控件相反！

**Image —— 图片**

```xml
<Image Source="/Asserts/Images/logo_cyan.png" Width="28" Height="28" Stretch="Uniform"/>
```

> ⚠️ **不设宽高时按原始像素显示**，且默认 `Stretch="Uniform"` 会在空间大时**放大填满**。图标类务必设宽高。

---

### 7. 样式与模板

**Style + Setter**

```xml
<!-- 具名样式：需显式引用 -->
<Style TargetType="Button" x:Key="MenuButtonStyle">
    <Setter Property="Height" Value="30"/>
</Style>

<!-- 隐式样式：不写 x:Key，该类型自动套用 -->
<Style TargetType="GroupBox">...</Style>
```

**ControlTemplate —— 完全重绘外观**

```xml
<Setter Property="Template">
    <Setter.Value>
        <ControlTemplate TargetType="Button">
            <Border CornerRadius="5" Background="Transparent" Name="border">
                <ContentPresenter VerticalAlignment="Center" HorizontalAlignment="Center"/>
            </Border>
            <ControlTemplate.Triggers>...</ControlTemplate.Triggers>
        </ControlTemplate>
    </Setter.Value>
</Setter>
```

> **`ContentPresenter` 是 ContentControl 模板的必需元素**——不放它，控件内容不显示。

**Trigger —— 条件触发器**

```xml
<Trigger Property="IsMouseOver" Value="True">
    <Setter TargetName="border" Property="Background" Value="#3000"/>
</Trigger>
```

- `Property` + `Value` = 条件；`TargetName` = 改模板内命名元素
- 条件不满足时**自动恢复**

**本项目自定义的样式**

| 样式 | 作用 |
|------|------|
| `MenuButtonStyle` | 菜单按钮：透明圆角，悬停底部淡白光 |
| `ExitButtonStyle` | 退出按钮：橙色旗帜造型 |
| `GroupBox`（隐式） | 工业风分组框：圆点装饰 + 标题 + 渐隐箭头 |
| `DeviceTypeButtonStyle` | 单选按钮：选中时橙色下划线 |
| `ControlButtonStyle` | 控制按钮：蓝色渐变 + 箭头尾巴 |

---

### 8. 变换（Transform）

```xml
<TextBlock.RenderTransform>
    <ScaleTransform ScaleX="1.2"/>
</TextBlock.RenderTransform>
```

| 变换 | 作用 |
|------|------|
| `ScaleTransform` | 缩放（`ScaleX` / `ScaleY`） |
| `TranslateTransform` | 平移（`X` / `Y`）——**Tank 用它移动液位** |
| `RotateTransform` | 旋转（`Angle`）——**Valve 用它开关阀门** |

> - `RenderTransform` **只改渲染、不改布局**（不改变占位）
> - ⚠️ **单向拉伸（只 `ScaleX`）会让图形 / 文字变形**，不是调宽度的正确方法（应改 `FontSize`）

---

### 9. 动画（Animation）

**代码里播放动画**

```csharp
// 300ms 内平滑过渡到目标值
DoubleAnimation da = new DoubleAnimation(targetValue, TimeSpan.FromMilliseconds(300));
element.BeginAnimation(TranslateTransform.YProperty, da);
```

**XAML 里播放动画（事件触发）**

```xml
<UserControl.Triggers>
    <EventTrigger RoutedEvent="Loaded">          <!-- 控件加载时启动 -->
        <BeginStoryboard>
            <Storyboard>
                <DoubleAnimation Duration="0:0:2"
                                 From="0" To="-118"
                                 Storyboard.TargetName="tt"
                                 Storyboard.TargetProperty="X"
                                 RepeatBehavior="Forever"/>   <!-- 无限循环 -->
            </Storyboard>
        </BeginStoryboard>
    </EventTrigger>
</UserControl.Triggers>
```

**本项目应用**：
- `Tank`：波浪横向循环流动（`X`）+ 液位升降（`Y`，由代码触发）
- 可用于：阀门平滑旋转、指示灯闪烁、数值滚动等

---

### 10. 自定义控件与依赖属性 ★

**UserControl —— 组合出可复用组件**

```
Components/XXX.xaml      ← 外观（用基础控件组合）
Components/XXX.xaml.cs   ← 逻辑（依赖属性 + 回调）
```

**依赖属性三步走**（详见第四章）

```csharp
// ① CLR 包装属性
public bool IsOpen { get => (bool)GetValue(IsOpenProperty); set => SetValue(IsOpenProperty, value); }

// ② 注册（名、类型、所属类、默认值 + 回调）
public static readonly DependencyProperty IsOpenProperty =
    DependencyProperty.Register("IsOpen", typeof(bool), typeof(Valve),
        new PropertyMetadata(false, OnIsOpenChanged));

// ③ 回调里改外观（操作 XAML 中 x:Name 的命名元素）
private static void OnIsOpenChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
{
    (d as Valve).rt.Angle = (d as Valve).IsOpen ? 0 : 90;
}
```

**枚举类型依赖属性**（`WarningLight`）

```csharp
public enum LightState { None, Fault, Warning, Run }

public LightState State { get => (LightState)GetValue(StateProperty); set => SetValue(StateProperty, value); }
```

回调里按枚举值切换多个元素的 `Visibility`。

---

### 11. 数据绑定与 MVVM

**设置 DataContext**

```csharp
public MainView()
{
    InitializeComponent();
    this.DataContext = new MainViewModel();   // 连接 View 与 ViewModel
}
```

**Binding**

```xml
<TextBlock Text="{Binding Header}"/>
<TextBlock Text="{Binding Value, StringFormat={}{0}%}"/>   <!-- 79 → "79%" -->
```

**ItemsControl 三件套**

```xml
<ItemsControl ItemsSource="{Binding StateList}">

    <ItemsControl.ItemsPanel>            <!-- ① 用什么容器排列所有项 -->
        <ItemsPanelTemplate>
            <StackPanel/>
        </ItemsPanelTemplate>
    </ItemsControl.ItemsPanel>

    <ItemsControl.ItemTemplate>          <!-- ② 每一项长什么样 -->
        <DataTemplate>
            <Grid Height="42">
                <Image Source="{Binding Icon}"/>
                <TextBlock Text="{Binding Header}"/>
                <ProgressBar Value="{Binding Value}"/>
            </Grid>
        </DataTemplate>
    </ItemsControl.ItemTemplate>
</ItemsControl>
```

| 部分 | 决定什么 |
|------|----------|
| `ItemsSource` | 数据来源（集合） |
| `ItemsPanel` | **用什么容器排列**（不写默认垂直 `StackPanel`） |
| `ItemTemplate` | **每一项的外观**（内部 `{Binding}` 绑定到当前项） |

**Model / ViewModel**

```csharp
// Models/DeviceStateModel.cs
public class DeviceStateModel
{
    public string Icon { get; set; }
    public string Header { get; set; }
    public double Value { get; set; }
}

// ViewModels/MainViewModel.cs
public List<DeviceModel> DeviceList { get; set; } = new();
public List<DeviceStateModel> StateList { get; set; } = new();
public ObservableCollection<MessageModel> MessageList { get; set; } = new();
```

**`List<T>` vs `ObservableCollection<T>`**

| | 增删改时界面是否自动刷新 |
|---|:---:|
| `List<T>` | ❌ |
| **`ObservableCollection<T>`** | ✅（本项目 `MessageList` 用它） |

> ⚠️ `{Binding xxx}` 的属性名必须和 Model 的属性名**完全一致**，否则绑定失败、界面空白。

---

### 12. 第三方图表（LiveCharts）

```xml
xmlns:lvc="clr-namespace:LiveCharts.Wpf;assembly=LiveCharts.Wpf"

<lvc:CartesianChart>
    <lvc:CartesianChart.Series>
        <lvc:LineSeries Values="10,25,43,30,80">
            <lvc:LineSeries.Fill>
                <LinearGradientBrush StartPoint="0,0" EndPoint="0,1">
                    <GradientStop Color="#416eda" Offset="0"/>
                    <GradientStop Color="#00416eda" Offset="1"/>
                </LinearGradientBrush>
            </lvc:LineSeries.Fill>
        </lvc:LineSeries>
    </lvc:CartesianChart.Series>

    <lvc:CartesianChart.AxisX>
        <lvc:Axis Labels="09:00,12:00,15:00,18:00,21:00" FontSize="9">
            <lvc:Axis.Separator>
                <lvc:Separator Step="1" StrokeThickness="0"/>
            </lvc:Axis.Separator>
        </lvc:Axis>
    </lvc:CartesianChart.AxisX>
</lvc:CartesianChart>
```

本项目用了 **4 个图表**：折线图 ×3 + 柱形图 ×1。

> 📌 **注意版本**：本项目用的是 **`LiveCharts.Wpf` 0.9.7（旧版 v0）**。
> 目前更活跃的是 **LiveCharts2**，包名 `LiveChartsCore.SkiaSharpView.WPF`，API 完全不同
> （`ISeries` / `ObservableCollection<ISeries>` / SkiaSharp 渲染）。**两者不可混用。**

---

### 13. 资源管理

**两级资源字典**

```xml
<!-- App.xaml：全局，整个应用可用 -->
<Application.Resources>
    <FontFamily x:Key="Jinbuti">pack://application:,,,/Asserts/Fonts/DingTalk-JinBuTi.ttf#钉钉进步体</FontFamily>
</Application.Resources>

<!-- MainView.xaml：窗体级，仅本窗体可用 -->
<Window.Resources>
    <Style TargetType="Button" x:Key="MenuButtonStyle">...</Style>
</Window.Resources>
```

引用方式：`{StaticResource Jinbuti}`

**pack URI 与自定义字体**

```
pack://application:,,,/Asserts/Fonts/DingTalk-JinBuTi.ttf#钉钉进步体
└──── 应用资源 ────┘└──────── 路径 ────────┘└─ 字体内部家族名 ─┘
```

**⚠️ 资源必须在 csproj 里声明为 `Resource`**

```xml
<ItemGroup>
    <Resource Include="Asserts\Fonts\DingTalk-JinBuTi.ttf" />
    <Resource Include="Asserts\Fonts\iconfont.ttf" />
    <Resource Include="Asserts\Images\*.png" />
    <PackageReference Include="LiveCharts.Wpf" Version="0.9.7" />
</ItemGroup>
```

否则 `.ttf` / `.png` 默认只是 `None`，**不嵌入程序集** → 设计器能看到（读磁盘），**运行时加载失败**。

---

### 14. 无边框窗口

```xml
<WindowChrome.WindowChrome>
    <WindowChrome NonClientFrameEdges="None" UseAeroCaptionButtons="False"/>
</WindowChrome.WindowChrome>
```

无边框后标题栏区域默认**不响应点击**，按钮必须加：

```xml
<Setter Property="WindowChrome.IsHitTestVisibleInChrome" Value="True"/>
```

---

## 六、踩坑记录

| # | 问题 | 原因 / 解决 |
|---|------|-------------|
| 1 | 字体 / 图片加载不出来 | 路径写成 `Assets`，实际文件夹是 **`Asserts`**（拼写坑） |
| 2 | XAML 报红波浪线 | 注释写在了标签内部 → **注释只能放元素之间** |
| 3 | 设计器能看到图片，运行却空白 | 资源没在 csproj 声明为 `<Resource>` |
| 4 | 图标显示得异常大 | `Image` 没设宽高 + 默认 `Stretch="Uniform"` 会放大填满 → **设 `Width` / `Height`** |
| 5 | 两行文字对不齐 | 用了 `ScaleTransform ScaleX` 拉伸（变形）→ 改用 `FontSize` |
| 6 | 进度条颜色不对 | `ProgressBar` 的**进度色是 `Foreground`** |
| 7 | 列表项挤在一起重叠 | `UniformGrid` 强制等分，数据多时每项只剩几像素 → 换 `StackPanel` + `ScrollViewer` |
| 8 | 元素重叠 | 多个元素没写 `Grid.Row` / `Grid.Column`，都落在第 0 行第 0 列 |
| 9 | 数据不显示 | Model 属性名与 `{Binding xxx}` 不一致，或没设 `DataContext` |
| 10 | 字体 `#` 后面写什么 | 必须是字体**内部家族名**（可用 `System.Drawing` 读取确认） |
| 1 | `CartesianChart` 找不到 | 需要装 NuGet 包；注意 **LiveCharts v0 与 LiveCharts2 是两个不同的库** |
| 12 | 动画没生效 | 依赖属性没注册、`x:Name` 没对上、或没用 `BeginAnimation` |

---

## 七、Visual Studio 快捷键速查

| 快捷键 | 作用 |
|--------|------|
| **F7** | XAML ↔ 代码后置切换 |
| **Shift+F7** | 切到设计视图 |
| **F4** | 属性窗口 |
| **Ctrl+K, Ctrl+D** | 格式化文档 |
| **Ctrl+K, Ctrl+F** | 格式化选中 |
| **Ctrl+K, Ctrl+C** | 注释 |
| **Ctrl+K, Ctrl+U** | 取消注释 |
| **Ctrl+Space** | 智能提示 |
| **F12** | 转到定义 |
| **Ctrl+M, Ctrl+M** | 折叠 / 展开当前块 |

> XAML 格式化建议装 **XAML Styler** 扩展，VS 自带的较弱。

---

## 八、知识点自查表

### 布局
- [x] `Grid`（行列定义、`*` / `auto` / 固定、`RowSpan` / `ColumnSpan`）
- [x] `StackPanel`（`Orientation` 与 `Alignment` 的区别）
- [x] `UniformGrid`（等分网格、自动分行列）
- [x] `Border`（单子元素容器 + 装饰）
- [x] `Canvas`（绝对定位）
- [x] `Viewbox`（等比缩放）
- [x] `ScrollViewer`

### 控件
- [x] `Button` / `RadioButton` / `TextBlock` / `Image` / `ProgressBar` / `GroupBox`
- [x] `ItemsControl`（`ItemsSource` / `ItemsPanel` / `ItemTemplate`）
- [x] 第三方图表 `LiveCharts`

### 样式与模板
- [x] `Style` + `Setter`
- [x] `ControlTemplate` + `ContentPresenter`
- [x] `Trigger`（`IsMouseOver` / `IsChecked`）
- [x] 隐式样式 vs 具名样式

### 图形
- [x] `Path`（`M` / `L` / `C` / `A` / `Z`）
- [x] `Ellipse` / `Line` / `Polyline` / `Polygon`
- [x] `Canvas.Clip` 裁剪
- [x] 各类 `Brush`（Solid / Linear / Radial / Visual）

### 变换与动画
- [x] `RenderTransform`（`Scale` / `Translate` / `Rotate`）
- [x] `DoubleAnimation` + `BeginAnimation`
- [x] `Storyboard` + `EventTrigger` + `RepeatBehavior`

### 自定义控件
- [x] `UserControl` 组合式控件
- [x] **`DependencyProperty` 依赖属性**
- [x] `PropertyMetadata` + `PropertyChangedCallback`
- [x] 枚举类型依赖属性
- [x] 用命名元素（`x:Name`）操作外观

### 数据
- [x] `DataContext` + MVVM 分层
- [x] `Binding` / `StringFormat`
- [x] `DataTemplate`
- [x] `List<T>` vs `ObservableCollection<T>`

### 其他
- [x] 无边框窗口 `WindowChrome`
- [x] 资源字典（App / Window 两级）
- [x] `pack://` URI 与自定义字体
- [x] csproj 资源声明

---

## 九、构建与运行

```bash
# 还原依赖 + 构建
dotnet build Configration/Configration.csproj

# 运行
dotnet run --project Configration/Configration.csproj
```

或用 Visual Studio 打开 `Configration.slnx`，按 **F5** 运行。

---

## 十、后续可拓展方向

项目已完结。如需继续深入 WPF：

- [ ] 把 `List<T>` 全部换成 `ObservableCollection<T>`，支持数据实时刷新
- [ ] 用 `DispatcherTimer` 让时间 / 数值真正动起来
- [ ] 按钮接入 `Command`（MVVM 命令绑定，彻底去掉代码后置）
- [ ] 引入 `INotifyPropertyChanged` 做双向绑定
- [ ] 把 `Asserts/Styles` 抽成独立 `ResourceDictionary`
- [ ] 尝试 `LiveCharts2`，对比旧版 API 差异
- [ ] 接入真实数据源（Modbus / OPC UA / 串口）

> 💡 **学习建议**：界面绘制只是上位机开发的一部分。若要走向实际工作，下一步重点应放在
> **通信（Modbus / PLC / OPC UA）→ 多线程 → 数据持久化** 上。
