# 快速开始

（1）先导入需要的第三方插件
1. DoTween
2. Addressable
3. 导入框架

（2）创建 Resources 文件夹
- 在文件夹内，右键 --> MRFramework --> 创建 UISetting（注意名字要一致）
- 在文件夹内，右键 --> MRFramework --> 创建 LogConfig（注意名字要一致）

UISetting 用于 UI 模块设置
LogConfig 用于日志打印系统设置

修复 Bug 流程，接收到玩家的 Bug 反馈可以让其提供
1. 日志系统存储的 Bug 日志信息
2. SaveUtil 存储的游戏存档
把存档导入 Unity ，对照着 Bug 日志还原 Bug 场景

（3）创建游戏开始场景
- 场景两个场景，游戏开始场景（比如 GameStartScene），主页面场景（比如 MainScene）
- 在开始场景场景一个 GameObject 挂载上 GameBoot 脚本
- 打开 GameBoot 脚本，找到 ”SceneManager.LoadScene("");” 添加上主页面场景的名称

游戏开始场景功能：
1. 进行框架初始化
2. 如果是网游，这一步应该是检测游戏内容热更新
3. 启动游戏开始界面
	- 比如游戏进度条加载
	- 比如热更新下载进度条
	- 游戏开始加载面板不用使用UI框架，直接使用 Unity 原生的 MonoBehaviour 脚本即可，该资源一般都会放在 Resources 文件夹中，会随着游戏整包一起打出去
4. 主页面场景，游戏开发的主场景

（4）添加宏命令 OPEN_LOG

![](笔记图片库/Pasted%20image%2020240707203523.png)

（5）创建资源文件夹

创建两个文件夹：
- ArtRes （名字随便）游戏资源
- AssetPackage（名称不能更改）收集需要打包的资源

目录结构：
- MRFramework
	- ExtensionAPI 公共扩展API
	- Framework 框架架构
	- Modules 框架模块
	- Tools 框架工具集
	- GameBoot.cs 框架启动脚本
	- GameModule.cs 框架模块管理脚本

# 框架模块 Modules

## 01 单例模式

普通单例（必须提供私有化构造函数）：
```C#
public class TestManager : Singleton<TestManager>
{
    // 私有化构造函数
    private TestManager() { }

	// 重写 OnSingletonInit 单例被创建时会被调用
	public override void OnSingletonInit()
	{
		
	}

	// 重写 DestroySingleton 单例被销毁时被调用
	public override void DestroySingleton()
    {
	    // 建议保留
        base.DestroySingleton();
    }
}
```

继承 MonoBehaviour 的单例：
```C#
// 如果不提供生成路径，就会按默认的生成位置
// 该自定义路径表示：该脚本会被挂载到 Test 对象的子对象 MonoScript 对象身上
[MonoSingletonPath("自定义生成路径：Test/MonoScript")]
public class MonoScript : MonoSingleton<TestManager>
{
    // 重写 OnSingletonInit 单例被创建时会被调用
	public override void OnSingletonInit()
	{
		
	}

	// 重写 OnDestroy 单例被销毁时被调用
    public override void OnDestroy()
    {
		// 建议保留
        base.OnDestroy();
    }
}
```

## 02 有限状态机

链式适合在快速开发阶段，或者在状态非常少的阶段使用。
而如果状态较多，或者相应代码量较多的阶段，可以使用类模式。

