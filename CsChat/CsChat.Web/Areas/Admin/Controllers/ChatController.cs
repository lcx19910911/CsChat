using CsChat.Web.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CsChat.Web.Controllers;

using System.IO;
using System.Collections;
using CsChat.IService;

namespace CsChat.Web.Areas.Admin.Controllers
{
    /// <summary>
    /// 会议
    /// </summary>
    public class ChatController : BaseAdminController
    {
        public IRecordService IRecordService;
        public IRelationGroupService IRelationGroupService;
        public IUserService IUserService;

        public ChatController(IRecordService _IRecordService, IUserService _IUserService, IRelationGroupService _IRelationGroupService)
        {
            this.IRecordService = _IRecordService;
            this.IUserService = _IUserService;
            this.IRelationGroupService = _IRelationGroupService;
        }


        /// <summary>
        /// 首页
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// 获取分页列表
        /// </summary>
        /// <param name="pageIndex">页码</param>
        /// <param name="pageSize">分页大小</param>
        /// <param name="title">标题 - 搜索项</param>
        /// <param name="createdTimeStart">发布日期起 - 搜索项</param>
        /// <param name="createdTimeEnd">发布日期止 - 搜索项</param>
        /// <returns></returns>
        public JsonResult GetPageList(int pageIndex, int pageSize, string sendUserName, string toUserName, DateTime? createdTimeStart, DateTime? createdTimeEnd)
        {
            var pagelist = IRelationGroupService.GetPageList(pageIndex, pageSize, sendUserName, toUserName, createdTimeStart, createdTimeEnd);
            return JResult(pagelist);
        }

        /// <summary>
        /// 查找实体
        /// </summary>
        /// <param name="model"</param>
        /// <returns></returns>
        public JsonResult Find(int ID,int sreachIndex,DateTime? searchTime)
        {
            var result = IRecordService.GetPrevList(ID, searchTime==null?DateTime.Now: searchTime.Value, 20, sreachIndex);
            return JResult(result);
        }      
    }
}