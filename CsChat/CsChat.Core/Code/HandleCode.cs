using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CsChat.Code
{
    public enum HandleCode
    {
        None = 0,

        /// <summary>
        /// 心跳
        /// </summary>
        Heartbeat = 1,

        #region 控制台事件
        /// <summary>
        /// 控制台授权
        /// </summary>
        Console_Authorize = 1101,

        /// <summary>
        /// 开始导入IMEI
        /// </summary>
        Console_Import_IMEI_Start = 1111,

        /// <summary>
        /// IMEI导入结束
        /// </summary>
        Console_Import_IMEI_End = 1112,

        /// <summary>
        /// 控制台_启动模拟器
        /// </summary>
        Console_Emulator_Start = 2111,

        /// <summary>
        /// 控制台_停止模拟器
        /// </summary>
        Console_Emulator_Stop = 2112,

        #endregion

        #region App事件
        #region 客户端请求
        /// <summary>
        /// App授权
        /// </summary>
        App_Authorize = 1201,

        /// <summary>
        /// 登录
        /// </summary>
        App_WeChat_AutoLogin = 1220,

        /// <summary>
        /// 登录验证
        /// </summary>
        App_WeChat_Login_Verify = 1221,

        /// <summary>
        /// App加好友
        /// </summary>
        App_Friend_Add = 1222,

        /// <summary>
        /// App接到消息
        /// </summary>
        App_Message_Received = 1223,


        /// <summary>
        /// App微信账号同步
        /// </summary>
        App_WeChat_Sync = 1224,


        /// <summary>
        /// 登录短信验证
        /// </summary>
        App_WeChat_Login_SMSVerify = 1225,


        /// <summary>
        /// App在一定时间在无任何消息操作则通知我们告知关闭
        /// </summary>
        App_InformClose = 1298,

        #endregion

        #region 服务端请求
        /// <summary>
        /// App微信登录
        /// </summary>
        App_WeChat_Login = 2211,

        /// <summary>
        /// 申请添加好友
        /// </summary>
        App_ApplyAddFriend = 2221,

        /// <summary>
        /// App查看聊天记录
        /// </summary>
        App_Record_View = 2222,

        /// <summary>
        /// App发送消息
        /// </summary>
        App_SendText = 2223,

        /// <summary>
        /// App发送消息
        /// </summary>
        App_SendImage = 2224,

        /// <summary>
        /// App获取朋友圈
        /// </summary>
        App_Get_FriendCircle = 2225,


        /// <summary>
        /// 获取摇一摇数据
        /// </summary>
        App_Shake = 2227,

        /// <summary>
        /// 附近的人
        /// </summary>
        App_LookAround = 2228,

        /// <summary>
        /// 打招呼
        /// </summary>
        App_SayHello = 2229,

        /// <summary>
        /// 个人中心
        /// </summary>
        App_IndividualCenter = 2230,

        /// <summary>
        /// 主动请求关闭APP
        /// </summary>
        App_Request_Close = 2298,

        /// <summary>
        /// 退回首页
        /// </summary>
        App_BackHome = 2299,

        #endregion
        #endregion

        #region Web事件

        #region 客户端请求
        /// <summary>
        /// web微信登录
        /// </summary>
        Web_WeChat_Login = 1301,

        /// <summary>
        /// web微信登录
        /// </summary>
        Web_WeChat_Exit = 1302,


        /// <summary>
        /// web微信添加好友自主添加
        /// </summary>
        Web_WeChat_Apply_AddFriendByPersonal = 1321,


        /// <summary>
        /// web微信添加好友系统分配
        /// </summary>
        Web_WeChat_Apply_AddFriendBySystem = 1322,


        /// <summary>
        /// Web消息发送
        /// </summary>
        Web_SendText = 1323,

        /// <summary>
        /// Web消息图片
        /// </summary>
        Web_SendImage = 1324,

        /// <summary>
        /// Web消息重发
        /// </summary>
        Web_ResendSend = 1325,

        /// <summary>
        /// Web获取未读消息
        /// </summary>
        Web_Message_NewView = 1326,

        /// <summary>
        /// 获取朋友圈
        /// </summary>
        Web_Get_FriendCircle = 1327,

        /// <summary>
        /// 摇一摇
        /// </summary>
        Web_Shake = 1329,

        /// <summary>
        /// 附近的人
        /// </summary>
        Web_LookAround = 1330,


        /// <summary>
        /// 打招呼
        /// </summary>
        Web_SayHello = 1331,

        /// <summary>
        /// 设置地理位置
        /// </summary>
        Web_SetLocation = 1332,


        /// <summary>
        /// 个人中心
        /// </summary>
        Web_IndividualCenter = 1333,


        /// <summary>
        /// 重启设备
        /// </summary>
        Web_Restart_IMEI = 1334,


        /// <summary>
        /// 发送短信验证码
        /// </summary>
        Web_Send_SMSVerify = 1335,

        /// <summary>
        /// 返回首页
        /// </summary>
        Web_BackHome = 1399,

        #endregion

        #region 服务端请求

        #endregion

        #endregion


    }
}