链式模式：
```C#
public class Test : MonoBehaviour
{
    // 创建状态枚举
    public enum States
    {
        AState,
        BState
    }
    
    // 创建状态机
    public FSM<States> FSM = new FSM<States>();
    
    private void Start()
    {
        // 创建状态A链
        FSM.State(States.AState)
            .OnCondition(() => FSM.CurrentStateId == States.BState)
            .OnEnter(() => { Debug.Log("AState OnEnter"); })
            .OnUpdate(() =>
            {
                if(Input.GetKeyDown(KeyCode.Space))
                    FSM.ChangeState(States.BState);
            })
            .OnFixedUpdate(() => { })
            .OnGUI(() => { })
            .OnExit(() => { Debug.Log("BState OnEnter"); });
        
        // 创建状态B链
        FSM.State(States.BState)
            .OnCondition(() => FSM.CurrentStateId == States.AState)
            .OnUpdate(() =>
            {
                if(Input.GetKeyDown(KeyCode.Space))
                    FSM.ChangeState(States.AState);
            });
        
        // 从状态A开始
        FSM.StartState(States.AState);
    }
    
    private void Update()
    {
        FSM.Update();
    }

    private void FixedUpdate()
    {
        FSM.FixedUpdate();
    }

    private void OnGUI()
    {
        FSM.OnGUI();
    }

    private void OnDestroy()
    {
        FSM.Clear();
    }
}
```

类模式：支持和链式模式混用
```C#
public class StateClassExample : MonoBehaviour
{
    // 创建状态枚举
    public enum States
    {
        AState,
        BState,
        CState
    }
    
    // 创建状态机
    public FSM<States> FSM = new FSM<States>();
    
    // 创建状态A类
    public class StateA : AbstractState<States, StateClassExample> // States: 状态枚举, StateClassExample: 状态机类
    {
        public StateA(FSM<States> fsm, StateClassExample target) : base(fsm, target)
        {
        }
        
        protected override bool OnCondition()
        {
            return fsm.CurrentStateId == States.BState;
        }

        protected override void OnEnter()
        {
            Debug.Log("AState OnEnter");
        }

        protected override void OnUpdate()
        {
            if(Input.GetKeyDown(KeyCode.Space))
                fsm.ChangeState(States.BState);
        }
    }
    
    // 创建状态B类
    public class StateB : AbstractState<States, StateClassExample>
    {
        public StateB(FSM<States> fsm, StateClassExample target) : base(fsm, target)
        {
        }
        
        protected override bool OnCondition()
        {
            return fsm.CurrentStateId == States.AState;
        }
        
        protected override void OnEnter()
        {
            Debug.Log("BState OnEnter");
        }
        
        protected override void OnUpdate()
        {
            if(Input.GetKeyDown(KeyCode.Space))
                fsm.ChangeState(States.AState);
        }
    }
    
    private void Start()
    {
        // 状态机添加状态
        FSM.AddState(States.AState, new StateA(FSM, this));
        FSM.AddState(States.BState, new StateB(FSM, this));
        
        // 支持和链式模式混用
        // FSM.State(States.CState)
        //     .OnEnter(() =>
        //     {
        //
        //     });
        
        // 从状态A开始
        FSM.StartState(States.AState);
    }
    
    private void Update()
    {
        FSM.Update();
    }

    private void OnDestroy()
    {
        FSM.Clear();
    }
}
```

## 03 事件系统

### 3.1 字符串事件

```C#
// 事件注册
GameModule.EventMgr.AddEventListener("EventKey1", Func1);  
GameModule.EventMgr.AddEventListener<int>("EventKey2", Func2); // 需要更多可以自己扩展

private void Func1()  
{  
    Log.Info("Test");  
}  
  
private void Func2(int num)  
{  
    Log.Info("Test", num);  
}

// 事件触发
GameModule.EventMgr.TriggerEventListener("EventKey1");
GameModule.EventMgr.TriggerEventListener<int>("EventKey2", 10);

// 事件注销
GameModule.EventMgr.RemoveEventListener("EventKey1", Func1);  
GameModule.EventMgr.RemoveEventListener<int>("EventKey2", Func2);
```

### 3.2 类型事件

```C#
// 事件注册
GameModule.EventMgr.AddEventListener<TestEvent>(Func3);

public struct TestEvent  // 也可以使用类，但是建议使用结构体，因为 struct 的 gc 更少
{  
    public int Num; 
}

public void Func3(TestEvent e)  
{  
    Log.Info(e.Num);
}

// 事件触发
GameModule.EventMgr.TriggerEventListener<TestEvent>(new TestEvent()  
{  
    Num = 18  
});

// 事件注销
GameModule.EventMgr.RemoveEventListener<TestEvent>(Func3);
```

