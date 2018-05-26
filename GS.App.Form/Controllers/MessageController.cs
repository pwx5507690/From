using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Webdiyer.WebControls.Mvc;
using GS.Services;
using GS.SQL;
using GS.Common.Util;
using System.Web.Mvc;
using GS.SQLModel;
using GS.App.Form.Models;
using GS.App.Form.App_Start;
using MessageCilentProxy;

namespace GS.App.Form.Controllers
{
    public class MessageController : BaseController
    {
        private readonly IMessageServices _iMessageServices;
        private readonly IUserServices _iUserServices;
        public MessageController(IMessageServices iMessageServices, IUserServices iUserServices)
        {
            _iUserServices = iUserServices;
            _iMessageServices = iMessageServices;
        }
        private PagedList<T> GetListMessage<T>(SQL.DataSource.SQLPage<T> data, int currentPage)
        {
            var mesgData = data.Result.ToPagedList(currentPage, _pageSize);
            mesgData.TotalItemCount = data.Count;
            mesgData.CurrentPageIndex = currentPage;
            return mesgData;
        }
        [HttpGet]
        public ActionResult UBindImportant(int id, int page, int type)
        {
            _iMessageServices.MessageAcceptUserImportant(id, string.Empty);
            SetMessage($"标记成功");
            return Redirect($"~/Message/index?currentPage={page}&type={type}");
        }
        [HttpGet]
        public ActionResult BindImportant(int id, int page, int type)
        {
            _iMessageServices.MessageAcceptUserImportant(id);
            SetMessage($"标记成功");
            return Redirect($"~/Message/index?currentPage={page}&type={type}");
        }
        [HttpGet]
        public ActionResult Delete(int id, int page, int type)
        {
            _iMessageServices.Remove(id);
            SetMessage($"删除成功");
            return Redirect($"~/Message/index?currentPage={page}&type={type}");
        }
        [HttpGet]
        public ActionResult Letter(int? id)
        {
            var letterModel = new LetterModel();
            if (id != null)
            {
                var acceptMessage = _iMessageServices.GetAcceptMessageById(id.Value);
                letterModel.Message = _iMessageServices.GetMessageByMessageCode(acceptMessage.MessageCode);
                acceptMessage.IsRead = true;
                _iMessageServices.Update(acceptMessage);
                letterModel.MessageAcceptUserItem = acceptMessage;
            }
            letterModel.User = _iUserServices.GetUser().Result;
            GetMessage();
            return View(letterModel);
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Send(Message message)
        {
            var messageId = message.Id;
            if (string.IsNullOrEmpty(message.MessageCode))
                messageId = _iMessageServices.Send(new MessageSendUser()
                {
                    MessageCode = message.MessageCode,
                    SendUserId = User.Model.Id,
                    AcceptUserId = message.AcceptUserId,
                    Title = message.Title,
                    Content = message.Content
                });
            else
                _iMessageServices.Reply(new MessageAcceptUser()
                {
                    MessageCode = message.MessageCode,
                    SendUserId = User.Model.Id,
                    AcceptUserId = message.AcceptUserId,
                    Title = message.Title,
                    Content = message.Content,
                    IsRead = false
                });
            MessageCilent.Invoke("Letter", User.Model.Id, messageId);
            if (message.MessageCode.IsNullOrEmpty())
            {
                SetMessage($"发送成功");
                return Redirect($"~/Message/Index");
            }
            else
            {
                SetMessage($"回复成功");
                return Redirect($"~/Message/letter?id={messageId}");
            }

        }
        [HttpGet]
        public ActionResult Index(int currentPage = 1, int type = 2, string title = "", string time = "")
        {
            var pageIndex = currentPage - 1;
            SQL.DataSource.SQLPage<MessageAcceptUser> mesg = null;

            if (type == 0)
                mesg = title.IsNullOrEmpty()
                     ? _iMessageServices.GetAccepMessageByUserId(User.Model.Id, _pageSize, pageIndex)
                     : _iMessageServices.GetAccepMessageByUserId(User.Model.Id, _pageSize, pageIndex, title);
            else if (type == 1)
                mesg = title.IsNullOrEmpty()
                   ? _iMessageServices.GetAccepMessageByUserIdRead(User.Model.Id, _pageSize, pageIndex)
                   : _iMessageServices.GetAccepMessageByUserIdRead(User.Model.Id, _pageSize, pageIndex, title);
            else if (type == 2)
                mesg = title.IsNullOrEmpty()
                 ? _iMessageServices.GetAccepMessageByUserIdNoRead(User.Model.Id, _pageSize, pageIndex)
                 : _iMessageServices.GetAccepMessageByUserIdNoRead(User.Model.Id, _pageSize, pageIndex, title);
            else if (type == 3)
                mesg = title.IsNullOrEmpty()
               ? _iMessageServices.GetAccepMessageByUserIdImportant(User.Model.Id, _pageSize, pageIndex)
               : _iMessageServices.GetAccepMessageByUserIdImportant(User.Model.Id, _pageSize, pageIndex, title);
            else if (type == 4)
                mesg = title.IsNullOrEmpty()
               ? _iMessageServices.GetAccepMessageByUserIdDelete(User.Model.Id, _pageSize, pageIndex)
               : _iMessageServices.GetAccepMessageByUserIdDelete(User.Model.Id, _pageSize, pageIndex, title);

            ViewBag.page = currentPage;
            var letterModel = new LetterModel();
            letterModel.MessageAcceptUser = GetListMessage(mesg, currentPage);
            ViewBag.Title = title;
            ViewBag.Time = time;
            GetMessage();
            return View(letterModel);
        }
        public ActionResult SendMessage(Message message)
        {
            return View();
        }
        public ActionResult MessageInfo(int messageId)
        {
            return View();
        }
    }
}