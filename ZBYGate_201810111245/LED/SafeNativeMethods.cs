using System.Runtime.InteropServices;
using System.Security;

namespace ZBYGate_Data_Collection.LED
{
    [SuppressUnmanagedCodeSecurityAttribute]
    internal class SafeNativeMethods
    {
        /*-------------------------------------------------------------------------------
        过程名:    Initialize
        初始化动态库；该函数不与显示屏通讯。
        参数:
        返回值            :详见返回状态代码定义。
   -------------------------------------------------------------------------------*/
        [DllImport("LedDynamicArea.dll")]
        public static extern int Initialize(); //初始化动态库    

        /*-------------------------------------------------------------------------------
        过程名:    Uninitialize
        释放动态库；该函数不与显示屏通讯。
        参数:
        返回值            :详见返回状态代码定义。
        -------------------------------------------------------------------------------*/
        [DllImport("LedDynamicArea.dll")]
        public static extern int Uninitialize(); //释放动态库    

        /*-------------------------------------------------------------------------------
            过程名:    AddScreen_Dynamic
            向动态库中添加显示屏信息；该函数不与显示屏通讯。
            参数:
            nControlType:显示屏的控制器型号，目前该动态区域动态库只支持BX-5E1、BX-5E2、BX-5E3等BX-5E系列控制器。
            nScreenNo：显示屏屏号；该参数与LedshowTW 2013软件中"设置屏参"模块的"屏号"参数一致。
            nSendMode：通讯模式；目前动态库中支持0:串口通讯；2：网络通讯(只支持固定IP模式)；5：保存到文件等三种通讯模式。
            nWidth：显示屏宽度；单位：像素。
            nHeight：显示屏高度；单位：像素。
            nScreenType：显示屏类型；1：单基色；2：双基色。
            nPixelMode：点阵类型，只有显示屏类型为双基色时有效；1：R+G；2：G+R。
            pCom：通讯串口，只有在串口通讯时该参数有效；例："COM1"
            nBaud：通讯串口波特率，只有在串口通讯时该参数有效；目前只支持9600、57600两种波特率。
            pSocketIP：控制器的IP地址；只有在网络通讯(固定IP模式)模式下该参数有效，例："192.168.0.199"
            nSocketPort：控制器的端口地址；只有在网络通讯(固定IP模式)模式下该参数有效，例：5005
            nServerMode     :0:服务器模式未启动；1：服务器模式启动。
            pBarcode        :设备条形码，用于服务器模式和中转服务器
            pNetworkID      :网络ID编号，用于服务器模式和中转服务器
            pServerIP       :中转服务器IP地址
            nServerPort     :中转服务器网络端口
            pServerAccessUser:中转服务器访问用户名
            pServerAccessPassword:中转服务器访问密码
            pCommandDataFile：保存到文件方式时，命令保存命令文件名称。只有在保存到文件模式下该参数有效，例："curCommandData.dat"
            返回值:    详见返回状态代码定义。
        -------------------------------------------------------------------------------*/
        [DllImport("LedDynamicArea.dll")]
        public static extern int AddScreen_Dynamic(int nControlType, int nScreenNo, int nSendMode, int nWidth, int nHeight,
              int nScreenType, int nPixelMode, string pCom, int nBaud, string pSocketIP, int nSocketPort, int nStaticIpMode, int nServerMode,
              string pBarcode, string pNetworkID, string pServerIP, int nServerPort, string pServerAccessUser, string pServerAccessPassword,
              string pCommandDataFile);

        /*-------------------------------------------------------------------------------
          过程名:    QuerryServerDeviceList
          查询中转服务器设备的列表信息。
          该函数与显示屏进行通讯
          参数:
            pTransitDeviceType :中转设备类型 BX-3GPRS，BX-3G
            pServerIP       :中转服务器IP地址
            nServerPort     :中转服务器网络端口
            pServerAccessUser:中转服务器访问用户名
            pServerAccessPassword:中转服务器访问密码
            pDeviceList       : 保存查询的设备列表信息
              将设备的信息用组成字符串, 比如：
              设备1：名称 条形码 状态 类型 网络ID
              设备2：名称 条形码 状态 类型 网络ID
              组成字符串为：'设备1名称,设备1条形码,设备1状态,设备1类型,设备1网络ID;设备2名称,设备2条形码,设备2状态,设备2类型,设备2网络ID'
              设备状态(Byte):  $10:设备未知
                               $11:设备在线
                               $12:设备不在线

              设备类型(Byte): $16:设备为2G
                              $17:设备为3G
            pDeviceCount      : 查询的设备个数

          返回值            :详见返回状态代码定义。
        -------------------------------------------------------------------------------*/
        [DllImport("LedDynamicArea.dll")]
        public static extern int QuerryServerDeviceList(string pTransitDeviceType, string pServerIP, int nServerPort, string pServerAccessUser, string pServerAccessPassword,
                                                        byte[] pDeviceList, ref int nDeviceCount);