### 3.3 事件自动注销

注册事件时，如果没有特殊需求，建议都使用自动注销，这样就不用手动管理事件的注销了。

继承了 MonoBehaviour 的脚本：
```C#
// 在 GameObject 对象被销毁时，自动注销事件
GameModule.EventMgr.AddEventListener("EventKey1", Func1).AutoUnRegister(this);  

GameModule.EventMgr.AddEventListener<int>("EventKey2", Func2).AutoUnRegister(this);

GameModule.EventMgr.AddEventListener<TestEvent>(Func3).AutoUnRegister(this);
```

非继承 MonoBehaviour 的脚本：
```C#
// 1. 继承 IUnRegisterList 接口
public class NoneMonoScript : IUnRegisterList
{
	// 2. 实现接口
	public List<IUnRegister> UnregisterList { get; } = new List<IUnRegister>();

	// 3. 添加事件
	public void AutoTest()
	{
		GameModule.EventMgr.AddEventListener("EventKey1", Func1)
			.AddToUnregisterList(this); 
			
		GameModule.EventMgr.AddEventListener<int>("EventKey2", Func2)
			.AddToUnregisterList(this);
			
		GameModule.EventMgr.AddEventListener<TestEvent>(Func3)
			.AddToUnregisterList(this);
	}

	// 4. 对象被销毁时调用，将添加到列表的事件全部注销
	public void OnDestroy()
	{
		this.UnRegisterAll();
	}
}
```

## 04 UI 模块

后续更新计划：
1. 实时加载子面板
2. 面板粒子特效加载

示例：
```C#
// 打开面板 (主面板使用，子面板请使用 Open)
GameModule.UIMgr.OpenPanle<WBP_Login_Main>();

// 关闭面板 (主面板使用，子面板请使用 Close)
GameModule.UIMgr.ClosePanel<WBP_Login_Main>();

// 销毁面板 或者 等关闭面板一段时间后自动销毁
GameModule.UIMgr.DisposePanel<WBP_Login_Main>();

// 初始化子面板
AddSubView<WBP_Login_ItemSV>();

// 关闭和打开子面板
WBP_Login_ItemSV.Close();
WBP_Login_ItemSV.Open();
```

### 4.1 遮罩系统

UI 遮罩系统的也是项目中很重要的一个功能，市面上普遍存在两种遮罩模式，一种是单遮模式，一种则是叠遮模式。
1. 单遮模式：无论打开多少界面，遮罩只有一层。
2. 叠遮模式：即每一个界面都有一层遮罩，**打开的界面越多，遮罩就越黑**。

市面上使用的普遍是前者居多，即单遮模式，相较于叠遮，单遮就比较复杂一点，问题也比较多一点。而叠遮则很简单。因为项目中是有很多特殊情况的，如果单遮模式写的不够完善，考虑的不够周全，那在碰到这些特殊情况的时候往往最让人头疼。

举几个特殊情况的例子：
1. 当前弹出了一个二级且有遮罩的弹窗，然后别人的代码又弹出了一个不需要遮罩的弹窗，比如滚动公告界面，这种情况如果不处理，就会出问题，导致二级弹窗遮罩丢失。
2. 当前弹出了一个层级为 110 的二级弹窗，然后又弹出了一个层级为 100 的二级弹窗，二者都有遮罩，如果处理不当，遮罩肯定在层级为 100 的弹窗上显示，就造成了遮罩错乱的问题。

### 4.2 UI 层级系统

