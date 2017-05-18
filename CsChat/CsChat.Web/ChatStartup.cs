﻿using System;
using System.Threading.Tasks;
using Microsoft.Owin;
using Owin;

[assembly: OwinStartup(typeof(CsChat.Web.ChatStartup))]

namespace CsChat.Web
{
    public class ChatStartup
    {
        public void Configuration(IAppBuilder app)
        {
            app.MapSignalR();
            //app.MapSignalR<ChatConnection>("/chatconnection");
            // 有关如何配置应用程序的详细信息，请访问 http://go.microsoft.com/fwlink/?LinkID=316888
        }
    }
}