        /*-------------------------------------------------------------------------------
          过程名:    AddScreenDynamicArea
          向动态库中指定显示屏添加动态区域；该函数不与显示屏通讯。
          参数:
            nScreenNo：显示屏屏号；该参数与AddScreen函数中的nScreenNo参数对应。
            nDYAreaID：动态区域编号；目前系统中最多5个动态区域；该值取值范围为0~4;
            nRunMode：动态区域运行模式：
                      0:动态区数据循环显示；
                      1:动态区数据显示完成后静止显示最后一页数据；
                      2:动态区数据循环显示，超过设定时间后数据仍未更新时不再显示；
                      3:动态区数据循环显示，超过设定时间后数据仍未更新时显示Logo信息,Logo 信息即为动态区域的最后一页信息
                      4:动态区数据顺序显示，显示完最后一页后就不再显示
            nTimeOut：动态区数据超时时间；单位：秒 
            nAllProRelate：节目关联标志；
                      1：所有节目都显示该动态区域；
                      0：在指定节目中显示该动态区域，如果动态区域要独立于节目显示，则下一个参数为空。
            pProRelateList：节目关联列表，用节目编号表示；节目编号间用","隔开,节目编号定义为LedshowTW 2013软件中"P***"中的"***"
            nPlayImmediately：动态区域是否立即播放0：该动态区域与异步节目一起播放；1：异步节目停止播放，仅播放该动态区域
            nAreaX：动态区域起始横坐标；单位：像素
            nAreaY：动态区域起始纵坐标；单位：像素
            nAreaWidth：动态区域宽度；单位：像素
            nAreaHeight：动态区域高度；单位：像素
            nAreaFMode：动态区域边框显示标志；0：纯色；1：花色；255：无边框
            nAreaFLine：动态区域边框类型, 纯色最大取值为FRAME_SINGLE_COLOR_COUNT；花色最大取值为：FRAME_MULI_COLOR_COUNT
            nAreaFColor：边框显示颜色；选择为纯色边框类型时该参数有效；
            nAreaFStunt：边框运行特技；
                      0：闪烁；1：顺时针转动；2：逆时针转动；3：闪烁加顺时针转动；
                      4:闪烁加逆时针转动；5：红绿交替闪烁；6：红绿交替转动；7：静止打出
            nAreaFRunSpeed：边框运行速度；
            nAreaFMoveStep：边框移动步长；该值取值范围：1~8；
          返回值:    详见返回状态代码定义。
        -------------------------------------------------------------------------------*/
        [DllImport("LedDynamicArea.dll")]
        public static extern int AddScreenDynamicArea(int nScreenNo, int nDYAreaID, int nRunMode,
            int nTimeOut, int nAllProRelate, string pProRelateList, int nPlayImmediately,
            int nAreaX, int nAreaY, int nAreaWidth, int nAreaHeight, int nAreaFMode, int nAreaFLine, int nAreaFColor,
            int nAreaFStunt, int nAreaFRunSpeed, int nAreaFMoveStep);

        /*-------------------------------------------------------------------------------
          过程名: DeleteScreenDynamicArea
         * 删除动态库中指定显示屏指定的动态区域信息；该函数不与显示屏通讯。
        参数：
        nScreenNo	显示屏屏号；该参数与AddScreen_Dynamic函数中的nScreenNo参数对应。
        nDYAreaID	动态区域编号；该参数与AddScreenDynamicArea函数中的nDYAreaID参数对应*/
        [DllImport("LedDynamicArea.dll")]
        public static extern int DeleteScreenDynamicArea(int nScreenNo, int nDYAreaID);