相信大家在开发时都会碰到界面与模型与特效与界面之间的穿插，并且在处理的时候搞的手忙脚乱，在费劲心思处理完出特效穿插的问题后，发想与其他界面的层级又乱套了，不得已又去重新改，但是界面层级一改就意味着特效的层级也要调，又要手动一个特效一个特效的去调层级。相信这个种问题是大家在开发初期都会遇到的。所以一款完善的 UI 管理系统就起到的至关重要的作用。

设计理念是把每个不同等级的弹窗故意留了 100 个层级去适配特效。即基础界面的特效永远不会穿插到二级弹窗上去，因为框架设计就已经避免掉了。当然如果是相同等级的弹窗叠加的情况，该设计仍然有 100 的层级去适配叠加的情况。

完全能做到，清晰透明的有规划的层级管理。 改了这个弹窗一眼就知道影响的其他弹窗有几个都是那些。

![](笔记图片库/Pasted%20image%2020240413200655.png)

### 4.3 自动化系统（弃用）

窗口组件：
1. TempWindow1 为 Canvas，名称可以随意修改
2. UIMask 为 UI 遮罩
3. UIContent 为 UI 组件存放的地方，UI 面板的拼接，所有组件都放在 UIContent 里面

![](笔记图片库/Pasted%20image%2020240413225111.png)

自动化生成脚本流程：
1. 在窗口对象的 **UIContent** 上挂载 CodeGen 脚本
2. 拖拽（可以多选拖拽）需要的 UI 组件然后点击代码生成
3. 编译结束后会为窗口对象挂载上带有 `UIContent` 后缀的脚本，该脚本主要进行 UI 组件的绑定，提供给窗口脚本。

![](笔记图片库/Pasted%20image%2020240413225525.png)

窗口脚本会自动生成 UI 事件函数：
1. 全称为 `CloseButton` 的按钮为关闭窗口按钮，自动化生成时会自动设置好关闭函数，无需写关闭的逻辑。
2. **自动化工具后续会继续增量添加 UI 组件事件函数，原理就是会找到 `#region UI组件事件` 索引下的第一个 Public 函数，然后插入进去。**
3. 如果后续不想要自动化工具管理该窗口脚本，只需要修改 `#region UI组件事件` 名称，或者直接删除。

全局设置：可以设置 CodeGen 脚本的初始化参数

![](笔记图片库/Pasted%20image%2020240413232145.png)

### 4.3 新版自动化系统

使用前缀生成UI控件，例如：Button_Close

对制作好的UI面板右键 --> UIPanle --> 自动生成UI脚本（快捷键 Shift + Z）

### 4.4 堆栈系统（暂时弃用）

堆栈系统是任何游戏必不可少的一项功能。它用作与首次进入大厅时一些特殊或活动面板的**有序自动弹出**，从而让玩家能够更好的去了解到游戏内容和新增功能。

接口函数：
```C#
// 弹窗压入堆栈，弹出时会执行 popCallBack 回调函数
// 把弹出压入堆栈后，会阻塞住，知道开始执行弹出
public void PushWindow<T>(Action<WindowBase> popCallBack = null)

// 开始执行堆栈弹窗
// 会首先打开第一个压入堆栈的弹窗，关闭这个弹窗后会继续打开下一个，知道压入堆栈的弹窗全部弹出为止
public void StartPopWindow()

// 压入并且弹出堆栈弹窗
// 压入堆栈后不会阻塞住，马上开始执行弹出操作
public void PushAndPopWindow<T>(Action<WindowBase> popCallBack = null)

// 清空弹窗堆栈
public void ClearStackWindows()
```

