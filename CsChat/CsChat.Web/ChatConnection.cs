//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Threading.Tasks;
//using System.Web;
//using Microsoft.AspNet.SignalR;
//using System.Diagnostics;
//using CsChat.IService;
//using CsChat.Core;
//using CsChat.Web.Filters;
//using CsChat.Service;

//namespace CsChat.Web
//{
//    /// <summary>
//    /// 实体
//    /// </summary>
//    public class HubAction
//    {
//        public ActionCode Action { get; set; }

//        public object Data { get; set; }
//    }
//    /// <summary>
//    /// 操作
//    /// </summary>
//    public enum ActionCode
//    {
//        /// <summary>
//        /// 好友上线
//        /// </summary>
//        FriendOnLine = 1,
//        /// <summary>
//        /// 好友下线
//        /// </summary>
//        FriendOffLine = 2,

//        /// <summary>
//        /// 好友上线成功
//        /// </summary>
//        OnLineSuccess = 3,
//        /// <summary>
//        /// 好友上线失败
//        /// </summary>
//        OnLineFailed = 4,
//        /// <summary>
//        /// 好友下线成功
//        /// </summary>
//        OffLineSuccess = 5,
//        /// <summary>
//        /// 好友下线失败
//        /// </summary>
//        OffLineFailed = 6,
//        /// <summary>
//        /// 发送消息
//        /// </summary>
//        SendMessage = 7

//    }
//    [ExceptionFilter]
//    public class ChatConnection : PersistentConnection
//    {
//        private IRecordService IRecordService=new RecordService();
//        private IUserService IUserService = new UserService();
//        private IUserDeviceService IUserDeviceService = new UserDeviceService();
//        private IRelationGroupService IRelationGroupService = new RelationGroupService();
//        //public ChatConnection(IUserService _IUserService, IRecordService _IRecordService, IUserDeviceService _IUserDeviceService, IRelationGroupService _IRelationGroupService)
//        //{
//        //    this.IRecordService = _IRecordService;
//        //    this.IUserService = _IUserService;
//        //    this.IUserDeviceService = _IUserDeviceService;
//        //    this.IRelationGroupService = _IRelationGroupService;
//        //}
//        /// <summary>
//        /// 获取当前用户
//        /// </summary>
//        /// <param name="request"></param>
//        /// <returns></returns>
//        protected LoginUser GetLoginUser(IRequest request)
//        {
//            return CryptoHelper.AES_Decrypt(request.Cookies[Params.UserCookieName].Value, Params.SecretKey).DeserializeJson<LoginUser>();
//        }
//        protected override Task OnConnected(IRequest request, string connectionId)
//        {
//            //Debug.WriteLine("OnConnected");

//            var loginUser = GetLoginUser(request);
//            //修改登录用户状态
//            var user = IUserService.Find(loginUser.ID);
//            if (user == null)
//                return Connection.Send(connectionId, new HubAction
//                {
//                    Data = loginUser.ID,
//                    Action = ActionCode.OnLineFailed
//                });

//            user.IsOnLine = true;
//            user.ConnectionId = connectionId;
//            if (IUserService.Update(user) > 0)
//            {

//                //推送上线消息给在线的好友
//                var relationList = IRelationGroupService.GetListByUserID(loginUser.ID);
//                if (relationList != null && relationList.Count > 0)
//                {
//                    var userIdList = relationList.Where(x => x.UserID == loginUser.ID).Select(x => x.RelationUserID).ToList();
//                    userIdList.AddRange(relationList.Where(x => x.RelationUserID == loginUser.ID).Select(x => x.UserID).ToList());

//                    var connectionIds = IUserService.GetConnectIdListByUserIDs(userIdList);
//                    if (connectionIds != null && connectionIds.Count > 0)
//                    {
//                        AsyncHelper.Run(() =>
//                        {
//                            Connection.Send(connectionIds, new HubAction
//                            {
//                                Data = loginUser.ID,
//                                Action = ActionCode.FriendOnLine
//                            });
//                        });
//                    }
//                }

//                return Connection.Send(connectionId, new HubAction
//                {
//                    Data = loginUser.ID,
//                    Action = ActionCode.OnLineSuccess
//                });
//            }
//            else
//                return Connection.Send(connectionId, new HubAction
//                {
//                    Data = loginUser.ID,
//                    Action = ActionCode.OnLineFailed
//                });
//        }

//        protected override Task OnReceived(IRequest request, string connectionId, string data)
//        {
//            //Debug.WriteLine("OnReceived");
//            var loginUser = GetLoginUser(request);
//            return Connection.Broadcast(data);
//        }

//        protected override Task OnDisconnected(IRequest request, string connectionId, bool stopCalled)
//        {
//            //Debug.WriteLine("OnDisconnected");

//            var loginUser = GetLoginUser(request);
//            //修改登录用户状态
//            var user = IUserService.Find(loginUser.ID);
//            if (user == null)
//                return Connection.Send(connectionId, new HubAction
//                {
//                    Data = loginUser.ID,
//                    Action = ActionCode.OffLineFailed
//                });

//            user.IsOnLine = false;
//            user.ConnectionId = string.Empty;
//            if (IUserService.Update(user) > 0)
//            {

//                //推送离线消息给在线的好友
//                var relationList = IRelationGroupService.GetListByUserID(loginUser.ID);
//                if (relationList != null && relationList.Count > 0)
//                {
//                    var userIdList = relationList.Where(x => x.UserID == loginUser.ID).Select(x => x.RelationUserID).ToList();
//                    userIdList.AddRange(relationList.Where(x => x.RelationUserID == loginUser.ID).Select(x => x.UserID).ToList());

//                    var connectionIds = IUserService.GetConnectIdListByUserIDs(userIdList);
//                    if (connectionIds != null && connectionIds.Count > 0)
//                    {
//                        AsyncHelper.Run(() =>
//                    {
//                        Connection.Send(connectionIds, new HubAction
//                        {
//                            Data = loginUser.ID,
//                            Action = ActionCode.FriendOffLine
//                        });
//                    });
//                    }
//                }

//                 Connection.Send(connectionId, new HubAction
//                {
//                    Data = loginUser.ID,
//                    Action = ActionCode.OffLineSuccess
//                });
//            }
//            else
//                Connection.Send(connectionId, new HubAction
//                {
//                    Data = loginUser.ID,
//                    Action = ActionCode.OffLineFailed
//                });
//            return base.OnDisconnected(request,connectionId,stopCalled);
//        }

//        protected override Task OnReconnected(IRequest request, string connectionId)
//        {
//            //Debug.WriteLine("OnReconnected");
//            var loginUser = GetLoginUser(request);
//            //修改登录用户状态
//            var user = IUserService.Find(loginUser.ID);
//            if (user == null)
//                return Connection.Send(connectionId, new HubAction
//                {
//                    Data = loginUser.ID,
//                    Action = ActionCode.OffLineFailed
//                });

//            user.IsOnLine = true;
//            user.ConnectionId = connectionId;
//            if (IUserService.Update(user) > 0)
//            {
//                Connection.Send(connectionId, new HubAction
//                {
//                    Data = loginUser.ID,
//                    Action = ActionCode.OffLineSuccess
//                });
//            }
//            else
//            {
//                user.IsOnLine = false;
//                user.ConnectionId = string.Empty;
//                IUserService.Update(user);
//                Connection.Send(connectionId, new HubAction
//                {
//                    Data = loginUser.ID,
//                    Action = ActionCode.OnLineFailed
//                });
//            }
//            return base.OnReconnected(request, connectionId);
//        }
//    }
//}