        /*-------------------------------------------------------------------------------
          过程名:    AddScreenDynamicAreaFile
          向动态库中指定显示屏的指定动态区域添加信息文件；该函数不与显示屏通讯。
          参数:
            nScreenNo：显示屏屏号；该参数与AddScreen函数中的nScreenNo参数对应。
            nDYAreaID：动态区域编号；该参数与AddScreenDynamicArea函数中的nDYAreaID参数对应
            pFileName：添加的信息文件名称；目前只支持txt(支持ANSI、UTF-8、Unicode等格式编码)、bmp的文件格式
            nShowSingle：文字信息是否单行显示；0：多行显示；1：单行显示；显示该参数只有szFileName为txt格式文档时才有效；
            pFontName：文字信息显示字体；该参数只有szFileName为txt格式文档时才有效；
            nFontSize：文字信息显示字体的字号；该参数只有szFileName为txt格式文档时才有效；
            nBold：文字信息是否粗体显示；0：正常；1：粗体显示；该参数只有szFileName为txt格式文档时才有效；
            nFontColor：文字信息显示颜色；该参数只有szFileName为txt格式文档时才有效；
            nStunt：动态区域信息运行特技；
                      00：随机显示 
                      01：静止显示
                      02：快速打出 
                      03：向左移动 
                      04：向左连移 
                      05：向上移动 
                      06：向上连移 
                      07：闪烁 
                      08：飘雪 
                      09：冒泡 
                      10：中间移出 
                      11：左右移入 
                      12：左右交叉移入 
                      13：上下交叉移入 
                      14：画卷闭合 
                      15：画卷打开 
                      16：向左拉伸 
                      17：向右拉伸 
                      18：向上拉伸 
                      19：向下拉伸 
                      20：向左镭射 
                      21：向右镭射 
                      22：向上镭射 
                      23：向下镭射 
                      24：左右交叉拉幕 
                      25：上下交叉拉幕 
                      26：分散左拉 
                      27：水平百页 
                      28：垂直百页 
                      29：向左拉幕 
                      30：向右拉幕 
                      31：向上拉幕 
                      32：向下拉幕 
                      33：左右闭合 
                      34：左右对开 
                      35：上下闭合 
                      36：上下对开 
                      37：向右移动 
                      38：向右连移 
                      39：向下移动 
                      40：向下连移
            nRunSpeed：动态区域信息运行速度
            nShowTime：动态区域信息显示时间；单位：10ms
          返回值:    详见返回状态代码定义。
        -------------------------------------------------------------------------------*/
        [DllImport("LedDynamicArea.dll")]
        public static extern int AddScreenDynamicAreaFile(int nScreenNo, int nDYAreaID,
            string pFileName, int nShowSingle, int nAlignment, string pFontName, int nFontSize, int nBold, int nFontColor,
            int nStunt, int nRunSpeed, int nShowTime);

        /*-------------------------------------------------------------------------------
          过程名:    AddScreenDynamicAreaText
          向动态库中指定显示屏的指定动态区域添加信息文件；该函数不与显示屏通讯。
          参数:
            nScreenNo：显示屏屏号；该参数与AddScreen函数中的nScreenNo参数对应。
            nDYAreaID：动态区域编号；该参数与AddScreenDynamicArea函数中的nDYAreaID参数对应
            pText：添加的信息文件名称；目前只支持txt(支持ANSI、UTF-8、Unicode等格式编码)、bmp的文件格式
            nShowSingle：文字信息是否单行显示；0：多行显示；1：单行显示；显示该参数只有szFileName为txt格式文档时才有效；
            pFontName：文字信息显示字体；该参数只有szFileName为txt格式文档时才有效；
            nFontSize：文字信息显示字体的字号；该参数只有szFileName为txt格式文档时才有效；
            nBold：文字信息是否粗体显示；0：正常；1：粗体显示；该参数只有szFileName为txt格式文档时才有效；
            nFontColor：文字信息显示颜色；该参数只有szFileName为txt格式文档时才有效；
            nStunt：动态区域信息运行特技；
                      00：随机显示 
                      01：静止显示
                      02：快速打出 
                      03：向左移动 
                      04：向左连移 
                      05：向上移动 
                      06：向上连移 
                      07：闪烁 
                      08：飘雪 
                      09：冒泡 
                      10：中间移出 
                      11：左右移入 
                      12：左右交叉移入 
                      13：上下交叉移入 
                      14：画卷闭合 
                      15：画卷打开 
                      16：向左拉伸 
                      17：向右拉伸 
                      18：向上拉伸 
                      19：向下拉伸 
                      20：向左镭射 
                      21：向右镭射 
                      22：向上镭射 
                      23：向下镭射 
                      24：左右交叉拉幕 
                      25：上下交叉拉幕 
                      26：分散左拉 
                      27：水平百页 
                      28：垂直百页 
                      29：向左拉幕 
                      30：向右拉幕 
                      31：向上拉幕 
                      32：向下拉幕 
                      33：左右闭合 
                      34：左右对开 
                      35：上下闭合 
                      36：上下对开 
                      37：向右移动 
                      38：向右连移 
                      39：向下移动 
                      40：向下连移
            nRunSpeed：动态区域信息运行速度
            nShowTime：动态区域信息显示时间；单位：10ms
          返回值:    详见返回状态代码定义。
        -------------------------------------------------------------------------------*/
        [DllImport("LedDynamicArea.dll")]
        public static extern int AddScreenDynamicAreaText(int nScreenNo, int nDYAreaID,
            string pText, int nShowSingle, int nAlignment, string pFontName, int nFontSize, int nBold, int nFontColor,
            int nStunt, int nRunSpeed, int nShowTime);

