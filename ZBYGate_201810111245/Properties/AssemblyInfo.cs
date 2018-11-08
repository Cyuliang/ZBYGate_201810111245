using System.Resources;
using System.Reflection;
using System.Runtime.InteropServices;

// 有关程序集的一般信息由以下
// 控制。更改这些特性值可修改
// 与程序集关联的信息。
[assembly: AssemblyTitle("ZBYGate_Data_Collection")]
[assembly: AssemblyDescription("闸口数据采集处理")]
[assembly: AssemblyConfiguration("")]
[assembly: AssemblyCompany("深圳众百源科技有限公司")]
[assembly: AssemblyProduct("ZBYGate_Data_Collection")]
[assembly: AssemblyCopyright("Copyright ©  2018")]
[assembly: AssemblyTrademark("百识科技")]
[assembly: AssemblyCulture("")]

// 将 ComVisible 设置为 false 会使此程序集中的类型
//对 COM 组件不可见。如果需要从 COM 访问此程序集中的类型
//请将此类型的 ComVisible 特性设置为 true。
[assembly: ComVisible(false)]

// 如果此项目向 COM 公开，则下列 GUID 用于类型库的 ID
[assembly: Guid("94f86c57-54bd-48e1-aa9f-c737e7289d84")]

// 程序集的版本信息由下列四个值组成: 
//
//      主版本
//      次版本
//      生成号
//      修订号
//
// 可以指定所有值，也可以使用以下所示的 "*" 预置版本号和修订号
// 方法是按如下所示使用“*”: :
// [assembly: AssemblyVersion("1.0.*")]
[assembly: AssemblyVersion("1.0.0.1")]
[assembly: AssemblyFileVersion("1.0.0.0")]
[assembly: log4net.Config.XmlConfigurator(ConfigFile = "Log\\Log4net.config", Watch = true)]
[assembly: NeutralResourcesLanguage("zh-Hans")]

