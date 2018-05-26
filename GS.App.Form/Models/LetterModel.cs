using GS.SQLModel;
using GS.View.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Webdiyer.WebControls.Mvc;

namespace GS.App.Form.Models
{
    public class LetterModel
    {
        public LetterModel()
        {
            MessageAcceptUserItem = new MessageAcceptUser();
        }
        public PagedList<MessageAcceptUser> MessageAcceptUser { get; set; }
        public MessageAcceptUser MessageAcceptUserItem { get; set; }
        public IEnumerable<VUser> User { get; set; }
        public IEnumerable<Message> Message { get; set; }
    }
}