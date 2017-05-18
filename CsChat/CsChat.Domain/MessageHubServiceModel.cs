using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CsChat.Domain
{
    public class MessageHubServiceModel
    {
        private string path = @"HubService\";
        public MessageHubServiceModel(HubServiceModel hubServiceModel, string messageID, Func<string, HubServiceModel> flushChannel)
        {
            this.HubServiceModel = hubServiceModel;
            this._messageID = messageID;
            this.flushChannel = flushChannel;
        }

        public MessageHubServiceModel(HubServiceModel hubServiceModel, Func<string, HubServiceModel> flushChannel)
        {
            this.HubServiceModel = hubServiceModel;
            this.flushChannel = flushChannel;
        }

        private Func<string, HubServiceModel> flushChannel = null;

        private HubServiceModel HubServiceModel { get; set; }


        private string _messageID { get; set; }


        public string MessageID
        {
            get
            {

                return this._messageID;
            }
        }
        private bool isFlushed = false;
    }
}