示例：
```C#
// 压入堆栈，再弹出
// 1. 把需要弹窗的窗口压入堆栈
UIModule.Instance.PushWindow<ChatWindow>();
UIModule.Instance.PushWindow<TipsWindow>();
UIModule.Instance.PushWindow<InfoWindow>();
// 2. 开始执行堆栈弹窗，弹窗会按顺序弹出
UIModule.Instance.StartPopWindow()
// 注意：如果弹出的弹窗中，再执行压入堆栈操作，不会影响弹出顺序，后续压入的弹窗，会紧跟着顺序弹出

// 压入后马上弹出
// 1. 不会阻塞，ChatWindow 压入后马上就会开始执行弹出
UIModule.Instance.PushAndPopWindow<ChatWindow>();
// 2. ChatWindow 关闭后就会弹出 TipsWindow，下面同理
UIModule.Instance.PushAndPopWindow<TipsWindow>();
UIModule.Instance.PushAndPopWindow<InfoWindow>();
// 注意：弹出的弹窗中，再执行压入弹出堆栈操作，不会影响弹出顺序，后续压入的弹窗会紧跟着顺序弹出
```

弹出动画管理：
```C#
// 基础的窗口（0~90）没有弹窗动画，后面层级的窗口都要动画
// 如果后续的窗口也需要关闭动画，比如一些二级弹窗是全屏窗口的
// 只要在 OnAwake 设置弃用动画即可
public override void OnAwake()  
{  
    base.OnAwake();  
    UIContent = gameObject.GetComponent<ChatWindowUIContent>();  
    UIContent.InitComponent(this);  
      
    // 弃用动画
    mDisableAnim = true;
}
```

### 4.5 UGUI 优化

增加游戏的流畅度和优化开发过程而设计的一系列解决方案。主要针对渲染、重绘、顶点、UI 组件、等多个方面进行性能的优化处理，以及一些 UGUI 的功能扩展。

#### 4.5.0 快捷键
快捷键创建的窗口，会自动添加到 UIRoot 对象下，并且**自动绑定 UICamera**，无需手动绑定。
![](笔记图片库/Pasted%20image%2020240414022606.png)

#### 4.5.1 窗口预加载 （暂时弃用）
针对一些**复杂的界面**我们可以使用预加载进行提前加载物体，来确保在真正使用界面时，能够流畅度加载出界面。

示例：
```C#
// ChatWindow 窗口会预加载到场景，然后进行隐藏
UIModule.Instance.PreLoadWindow<ChatWindow>();

// 调用 OpenWindow，就会直接显示窗口，跳过了资源加载的步骤
UIModule.Instance.OpenWindow<ChatWindow>();
```

#### 4.5.2 智能化禁用不必要的组件属性
UI 组件创建时自动取消勾选 RaycastTarget，从而来避免一些不必要的性能开销。
目前扩展的组件有：Image、Text、RawImage 

![](笔记图片库/Pasted%20image%2020240414010444.png)

#### 4.5.3 窗口和组件隐藏
扩展**窗口**和**组件**以及 **GameObject**、**Transform** 隐藏不会再调用原生的 SetVisible，因为 SetVisible 会造成 UI 重绘和 GC 垃圾。用 CanvasGroup 和 Scale 进行代替。

示例：
```C#
GameObject obj = new GameObject();  
obj.SetVisible(false);
  
Button btn = GetComponent<Button>();  
btn.SetVisible(false);
```

#### 4.5.4 一键优化合批
自动根据图集图片和相邻组件的特征进行重新排序，减少 Drawcall。
![](笔记图片库/Pasted%20image%2020240414010038.png)

![](笔记图片库/Pasted%20image%2020240414010052.png)

## 05 资源加载

示例：
```C#
private AssetLoader mAssetLoader;
private int mHandleId;

private void Awake()
{
    // 获取资源加载器
    mAssetLoader = AssetLoader.Allocate();
}

private void Start()
{
    // 加载
    var handle = mAssetLoader.LoadAsset<GameObject>("资源路径");
    // 实例化
    Instantiate(handle.Result);
    
    // 加载完成后实例化
    var handle = mAssetLoader.Instantiate("资源路径");
    
    // 得到该资源的Id
    mHandleId = handle.Id;
    
    // 通过handle获取对象
    GameObject go = handle.Result;
    
    // 通过Id获取对象
    GameObject go = mAssetLoader.GetAsset<GameObject>(mHandleId);
}

private void Update()
{
    if (Input.GetKeyDown(KeyCode.Q))
    {
        // 不要使用 GameObject.Destroy();
        // 应该通过资源的Id把对象删除
        mAssetLoader.Release(mHandleId);
    }
}

private void OnDestroy()
{
    // 释放该 AssetLoader 加载的所有资源
    mAssetLoader.ReleaseAll();
    
    // AssetLoader 回收对象池
    mAssetLoader.Recycle2Cache();
}
```

