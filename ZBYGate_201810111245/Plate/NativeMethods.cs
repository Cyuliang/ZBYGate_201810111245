using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace ZBYGate_Data_Collection.Plate
{
    public partial class NativeConstants
    {
        // public const string _CLIENT_H = "";      
        public const int CLIENT_LPRC_BIG_PICSTREAM_SIZE = (200000 - 312);   /*相机上传jpeg流每帧占用的内存的最大大小*/

        public const int CLIENT_LPRC_BIG_PICSTREAM_SIZE_EX = (1
                    * 800
                    * 1024 - 312);                                       /*相机上传全景图占用内存的最大大小		*/
        public const int CLIENT_LPRC_SMALL_PICSTREAM_SIZE_EX = 10000;       /*相机上传车牌截图占用内存的最大大小	*/
    }
    /* 相机时间 */
    [System.Runtime.InteropServices.StructLayoutAttribute(System.Runtime.InteropServices.LayoutKind.Sequential)]
    public struct CLIENT_LPRC_CAMERA_TIME
    {
        public int Year;
        public int Month;
        public int Day;
        public int Hour;
        public int Minute;
        public int Second;
        public int Millisecond;
    }
    /* 识别结果坐标 */
    [System.Runtime.InteropServices.StructLayoutAttribute(System.Runtime.InteropServices.LayoutKind.Sequential)]
    public struct CLIENT_LPRC_PLATE_LOCATION
    {
        public int Left;
        public int Top;
        public int Right;
        public int Bottom;
    }

    /* 图像信息*/
    [System.Runtime.InteropServices.StructLayoutAttribute(System.Runtime.InteropServices.LayoutKind.Sequential, CharSet = System.Runtime.InteropServices.CharSet.Ansi)]
    public struct CLIENT_LPRC_IMAGE_INFO
    {
        public int nWidth;		/* 宽度					*/
        public int nHeight;		/* 高度					*/
        public int nPitch;		/* 图像宽度的一行像素所占内存字节数*/
        public int nLen;		/* 图像的长度			*/
        [System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.ByValTStr, SizeConst = 16)]
        public string reserved; /* 预留     			*/
                                /* 图像内存的首地址		*/
        public System.IntPtr pBuffer;
    }

    /* 识别结果 */
    [System.Runtime.InteropServices.StructLayoutAttribute(System.Runtime.InteropServices.LayoutKind.Sequential, CharSet = System.Runtime.InteropServices.CharSet.Ansi)]
    public struct CLIENT_LPRC_PLATE_RESULTEX
    {
        /* 相机IP           */
        [System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.ByValTStr, SizeConst = 16)]
        public string chCLIENTIP;
        /* 车牌颜色 		*/
        [System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.ByValTStr, SizeConst = 8)]
        public string chColor;
        /* 车牌号码 		*/
        [System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.ByValTStr, SizeConst = 16)]
        public string chLicense;
        /* 车牌在图像中的坐标 	*/
        public CLIENT_LPRC_PLATE_LOCATION pcLocation;
        /* 识别出车牌的时间 	*/
        public CLIENT_LPRC_CAMERA_TIME shootTime;
        /* 车牌可信度			*/
        public int nConfidence;
        /* 识别耗时				*/
        public int nTime;
        /* 车牌运动方向，0 unknown, 1 left, 2 right, 3 up, 4 down */
        public int nDirection;
        /*预留*/
        [System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.ByValTStr, SizeConst = 256)]
        public string reserved;
        /* 全景图像数据(注意：相机不传输，此处指针为空) */
        public CLIENT_LPRC_IMAGE_INFO pFullImage;
        /* 车牌图像数据(注意：相机不传输，此处指针为空) */
        public CLIENT_LPRC_IMAGE_INFO pPlateImage;
    }


    [System.Runtime.InteropServices.StructLayoutAttribute(System.Runtime.InteropServices.LayoutKind.Sequential, CharSet = System.Runtime.InteropServices.CharSet.Ansi)]
    public struct CLIENT_LPRC_DeviceInfo
    {

        /// char[256]
        [System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.ByValTStr, SizeConst = 256)]
        public string chDevName;

        /// char[20]
        [System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.ByValTStr, SizeConst = 20)]
        public string chSoftVer;

        /// char[20]
        [System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.ByValTStr, SizeConst = 20)]
        public string chHardVer;

        /// char[20]
        [System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.ByValTStr, SizeConst = 20)]
        public string chSysVer;

        /// int
        public int nSdkPort;

        /// char[16]
        [System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.ByValTStr, SizeConst = 16)]
        public string chIp;

        /// char[16]
        [System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.ByValTStr, SizeConst = 16)]
        public string chGateway;

        /// char[16]
        [System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.ByValTStr, SizeConst = 16)]
        public string chNetmask;

        /// char[18]
        [System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.ByValTStr, SizeConst = 18)]
        public string chMac;

        /// char[20]
        [System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.ByValTStr, SizeConst = 20)]
        public string chRoomID;

        /// char[20]
        [System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.ByValTStr, SizeConst = 20)]
        public string chSN;

        /// char[256]
        [System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.ByValTStr, SizeConst = 256)]
        public string reserved;
    }



    /* Jpeg流回调返回每一帧jpeg数据结构体 */
    [System.Runtime.InteropServices.StructLayoutAttribute(System.Runtime.InteropServices.LayoutKind.Sequential, CharSet = System.Runtime.InteropServices.CharSet.Ansi)]
    public struct CLIENT_LPRC_DEVDATA_INFO
    {
        [System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.ByValTStr, SizeConst = 16)]
        public string chIp;
        public System.IntPtr pchBuf;
        public uint nLen;
        public int nStatus;
        [System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.ByValTStr, SizeConst = 128)]
        public string reserved;
    }

    /*接收串口数据的结构体*/
    [System.Runtime.InteropServices.StructLayoutAttribute(System.Runtime.InteropServices.LayoutKind.Sequential, CharSet = System.Runtime.InteropServices.CharSet.Ansi)]
    public struct CLIENT_LPRC_DEVSERIAL_DATA
    {
        /*串口数据指针
        [System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.LPStr)]
        public string pData;*/
        public System.IntPtr pData;
        /*串口数据大小*/
        public int nsize;
        /* 保留*/
        [System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.ByValTStr, SizeConst = 128)]
        public string reserved;
    }

    /************************************************************************/
    /* 回调函数: 通知相机设备通讯状态的回调函数								*/
    /*		Parameters:														*/
    /*			chWTYIP[out]:		返回设备IP								*/
    /*			nStatus[out]:		设备状态：0表示网络异常或设备异常；		*/
    /*										  1表示网络正常，设备已连接		*/
    /*		Return Value:   void											*/
    /************************************************************************/
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate void CLIENT_LPRC_ConnectCallback(System.IntPtr chCLIENTIP, uint nStatus, uint dwUser);

    /***********************************************************************************/
    /* 回调函数:获取相机485发送的数据						      					   */
    /*		Parameters:														           */
    /*			chCLIENTIP[out]:		返回设备IP								       */
    /*          serialData[out]          串口数据地址								   */
    /*			nlen[out]				串口数据大小								   */
    /*          dwUser[out]            CLIENT_LPRC_InitSDK传给sdk的用户自定义字段      */
    /*		Return Value:   void											           */
    /***********************************************************************************/
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate void CLIENT_LPRC_SerialDataCallback(System.IntPtr chCLIENTIP, ref CLIENT_LPRC_DEVSERIAL_DATA pSerialData, uint dwUser);
    /************************************************************************/
    /*--------------------------7.1.6.0新增---------------------------------*/
    /* 回调函数: 获取识别结果的回调函数										*/
    /*		Parameters:														*/
    /*			recResult[out]:		识别结果数据							*/
    /*		Return Value:   void											*/
    /************************************************************************/
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate void CLIENT_LPRC_DataEx2Callback(ref CLIENT_LPRC_PLATE_RESULTEX recResultEx, uint dwUser);
    /************************************************************************/
    /* 回调函数: 获取Jpeg流的回调函数										*/
    /*		Parameters:														*/
    /*			JpegInfo[out]:		JPEG流数据信息							*/
    /*		Return Value:   void											*/
    /*																		*/
    /*		Notice:															*/
    /*			一台PC连接多台设备时，此函数仅需实现一次。当区分不同设备	*/
    /*			的JPEG流时，可以通过输出参数中KHT_DevData中的chIp来区分		*/
    /*																		*/
    /************************************************************************/
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate void CLIENT_LPRC_JpegCallback(ref CLIENT_LPRC_DEVDATA_INFO JpegInfo, uint dwUser);
    /************************************************************************/
    /* 回调函数: 获取报警信息的回调函数										*/
    /*		Parameters:														*/
    /*			alarmInfo[out]:		报警信息								*/
    /*		Return Value:   void											*/
    /*																		*/
    /*		Notice:															*/
    /*			一台PC连接多台设备时，此函数仅需实现一次。当区分不同设备	*/
    /*			的Alarm时，可以通过输出参数中KHT_DevData中的chIp来区分		*/
    /*																		*/
    /************************************************************************/
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate void CLIENT_LPRC_AlarmCallback(ref CLIENT_LPRC_DEVDATA_INFO alarmInfo, uint dwUser);

    [System.Runtime.InteropServices.StructLayoutAttribute(System.Runtime.InteropServices.LayoutKind.Sequential)]
    public struct HWND__
    {

        /// int
        public int unused;
    }

    internal class NativeMethods
    {
        /************************************************************************/
        /* CLIENT_LPRC_InitSDK: 连接相机										*/
        /*		Parameters:														*/
        /*			nPort[in]:		连接相机的端口，现默认为8080				*/
        /*			hWndHandle[in]:	接收消息的窗体句柄，当为NULL时，表示无窗体  */
        /*			uMsg[in]:		用户自定义消息，当hWndHandle不为NULL时，	*/
        /*							检测到有新的车牌识别结果并准备好当前车牌	*/
        /*							缓冲区信息后，用::PostMessage 给窗口		*/
        /*							hWndHandle发送uMsg消息，其中WPARAM参数为0，	*/
        /*							LPARAM参数为0								*/
        /*			chServerIP[in]:	相机的IP地址								*/
        /*          dwUser[in]:     用户自定义字段，主要用来回传给回调函数。    */
        /*		Return Value:   int												*/
        /*							0	相机连接成功							*/
        /*							1	相机连接失败							*/
        /*		Notice:   														*/
        /*				如果采用回调的方式获取数据时，hWndHandle句柄为NULL，	*/
        /*				uMsg为0，并且注册回调函数，通知有新的数据；				*/
        /*				反之，在主窗口收到消息时，调用CLIENT_LPRC_GetVehicleInfoEx获取*/
        /*				数据。													*/
        /************************************************************************/
        [System.Runtime.InteropServices.DllImportAttribute("WTY.dll", EntryPoint = "CLIENT_LPRC_InitSDK", CallingConvention = System.Runtime.InteropServices.CallingConvention.StdCall)]
        public static extern int CLIENT_LPRC_InitSDK(uint nPort, System.IntPtr hWndHandle, uint uMsg, System.IntPtr chServerIP, uint dwUser);

        /************************************************************************/
        /* CLIENT_LPRC_QuitSDK: 断开所有已经连接设备，释放资源					*/
        /*		Parameters:														*/
        /*		Return Value:   void											*/
        /************************************************************************/
        [System.Runtime.InteropServices.DllImportAttribute("WTY.dll", EntryPoint = "CLIENT_LPRC_QuitSDK", CallingConvention = System.Runtime.InteropServices.CallingConvention.StdCall)]
        public static extern void CLIENT_LPRC_QuitSDK();


        /************************************************************************/
        /* CLIENT_LPRC_RegCLIENTConnEvent: 注册相机通讯状态的回调函数			*/
        /*		Parameters:														*/
        /*			CLIENTConnect[in]:		CLIENT_LPRC_ConnectCallback类型回调函数*/
        /*		Return Value:   void											*/
        /************************************************************************/
        [System.Runtime.InteropServices.DllImportAttribute("WTY.dll", EntryPoint = "CLIENT_LPRC_RegCLIENTConnEvent", CallingConvention = System.Runtime.InteropServices.CallingConvention.StdCall)]
        public static extern void CLIENT_LPRC_RegCLIENTConnEvent(CLIENT_LPRC_ConnectCallback CLIENTConnect);


        /************************************************************************/
        /* CLIENT_LPRC_CheckStatus: 主动检查相机设备的通讯状态					*/
        /*		Parameters:														*/
        /*			chCLIENTIP[in]:		要检查的相机的IP						*/
        /*		Return Value:   int												*/
        /*							0	正常									*/
        /*							1	网络不通								*/
        /************************************************************************/
        [System.Runtime.InteropServices.DllImportAttribute("WTY.dll", EntryPoint = "CLIENT_LPRC_CheckStatus", CallingConvention = System.Runtime.InteropServices.CallingConvention.StdCall)]
        public static extern int CLIENT_LPRC_CheckStatus(System.IntPtr chCLIENTIP);


        /************************************************************************/
        /* CLIENT_LPRC_RegSerialDataEvent: 注册获取串口数据的回调函数           */
        /*		Parameters:														*/
        /*			CLIENTSerialData[in]:		处理接收串口数据的回调函数的指针*/
        /*		Return Value:   void											*/
        /************************************************************************/
        [System.Runtime.InteropServices.DllImportAttribute("WTY.dll", EntryPoint = "CLIENT_LPRC_RegSerialDataEvent", CallingConvention = System.Runtime.InteropServices.CallingConvention.StdCall)]
        public static extern void CLIENT_LPRC_RegSerialDataEvent(CLIENT_LPRC_SerialDataCallback CLIENTSerialData);


        /************************************************************************/
        /* CLIENT_LPRC_RegDataEx2Event: 注册获取识别结果的回调函数              */
        /*		Parameters:														*/
        /*			CLIENTData[in]:		处理识别结果的回调函数的指针			*/
        /*		Return Value:   void											*/
        /*	Note:																*/
        /*		接收清晰度较高，或分辨率较高的JPEG图像							*/
        /************************************************************************/
        [System.Runtime.InteropServices.DllImportAttribute("WTY.dll", EntryPoint = "CLIENT_LPRC_RegDataEx2Event", CallingConvention = System.Runtime.InteropServices.CallingConvention.StdCall)]
        public static extern void CLIENT_LPRC_RegDataEx2Event(CLIENT_LPRC_DataEx2Callback CLIENTDataEx2);


        /************************************************************************/
        /* 	函数: 消息方式获取指定IP的相机识别结果。							*/
        /*		  当CLIENT_LPRC_initSDK函数中设置了窗体句柄和消息时，           */
        /*		  需要在消息处理函数中调用此函数来主动获取识别结果。			*/
        /*		Parameters:														*/
        /*			chCLIENTIP[in]:		根据消息，获取指定IP设备识别数据		*/
        /*			chPlate[in]:		车牌号码								*/
        /*			chColor[in]:		车牌颜色								*/
        /*			chFullImage[in]:	全景图数据								*/
        /*			nFullLen[in]:		全景图数据长度							*/
        /*			chPlateImage[in]:	车牌图数据								*/
        /*			nPlateLen[in]:		车牌图数据长度							*/
        /*		Return Value:   int												*/
        /*							0	获取成功								*/
        /*							1	获取失败								*/
        /*		Notice:   														*/
        /*			当设置了传输内容不传时，各自对应的数据为NULL，长度为-1；	*/
        /*			当没有形成数据时，各自对应数据为NULL，长度为0				*/
        /************************************************************************/
        [System.Runtime.InteropServices.DllImportAttribute("WTY.dll", EntryPoint = "CLIENT_LPRC_GetVehicleInfoEx", CallingConvention = System.Runtime.InteropServices.CallingConvention.StdCall)]
        public static extern int CLIENT_LPRC_GetVehicleInfoEx(System.IntPtr chCLIENTIP, System.IntPtr chPlate, System.IntPtr chColor, System.IntPtr chFullImage, ref int nFullLen, System.IntPtr chPlateImage, ref int nPlateLen);


        /************************************************************************/
        /* CLIENT_LPRC_SetSavePath: 如果用户需要动态库自动保存图片，可以通过该函*/
        /* 数设置保存图片的路径。										        */
        /*		Parameters:														*/
        /*			chSavePath[in]:	文件存储路径，以"\\"结束，如："D:\\Image\\"	*/
        /*		Return Value:   void											*/
        /*																		*/
        /*		Notice:   														*/
        /*			全景图：指定目录\\设备IP\\年月日（YYYYMMDD）\\FullImage\\	*/
        /*									时分秒-毫秒__颜色_车牌号码__.jpg	*/
        /*			车牌图：指定目录\\设备IP\\年月日（YYYYMMDD）\\PlatelImage\\	*/
        /*									时分秒-毫秒__颜色_车牌号码__.jpg	*/
        /************************************************************************/
        [System.Runtime.InteropServices.DllImportAttribute("WTY.dll", EntryPoint = "CLIENT_LPRC_SetSavePath", CallingConvention = System.Runtime.InteropServices.CallingConvention.StdCall)]
        public static extern void CLIENT_LPRC_SetSavePath(System.IntPtr chSavePath);


        /************************************************************************/
        /* CLIENT_LPRC_SetTrigger: 触发识别										*/
        /*		Parameters:														*/
        /*			pCameraIP[in]:			要触发的相机设备的IP				*/
        /*			nCameraPort[in]:		端口,默认8080						*/
        /*		Return Value:													*/
        /*					0	触发成功返回									*/
        /*				  非0	失败											*/
        /************************************************************************/
        [System.Runtime.InteropServices.DllImportAttribute("WTY.dll", EntryPoint = "CLIENT_LPRC_SetTrigger", CallingConvention = System.Runtime.InteropServices.CallingConvention.StdCall)]
        public static extern int CLIENT_LPRC_SetTrigger(System.IntPtr pCameraIP, int nCameraPort);


        /************************************************************************/
        /* CLIENT_LPRC_SetTransContent: 控制相机设备上传内容					*/
        /*		Parameters:														*/
        /*			pCameraIP[in]:		要设置的设备IP							*/
        /*			nCameraPort[in]:	端口,默认8080							*/
        /*			nFullImg[in]:		全景图，0表示不传，1表示传				*/
        /*			nPlateImg[in]:		车牌图，0表示不传，1表示传				*/
        /*		Return Value:   int												*/
        /*							0	成功									*/
        /*						  非0	失败									*/
        /************************************************************************/
        [System.Runtime.InteropServices.DllImportAttribute("WTY.dll", EntryPoint = "CLIENT_LPRC_SetTransContent", CallingConvention = System.Runtime.InteropServices.CallingConvention.StdCall)]
        public static extern int CLIENT_LPRC_SetTransContent(System.IntPtr pCameraIP, int nCameraPort, int nFullImg, int nPlateImg);


        /************************************************************************/
        /* 函数说明: 控制继电器的闭合											*/
        /*		Parameters:														*/
        /*			pCameraIP[in]:			相机IP								*/
        /*			nCameraPort[in]:		端口,默认9110						*/
        /*		Return Value:   int												*/
        /*							0	设置成功								*/
        /*						  非0	失败									*/
        /*		Notice:   														*/
        /*				通过此功能，可以在PC端通过一体机设备，来控制道闸的抬起	*/
        /*				设备继电器输出信号为：开关量信号。						*/
        /************************************************************************/
        [System.Runtime.InteropServices.DllImportAttribute("WTY.dll", EntryPoint = "CLIENT_LPRC_SetRelayClose", CallingConvention = System.Runtime.InteropServices.CallingConvention.StdCall)]
        public static extern int CLIENT_LPRC_SetRelayClose(System.IntPtr pCameraIP, int nCameraPort);


        /************************************************************************/
        /* CLIENT_LPRC_RegJpegEvent: 注册获取Jpeg流的回调函数					*/
        /*		Parameters:														*/
        /*			JpegInfo[in]:		CLIENT_LPRC_JpegCallback类型回调函数	*/
        /*		Return Value:   void											*/
        /*																		*/
        /*		Notice:															*/
        /*			1:一台PC连接多台设备时，此函数仅需实现一次。当区分不同		*/
        /*	设备的JPEG流时，可以通过输出参数中CLIENT_LPRC_DEVDATA_INFO中的chIp来*/
        /*	区分.													        	*/
        /*			2:此功能目前适用于V5.5.3.0、V6.0.0.0及以上版本,				*/
        /*			  V5.2.1.0、V5.2.2.0、V5.2.6.0等版本不能使用此功能			*/
        /************************************************************************/
        [System.Runtime.InteropServices.DllImportAttribute("WTY.dll", EntryPoint = "CLIENT_LPRC_RegJpegEvent", CallingConvention = System.Runtime.InteropServices.CallingConvention.StdCall)]
        public static extern void CLIENT_LPRC_RegJpegEvent(CLIENT_LPRC_JpegCallback JpegInfo);


        /************************************************************************/
        /* CLIENT_LPRC_RegAlarmEvent: 注册获取报警信息的回调函数				*/
        /*		Parameters:														*/
        /*			AlarmInfo[in]:		CLIENT_LPRC_AlarmCallback类型回调函数	*/
        /*		Return Value:   void											*/
        /*																		*/
        /*		Notice:															*/
        /*			1:一台PC连接多台设备时，此函数仅需实现一次。当区分不同		*/
        /*	设备的报警信息时，可以通过输出参数中LPRC_CLIENT_DEVDATA_INFO中的chIp来*/
        /*	区分.														        */
        /*			2:此功能目前适用于V5.5.3.0、V6.0.0.0及以上版本,				*/
        /*			  V5.2.1.0、V5.2.2.0、V5.2.6.0等版本不能使用此功能			*/
        /************************************************************************/
        [System.Runtime.InteropServices.DllImportAttribute("WTY.dll", EntryPoint = "CLIENT_LPRC_RegAlarmEvent", CallingConvention = System.Runtime.InteropServices.CallingConvention.StdCall)]
        public static extern void CLIENT_LPRC_RegAlarmEvent(CLIENT_LPRC_AlarmCallback AlarmInfo);

        /************************************************************************/
        /* CLIENT_LPRC_RS485Send: RS485透明传输									*/
        /*		Parameters:														*/
        /*			pCameraIP[in]				相机设备IP地址					*/
        /*			nPort[in]					端口,默认9110					*/
        /*			chData[in]					将要传输的数据块的首地址		*/
        /*			nSendLen[in]				将要传输的数据块的字节数		*/
        /*		Return Value:   int												*/
        /*							0	成功									*/
        /*						  非0	失败									*/
        /*		notice：														*/
        /*				1：用户通过此接口，往相机发送数据，相机设备会原样将数据	*/
        /*				通过RS485接口转发出去，到客户所接的外部设备上。			*/
        /*				2：使用此功能前，需要在演示DEMO的设置界面上，设置相机不 */
        /*				能传输识别结果(默认S485传输识别结果)。					*/
        /************************************************************************/
        [System.Runtime.InteropServices.DllImportAttribute("WTY.dll", EntryPoint = "CLIENT_LPRC_RS485Send", CallingConvention = System.Runtime.InteropServices.CallingConvention.StdCall)]
        public static extern int CLIENT_LPRC_RS485Send(System.IntPtr pCameraIP, int nCameraPort, System.IntPtr chData, int nSendLen);


        /************************************************************************/
        /* 函数: Jpeg流消息处理初始化											*/
        /*		Parameters:														*/
        /*			hWndHandle[in]:	接收消息的窗体句柄							*/
        /*			uMsg[in]:		用户自定义消息								*/
        /*							检测到有数据并准备好缓冲区数据后，			*/
        /*							用::PostMessage 给窗口hWndHandle发送uMsg	*/
        /*							消息，其中WPARAM参数为0，LPARAM参数为0		*/
        /*			chIp[in]:		相机IP地址							    	*/
        /*		Return Value:   int												*/
        /*							0	获取成功								*/
        /*							1	获取失败								*/
        /************************************************************************/
        [System.Runtime.InteropServices.DllImportAttribute("WTY.dll", EntryPoint = "CLIENT_LPRC_JpegMessageInit", CallingConvention = System.Runtime.InteropServices.CallingConvention.StdCall)]
        public static extern int CLIENT_LPRC_JpegMessageInit(System.IntPtr hWndHandle, uint uMsg, System.IntPtr chIp);


        /************************************************************************/
        /* 	函数: 消息方式获取指定IP的相机的Jpeg流数据							*/
        /*		Parameters:														*/
        /*			chIp[in]:			相机IP地址								*/
        /*			chJpegBuf[in]:		存储JPEG的buffer						*/
        /*			nJpegBufLen[in]:	获取到的JPEG数据长度					*/
        /*		Return Value:   int												*/
        /*							0	获取成功								*/
        /*							1	获取失败								*/
        /*		Notice:   														*/
        /*			使用此函数前需先调用CLIENT_JpegMessageInit函数设置自定义消息*/
        /************************************************************************/
        [System.Runtime.InteropServices.DllImportAttribute("WTY.dll", EntryPoint = "CLIENT_LPRC_GetJpegStream", CallingConvention = System.Runtime.InteropServices.CallingConvention.StdCall)]
        public static extern int CLIENT_LPRC_GetJpegStream(System.IntPtr chIp, System.IntPtr chJpegBuf, System.IntPtr nJpegBufLen);


        /************************************************************************/
        /* 	函数: 根据IP地址，断开指定设备链接									*/
        /*		Parameters:														*/
        /*			pCameraIP[in]:			相机IP地址							*/
        /*		Return Value:   int												*/
        /*							0	获取成功								*/
        /*							1	获取失败								*/
        /************************************************************************/
        [System.Runtime.InteropServices.DllImportAttribute("WTY.dll", EntryPoint = "CLIENT_LPRC_QuitDevice", CallingConvention = System.Runtime.InteropServices.CallingConvention.StdCall)]
        public static extern int CLIENT_LPRC_QuitDevice(System.IntPtr pCameraIP);


        /************************************************************************/
        /* CLIENT_LPRC_SetNetworkCardBind: 手动绑定指定网卡IP					*/
        /*		Parameters:														*/
        /*			pCameraIP[in]		要绑定的网卡IP地址						*/
        /*		Return Value:   int												*/
        /*							0	成功									*/
        /*						  非0	失败									*/
        /*		notice:当PC机存在多网卡的情况时，又不想禁用为单网卡时，可通过该	*/
        /*				函数绑定与相机通讯的网卡IP								*/
        /************************************************************************/
        [System.Runtime.InteropServices.DllImportAttribute("WTY.dll", EntryPoint = "CLIENT_LPRC_SetNetworkCardBind", CallingConvention = System.Runtime.InteropServices.CallingConvention.StdCall)]
        public static extern int CLIENT_LPRC_SetNetworkCardBind(System.IntPtr pCameraIP);


        /*************************************************************************/
        /*CLIENT_LPRC_SnapJpegFrame 快速抓拍一帧，两种保存方式，直接保存到固定目录或者保存到特定内存,要是保存特定内存模式需要传入内存最大值,两种方式可选*/
        /*		Parameters:														*/
        /*			chIp[in]		   相机的IP地址						        */
        /*			pSaveFileName[in]  路径和带JPEG后缀名的文件名，用于把当前抓拍到的帧保存为特定文件  默认先匹配文件名	*/
        /*          pSaveBuf[in]       用于保存当前帧在特定内存的,并且需要传输内存可存储的最大值，当文件名为空的时候这个才会生效。*/
        /*          Maxlen[in]         保存当前帧特定内存的最大值               */
        /*		Return Value:   int												*/
        /*						   0	保存到特定目录成功						*/
        /*                         >0   保存到特定内存的数据的实际大小          */
        /*						  -1	失败									*/
        /************************************************************************/
        [System.Runtime.InteropServices.DllImportAttribute("WTY.dll", EntryPoint = "CLIENT_LPRC_SnapJpegFrame", CallingConvention = System.Runtime.InteropServices.CallingConvention.StdCall)]
        public static extern int CLIENT_LPRC_SnapJpegFrame(System.IntPtr chIp, System.IntPtr pSaveFileName, System.IntPtr pSaveBuf, int Maxlen);


        /************************************************************************/
        /* CLIENT_LPRC_SetJpegStreamPlayOrStop: 设置jpeg流的开关				*/
        /*		Parameters:														*/
        /*		pCameraIP[in]		需要设置的相机设备的ip地址				    */
        /*		onoff[in]			jpeg流开关项，0表示关闭流，1表示打开流 2打开H264*/
        /*		Return Value:   	int											*/
        /*							0	成功									*/
        /*						  	非0	失败									*/
        /************************************************************************/
        [System.Runtime.InteropServices.DllImportAttribute("WTY.dll", EntryPoint = "CLIENT_LPRC_SetJpegStreamPlayOrStop", CallingConvention = System.Runtime.InteropServices.CallingConvention.StdCall)]
        public static extern int CLIENT_LPRC_SetJpegStreamPlayOrStop(System.IntPtr pCameraIP, int onoff);


        /************************************************************************/
        /* CLIENT_LPRC_SetDevTimeParam:    修改设备系统时间					    */
        /*		Parameters:														*/
        /*		pCameraIP[in]		需要修改的相机设备的ip地址				    */
        /*		sysTime[in]			设置时间结构体		                        */
        /*		Return Value:   	int											*/
        /*							==0	成功									*/
        /*						  	非0	失败									*/
        /************************************************************************/
        [System.Runtime.InteropServices.DllImportAttribute("WTY.dll", EntryPoint = "CLIENT_LPRC_SetDevTimeParam", CallingConvention = System.Runtime.InteropServices.CallingConvention.StdCall)]
        public static extern int CLIENT_LPRC_SetDevTimeParam(System.IntPtr pCameraIP, ref CLIENT_LPRC_CAMERA_TIME sysTime);

        /************************************************************************/
        /* CLIENT_LPRC_SearchDeviceList:    搜索设备IP列表						*/
        /*		Parameters:														*/
        /*		pBuf[out]			存储搜索到的相机列表信息结构体数组		    */
        /*		Return Value:   	int											*/
        /*							大于0	成功搜索到的设备数					*/
        /*						  	-1	失败									*/
        /************************************************************************/
        [System.Runtime.InteropServices.DllImportAttribute("WTY.dll", EntryPoint = "CLIENT_LPRC_SearchDeviceList", CallingConvention = System.Runtime.InteropServices.CallingConvention.StdCall)]
        public static extern int CLIENT_LPRC_SearchDeviceList(ref CLIENT_LPRC_DeviceInfo pBuf);
    }
}
