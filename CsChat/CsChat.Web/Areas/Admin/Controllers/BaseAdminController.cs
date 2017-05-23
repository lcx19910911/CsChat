using CsChat.Web.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CsChat.Web.Controllers;

namespace CsChat.Web.Areas.Admin.Controllers
{
    [LoginFilter]
    public class BaseAdminController : BaseController
    {

    }
}