        /*-------------------------------------------------------------------------------
          过程名:    DeleteScreen
          删除动态库中指定显示屏的所有信息；该函数不与显示屏通讯。
          参数:
            nScreenNo：显示屏屏号；该参数与AddScreen函数中的nScreenNo参数对应。
          返回值:    详见返回状态代码定义
        -------------------------------------------------------------------------------*/
        [DllImport("LedDynamicArea.dll")]
        public static extern int DeleteScreen_Dynamic(int nScreenNo);

        /*-------------------------------------------------------------------------------
          过程名:    DeleteScreenDynamicAreaFile
          删除动态库中指定显示屏指定的动态区域指定文件信息；该函数不与显示屏通讯。
          参数:
            nScreenNo：显示屏屏号；该参数与AddScreen函数中的nScreenNo参数对应。
            nDYAreaID：动态区域编号；该参数与AddScreenDynamicArea函数中的nDYAreaID参数对应
            nFileOrd：动态区域的指定文件的文件序号；该序号按照文件添加顺序，从0顺序递增，如删除中间的文件，后面的文件序号自动填充。
          返回值:    详见返回状态代码定义
        -------------------------------------------------------------------------------*/
        [DllImport("LedDynamicArea.dll")]
        public static extern int DeleteScreenDynamicAreaFile(int nScreenNo, int nDYAreaID, int nFileOrd);

        /*-------------------------------------------------------------------------------
         过程名:    SendDynamicAreasInfoCommand
         发送动态库中指定显示屏指定的动态区域信息到显示屏；该函数与显示屏通讯。
         参数:
           nScreenNo：显示屏屏号；该参数与AddScreen函数中的nScreenNo参数对应。
         * nDelAllDYArea	动态区域编号列表；1：同时发送多个动态区域，0：发送单个动态区域
           pDYAreaIDList	动态区域编号；当nDelAllDYArea为1时，其值为""；当nDelAllDYArea为0时，该参数与AddScreenDynamicArea函数中的nDYAreaID参数对应，发送相应动态区域
          返回值:    详见返回状态代码定义
       -------------------------------------------------------------------------------*/
        [DllImport("LedDynamicArea.dll")]
        public static extern int SendDynamicAreasInfoCommand(int nScreenNo, int nDelAllDYArea, string pDYAreaIDList);

        /*-------------------------------------------------------------------------------
          过程名:    SendDeleteDynamicAreasCommand
          删除动态库中指定显示屏指定的动态区域信息；同时向显示屏通讯删除指定的动态区域信息。该函数与显示屏通讯
          参数:
            nScreenNo：显示屏屏号；该参数与AddScreen函数中的nScreenNo参数对应。
            nDelAllDYArea	动态区域编号列表；1：同时删除多个动态区域，0：删除单个动态区域
            pDYAreaIDList	动态区域编号；当nDelAllDYArea为1时，其值为""；当nDelAllDYArea为0时，该参数与AddScreenDynamicArea函数中的nDYAreaID参数对应，删除相应动态区域
          返回值:    详见返回状态代码定义
        -------------------------------------------------------------------------------*/
        [DllImport("LedDynamicArea.dll")]
        public static extern int SendDeleteDynamicAreasCommand(int nScreenNo, int nDelAllDYArea, string pDYAreaIDList); //删除指定动态区域的信息

        /*-------------------------------------------------------------------------------
          过程名:    StartServer
          启动服务器,用于网络模式下的服务器模式和GPRS通讯模式。
          参数:
            nSendMode       :与显示屏的通讯模式；
              0:串口模式、BX-5A2&RF、BX-5A4&RF等控制器为RF串口无线模式;
              1:GPRS模式
              2:网络模式
              4:WiFi模式
              5:ONBON服务器-GPRS
              6:ONBON服务器-3G
            pServerIP       :中转服务器IP地址
            nServerPort     :中转服务器网络端口
          返回值            :详见返回状态代码定义。
        -------------------------------------------------------------------------------*/
        [DllImport("LedDynamicArea.dll")]
        public static extern int StartServer(int nSendMode, string pServerIP, int nServerPort);

        /*-------------------------------------------------------------------------------
          过程名:    StopServer
          关闭服务器,用于网络模式下的服务器模式和GPRS通讯模式。
          参数:
            nSendMode       :与显示屏的通讯模式；
              0:串口模式、BX-5A2&RF、BX-5A4&RF等控制器为RF串口无线模式;
              1:GPRS模式
              2:网络模式
              4:WiFi模式
              5:ONBON服务器-GPRS
              6:ONBON服务器-3G
          返回值            :详见返回状态代码定义。
         * 
        -------------------------------------------------------------------------------*/
        [DllImport("LedDynamicArea.dll")]
        public static extern int QuerryServerOnlineList(byte[] pDeviceList);
        [DllImport("LedDynamicArea.dll")]
        public static extern int StopServer(int nSendMode);
    }
}