## 06 音效模块

示例：
```C#
// 可以通过资源动态加载获得，或者手动拖拽
public AudioClip MusicClip; // 背景音乐
public AudioClip SoundClip; // 音效

private void Awake()
{
    // 播放音乐
    GameModule.AudioMgr.PlayMusic(MusicClip);
    
    // 其它 API 
    // ...
}

private void Update()
{
    if (Input.GetKeyDown(KeyCode.Q))
    {
        // 播放音效
        GameModule.AudioMgr.PlaySound(SoundClip);
        
        // 其它 API 
        // ...
    }
}
```

# 工具集

## 01 日志打印

日志系统功能介绍：
1. 颜色日志：自定义日志颜色，默认其中颜色任意选择。
2. 本地日志：用户可以将日志打印存储到本地文件，在测试期或线上遇到 BUG 时，能够准确及时的定为到哪些非必现的问题（存储路径在 LogConfig 查看）。
3. 编译剔除：在打包时可以选择将游戏内的所有日志相关的代码从 DLL 中剔除，从而避免字符串拼接带来的性能消耗。
4. 运行时查看：在移动设备上，我们可以通过屏幕划圈圈调出日志显示窗口，从而查日志打印，定位问题所在。
5. FPS 实时监测：屏幕上实时显示游戏运行时的帧率，更加清晰的查看游戏的性能消耗。
6. ProtoToJson：将与服务端通讯的所有 ProtoBuff 消息转换为 Json 字符串，并进行打印，帮助我们定位极端 Bug。

在 `Assets/MRFramework/Tools/Log/Resources/LogConfig.asset` 可以打开日志系统配置。

![](笔记图片库/Pasted%20image%2020240413232247.png)

关闭日志系统后，在打包时可以将游戏内的所有日志相关的代码从 DLL 中剔除，从而避免字符串拼接带来的性能消耗。

代码使用示例：
```C#
// 普通日志
Log.Info("123");
Log.Warning("123");
Log.Error("123");

// 颜色日志
Log.InfoBlue("123");
Log.InfoYellow("123");
Log.InfoGreen("123");
Log.InfoRed("123");

// 打印服务端 ProtoBuff 消息
ProtoBuffConvert.ToJson<T>(T proto);
```

## 02 Excel 导表工具

Excel 文件应该放置在项目根目录的 Excel 文件夹当中（防止游戏打包时把 Excel 文件打进整包）
	如果想要修改 就去修改 ExcelTool 当中的 EXCEL_PATH 路径变量

配置表规则
	第一行：字段名
	第二行：字段类型（字段类型一定不要配置错误，字段类型目前只支持int float bool string）
	第三行：主键是哪一个字段 需要通过"key"来标识主键
	第四行：描述信息（只是给别人看，不会有别的作用）
	第五行~第n行：就是具体数据信息
	下方的表名决定了数据结构类，容器类，Json文件的文件名

![](笔记图片库/Pasted%20image%2020240707202015.png)

![](笔记图片库/Pasted%20image%2020240707202132.png)

使用示例：
```C#
// 获取导表数据
GameConfig.GetConfig<Global_Pet_Pet, int>();

// 按行获取导表数据
GameConfig.GetConfigRow<Global_Pet_Pet, int>(2, true);
```

TODO 后续更新优化：
1. 添加单独导出 Excel 导表功能（现在是全部导出）
2. 优化获取导表数据时，需要填入主键的问题，让获取更简便
3. 支持更多的数据结构