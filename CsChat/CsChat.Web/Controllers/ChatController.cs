using CsChat.Core;
using CsChat.IService;
using CsChat.Model;
using CsChat.Service;
using CsChat.Web.Filters;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace CsChat.Web.Controllers
{


    [HubName("ChatHub")]
    public class ChatHub : Hub
    {
        private LoginUser _loginUser = null;

        public LoginUser LoginUser
        {
            get
            {
                return _loginUser != null ? _loginUser : CryptoHelper.AES_Decrypt(this.Context.RequestCookies[Params.UserCookieName].Value, Params.SecretKey).DeserializeJson<LoginUser>();
            }
        }

        private IRecordService IRecordService = new RecordService();
        private IUserService IUserService = new UserService();
        private IUserDeviceService IUserDeviceService = new UserDeviceService();
        private IRelationGroupService IRelationGroupService = new RelationGroupService();
        /// <summary>
        /// 打开链接
        /// </summary>
        /// <param name="userId">链接名</param>
        public void SetRecGroup(int userId)//设置接收组  
        {
            int id = 0;
            if (IUserDeviceService.Add(userId, GetIp(), out id).Result)
                this.Groups.Add(this.Context.ConnectionId, userId + "_" + id);
        }

        public string GetIp()
        {
            return  Guid.NewGuid().ToString("N");
            //WebHelper.GetRealIP();
        }
        /// <summary>
        /// 关闭链接
        /// </summary>
        /// <param name="groupName">链接名</param>
        public void RemoveRecGroup(string groupName)//设置接收组  
        {
            this.Groups.Remove(this.Context.ConnectionId, groupName);
        }

        /// <summary>
        ///发送文本消息
        /// </summary>
        /// <param name="id"></param>
        /// <param name="message"></param>
        /// <param name="toUserId"></param>
        public void SendMessage(string id, string message, int toUserId, bool isImages = false)
        {
            var deviceList = IUserDeviceService.GetListByUserID(toUserId);
            if (deviceList != null && deviceList.Count > 0)
            {
                deviceList.ForEach(x =>
                {
                    //group组名，用户id_设备id
                    this.Clients.Group(toUserId + "_" + x.ID).NotifyNewMessage(new
                    {
                        GuID = id,
                        ToUserID = toUserId,
                        From = this.LoginUser.ID,
                        Message = message,
                        IsImages = isImages
                    });//向指定组发送 
                });
            }
        }

        /// <summary>
        /// 设置已读
        /// </summary>
        /// <param name="relationId"></param>
        public void SetReaded(int relationId)
        {
            IRecordService.SetReadList(relationId);
        }

        /// <summary>
        ///发送文本消息
        /// </summary>
        /// <param name="id"></param>
        /// <param name="message"></param>
        /// <param name="toUserId"></param>
        public void NotityHadSendMessage(string id, string message,int toUserId, bool isImages = false)
        {
            var deviceList = IUserDeviceService.GetListByUserID(this.LoginUser.ID);
            if (deviceList != null && deviceList.Count > 0)
            {
                deviceList.ForEach(x =>
                {
                    if (x.IP != GetIp())
                    {
                        //group组名，用户id_设备id
                        this.Clients.Group(this.LoginUser.ID + "_" + x.ID).NotifyHadSendMessage(new
                        {
                            GuID = id,
                            ToUserID = toUserId,
                            From = this.LoginUser.ID,
                            Message = message,
                            IsImages = isImages
                        });//向指定组发送 
                    }
                });
            }
        }

        /// <summary>
        /// 连接
        /// </summary>
        /// <returns></returns>
        public override Task OnConnected()
        {
            var userId = this.LoginUser.ID;
            //修改登录用户状态
            var user = IUserService.Find(userId);
            if (user != null)
            {
                SetRecGroup(userId);
                if (IUserDeviceService.GetListByUserID(userId).Count == 0)
                {
                    user.IsOnLine = true;
                    IUserService.Update(user, userId);
                }


                var relationList = IRelationGroupService.GetListByUserID(userId).OrderByDescending(x => x.LastTime).ToList();
                //用户集合 
                var userIdList = relationList.Where(x => x.UserID == userId).Select(x => x.RelationUserID).ToList();
                userIdList.AddRange(relationList.Where(x => x.RelationUserID == userId).Select(x => x.UserID).ToList());
                if (userIdList != null && userIdList.Count > 0)
                {
                    var onLineUserIds = IUserService.GetOnLineIdsByUserIDs(userIdList);
                    if (onLineUserIds != null && onLineUserIds.Count > 0)
                    {
                        var devicdList = IUserDeviceService.GetListByUserIDs(onLineUserIds);
                        if (devicdList != null && devicdList.Count > 0)
                        {
                            var dic = devicdList.GroupBy(x => x.UserID).ToDictionary(x => x.Key, x => x.ToList());
                            AsyncHelper.Run(() =>
                            {
                                onLineUserIds.ForEach(x =>
                                {
                                    if (dic.ContainsKey(x))
                                    {
                                        dic[x].ForEach(y =>
                                        {
                                            this.Clients.Group(x + "_" + y.ID).NotifyOnLine(userId);//向指定组发送  
                                        });
                                    }
                                });
                            });
                        }
                    }
                }
            }
            return base.OnConnected();

        }

        /// <summary>
        /// 断开
        /// </summary>
        /// <returns></returns>
        public override Task OnDisconnected(bool stopCalled)
        {
            var userId = this.LoginUser.ID;
            var deviceList = IUserDeviceService.GetListByUserID(userId);
            if (deviceList != null && deviceList.Count > 0)
            {
                deviceList.ForEach(x =>
                {
                    //删除组
                    RemoveRecGroup(userId + "_" + x.ID);
                });
                IUserDeviceService.DeleteDeviceByUserID(userId,GetIp());
            }
            //修改登录用户状态
            var user = IUserService.Find(userId);
            if (user != null)
            {
                //当前无设备联系
                if (IUserDeviceService.GetListByUserID(userId).Count == 0)
                {
                    user.IsOnLine = false;
                    IUserService.Update(user, userId);
                }
                var relationList = IRelationGroupService.GetListByUserID(userId).OrderByDescending(x => x.LastTime).ToList();
                //用户集合 
                var userIdList = relationList.Where(x => x.UserID == userId).Select(x => x.RelationUserID).ToList();
                userIdList.AddRange(relationList.Where(x => x.RelationUserID == userId).Select(x => x.UserID).ToList());
                if (userIdList != null && userIdList.Count > 0)
                {
                    var onLineUserIds = IUserService.GetOnLineIdsByUserIDs(userIdList);
                    if (onLineUserIds != null && onLineUserIds.Count > 0)
                    {
                        var devicdList = IUserDeviceService.GetListByUserIDs(onLineUserIds);
                        if (devicdList != null && devicdList.Count > 0)
                        {
                            var dic = devicdList.GroupBy(x => x.UserID).ToDictionary(x => x.Key, x => x.ToList());
                            AsyncHelper.Run(() =>
                            {
                                onLineUserIds.ForEach(x =>
                                {
                                    if (dic.ContainsKey(x))
                                    {
                                        dic[x].ForEach(y =>
                                        {
                                            this.Clients.Group(x + "_" + y.ID).NotifyOffLine(userId);//向指定组发送  
                                            });
                                    }
                                });
                            });
                        }
                    }
                }

            }
            return base.OnDisconnected(stopCalled);
        }
    }

    [LoginFilter]
    public class ChatController : BaseController
    {

        private IRecordService IRecordService;
        private IUserService IUserService;
        private IUserDeviceService IUserDeviceService;
        private IRelationGroupService IRelationGroupService;

        public ChatController(IUserService _IUserService, IRecordService _IRecordService, IUserDeviceService _IUserDeviceService, IRelationGroupService _IRelationGroupService)
        {
            this.IRecordService = _IRecordService;
            this.IUserService = _IUserService;
            this.IUserDeviceService = _IUserDeviceService;
            this.IRelationGroupService = _IRelationGroupService;
        }

        /// <summary>
        /// 首页
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            return View(this.LoginUser);
        }

        /// <summary>
        ///发送文本消息
        /// </summary>
        /// <param name="id"></param>
        /// <param name="message"></param>
        /// <param name="toUserId"></param>
        public ActionResult Init()
        {
            var userId = this.LoginUser.ID;
            var userList = new List<User>();
            var recordDic = new Dictionary<int, List<Record>>();
            //用户上线
            var user = IUserService.Find(userId);
            if (!user.IsOnLine)
            {
                user.IsOnLine = true;
                IUserService.Update(user);
            }
            //获取用户聊天的关系
            var relationList = IRelationGroupService.GetListByUserID(userId).OrderByDescending(x => x.LastTime).ToList();
            if (relationList != null)
            {
                //用户集合
                var userIdList = relationList.Select(x => x.UserID).ToList();
                userIdList.AddRange(relationList.Select(x => x.RelationUserID).ToList());
                var relationIdList = relationList.Select(x => x.ID).ToList();
                if (userIdList != null && userIdList.Count > 0)
                {
                    userList = IUserService.GetList(x => userIdList.Contains(x.ID) && x.ID != userId).ToList();

                    if (userList != null && userList.Count > 0)
                    {
                        //获取未读的聊天信息
                        recordDic = IRecordService.GetList(x => relationIdList.Contains(x.RelationID) && !x.IsRead && x.SendUserID != userId).GroupBy(x => x.SendUserID).ToDictionary(x => x.Key, x => x.ToList());
                        ////无未读获取最近聊天
                        //if (recordDic == null || recordDic.Count == 0)
                        //{
                        //    var recordList = IRecordService.GetPrevList(relationList[0].ID, DateTime.Now, 10);
                        //    if (recordList != null && recordList.Count > 0)
                        //        recordDic.Add(relationList[0].UserID, recordList);
                        //}
                        var relationDic = relationList.Where(x => x.UserID == userId).ToDictionary(x => x.RelationUserID, x => x.ID);
                        var relationSendDic = relationList.Where(x => x.UserID != userId).ToDictionary(x => x.UserID, x => x.ID);
                        userList.ForEach(x =>
                        {
                            if (recordDic.ContainsKey(x.ID))
                            {
                                x.IsHadNoReadMsg = true;
                                x.SendTime = recordDic[x.ID].Max(y => y.SendTime);
                            }
                            if (relationDic.ContainsKey(x.ID))
                            {
                                x.RelationID = relationDic[x.ID];
                            }
                            else if (relationSendDic.ContainsKey(x.ID))
                            {
                                x.RelationID = relationSendDic[x.ID];
                            }
                        });
                        userList = userList.OrderByDescending(x => x.SendTime).ToList();
                        userList[0].RecordList = IRecordService.GetPrevList(userList[0].RelationID, DateTime.Now, 20, 0);
                    }
                }

            }
            return JResult(userList);

        }

        /// <summary>
        ///发送文本消息
        /// </summary>
        /// <param name="id"></param>
        /// <param name="message"></param>
        /// <param name="toUserId"></param>
        public ActionResult AddText(string id, string message, int relationId)
        {
            return JResult(IRecordService.AddText(id, message, relationId, this.LoginUser.ID));

        }

        /// <summary>
        /// 发送图片
        /// </summary>
        /// <param name="id"></param>
        /// <param name="message"></param>
        /// <param name="toUserId"></param>
        public ActionResult AddImage(string id, string message, int relationId)
        {
            return JResult(IRecordService.AddImage(id, message, relationId, this.LoginUser.ID));
        }

        /// <summary>
        /// 加载聊天记录
        /// </summary>
        /// <param name="relationId"></param>
        public ActionResult GetRecord(int relationId, DateTime time, int searchId)
        {
            return JResult(IRecordService.GetPrevList(relationId, time, 20, searchId));
        }


        /// <summary>
        /// 加载聊天记录
        /// </summary>
        /// <param name="relationId"></param>
        public ActionResult AddChat(int toUserId)
        {
            IRelationGroupService.Add(LoginUser.ID, toUserId);
            return RedirectToAction("Index");
        }

        /// <summary>
        /// 加载聊天记录
        /// </summary>
        /// <param name="relationId"></param>
        public ActionResult FindUser(int id)
        {
            var userResult = IUserService.Find(id);
            if (userResult != null)
            {
                var relation = IRelationGroupService.Find(x => (x.UserID == id && x.RelationUserID == this.LoginUser.ID) || (x.UserID == this.LoginUser.ID && x.RelationUserID == id));
                if(relation!=null)
                    userResult.RelationID = relation.ID;
            }
            return JResult(userResult);
        }
    }
}