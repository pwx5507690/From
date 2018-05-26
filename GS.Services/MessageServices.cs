using GS.SQL.DataSource;
using GS.SQLModel;
using GS.SQLModel.Cms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GS.Services
{
    public interface IMessageServices
    {
        int Send(MessageSendUser messageSendUser);
        int Reply(MessageAcceptUser messageAcceptUser);
        void Delete(Message message);
        void Delete(MessageAcceptUser messageAcceptUser);
        void Remove(int id);
        void Update(MessageAcceptUser messageAcceptUser);
        void MessageAcceptUserImportant(int id, string important = "Important");
        MessageAcceptUser GetAcceptMessageById(int id);
        IEnumerable<Message> GetMessageByMessageCode(string messageCode);
        SQLPage<MessageAcceptUser> GetAccepMessageByUserIdDelete(int userId, int pageSize, int currentPage, string name);
        SQLPage<MessageAcceptUser> GetAccepMessageByUserIdDelete(int userId, int pageSize, int currentPage);
        SQLPage<MessageAcceptUser> GetAccepMessageByUserIdImportant(int userId, int pageSize, int currentPage, string name);
        SQLPage<MessageAcceptUser> GetAccepMessageByUserIdImportant(int userId, int pageSize, int currentPage);
        SQLPage<MessageAcceptUser> GetAccepMessageByUserId(int userId, int pageSize, int currentPage, string name);
        SQLPage<MessageAcceptUser> GetAccepMessageByUserId(int userId, int pageSize, int currentPage);
        SQLPage<MessageAcceptUser> GetAccepMessageByUserIdRead(int userId, int pageSize, int currentPage, string name);
        SQLPage<MessageAcceptUser> GetAccepMessageByUserIdRead(int userId, int pageSize, int currentPage);
        SQLPage<MessageAcceptUser> GetAccepMessageByUserIdNoRead(int userId, int pageSize, int currentPage, string name);
        SQLPage<MessageAcceptUser> GetAccepMessageByUserIdNoRead(int userId, int pageSize, int currentPage);
        SQLPage<MessageAcceptUser> GetAccepMessageByUserId(int userId, int pageSize, int currentPage, DateTime datetime);
    }
    public class MessageServices : IMessageServices
    {
        private readonly User _user;
        private readonly Message _message;
        private readonly MessageAcceptUser _messageAcceptUser;
        private readonly MessageSendUser _messageSendUser;
        public readonly Site _site;
        public MessageServices()
        {
            _messageAcceptUser = new MessageAcceptUser();
            _messageSendUser = new MessageSendUser();
            _message = new Message();
            _user = new User();
            _site = new Site();
        }
        private List<T> SetMessageUser<T>(IEnumerable<T> msg) where T : MessageBase<T>
        {
            var mesg = msg.ToList();
            if (!mesg.Any())
                return mesg;

            var userIds = mesg.Select(t => t.SendUserId).Where(t => t > 0).Distinct();
            if (userIds.Any())
            {
                var user = _user.Exp().And(t => t.Id.In(userIds.ToArray())).Query();
                foreach (var item in mesg)
                    item.SendUser = user.Where(t => t.Id.Equals(item.SendUserId)).SingleOrDefault();
            }

            var siteUser = mesg.Where(t => t.SendUserId < 0);
            if (siteUser.Any())
            {
                var siteIds = siteUser.Select(t => Math.Abs(t.SendUserId)).Distinct();
                var site = _site.Exp().And(t => t.Id.In(siteIds.ToArray())).Query();
                foreach (var item in mesg)
                {
                    if (item.SendUserId > 0)
                        continue;
                    var siteItem = site.Where(t => t.Id.Equals(Math.Abs(item.SendUserId))).SingleOrDefault();
                    item.SendUser = new User()
                    {
                        Id = item.SendUserId,
                        Name = siteItem.Name
                    };
                }
            }
            return mesg.ToList();
        }
        public int Reply(MessageAcceptUser messageAcceptUser)
        {
            int r;
            messageAcceptUser.Updatetime = DateTime.UtcNow;
            var message = new Message()
            {
                Content = messageAcceptUser.Content,
                Updatetime = messageAcceptUser.Updatetime,
                SendUserId = messageAcceptUser.AcceptUserId,
                AcceptUserId = messageAcceptUser.SendUserId,
                MessageCode = messageAcceptUser.MessageCode,
                Title = messageAcceptUser.Title,
            };
            _message.Add(message);

            var messageUser = _messageAcceptUser.Exp()
                 .And(t => t.MessageCode == messageAcceptUser.MessageCode && t.AcceptUserId == messageAcceptUser.SendUserId).Query().FirstOrDefault();
            if (messageUser != null)
            {
                messageUser.IsRead = false;
                r = _messageAcceptUser.Update(messageUser);
            }
            else
            {
                messageUser = new MessageAcceptUser()
                {
                    Content = messageAcceptUser.Content,
                    Updatetime = messageAcceptUser.Updatetime,
                    SendUserId = messageAcceptUser.AcceptUserId,
                    AcceptUserId = messageAcceptUser.SendUserId,
                    MessageCode = messageAcceptUser.MessageCode,
                    Title = messageAcceptUser.Title,
                    IsRead = false
                };
                r = _messageAcceptUser.Add(messageUser);
            }
            return r;
        }
        public int Send(MessageSendUser messageSendUser)
        {
            messageSendUser.Updatetime = DateTime.UtcNow;
            messageSendUser.MessageCode = Guid.NewGuid().ToString();
            var r = _messageSendUser.Add(messageSendUser);
            var message = new Message()
            {
                Content = messageSendUser.Content,
                Updatetime = messageSendUser.Updatetime,
                SendUserId = messageSendUser.SendUserId,
                AcceptUserId = messageSendUser.AcceptUserId,
                MessageCode = messageSendUser.MessageCode,
                Title = messageSendUser.Title,
            };
            _message.Add(message);
            var messageAcceptUser = new MessageAcceptUser()
            {
                Content = messageSendUser.Content,
                Updatetime = messageSendUser.Updatetime,
                SendUserId = messageSendUser.SendUserId,
                AcceptUserId = messageSendUser.AcceptUserId,
                MessageCode = messageSendUser.MessageCode,
                Title = messageSendUser.Title,
                IsRead = false
            };
            _messageAcceptUser.Add(messageAcceptUser);
            return r;
        }
        public void Delete(Message message)
        {
            _message.Delete(message);
        }
        public MessageAcceptUser GetAcceptMessageById(int id)
        {
            var mesg = _messageAcceptUser.Exp().And(t => t.Id == id).And(t => t.IsDelete == false).Query();
            return SetMessageUser(mesg).FirstOrDefault();
        }
        public SQLPage<MessageAcceptUser> GetAccepMessageByUserIdNoRead(int userId, int pageSize, int currentPage, string name)
        {
            var mesg = _messageAcceptUser.Exp().And(t => t.AcceptUserId == userId && t.IsRead == false).And(t => t.IsDelete == false).And(t => t.Title.Like(name))
                .OrderAsc(t => t.Updatetime).Page(pageSize, currentPage).QueryPage();

            mesg.Result = SetMessageUser(mesg.Result);
            return mesg;
        }
        public SQLPage<MessageAcceptUser> GetAccepMessageByUserIdNoRead(int userId, int pageSize, int currentPage)
        {
            var mesg = _messageAcceptUser.Exp().And(t => t.AcceptUserId == userId && t.IsRead == false).And(t => t.IsDelete == false)
                .OrderAsc(t => t.Updatetime).Page(pageSize, currentPage).QueryPage();
            mesg.Result = SetMessageUser(mesg.Result);
            return mesg;
        }
        public SQLPage<MessageAcceptUser> GetAccepMessageByUserIdRead(int userId, int pageSize, int currentPage, string name)
        {
            var mesg = _messageAcceptUser.Exp().And(t => t.AcceptUserId == userId && t.IsRead == true).And(t => t.IsDelete == false).And(t => t.Title.Like(name))
                .OrderAsc(t => t.Updatetime).Page(pageSize, currentPage).QueryPage();
            mesg.Result = SetMessageUser(mesg.Result);
            return mesg;
        }
        public SQLPage<MessageAcceptUser> GetAccepMessageByUserIdRead(int userId, int pageSize, int currentPage)
        {
            var mesg = _messageAcceptUser.Exp().And(t => t.AcceptUserId == userId && t.IsRead == true).And(t => t.IsDelete == false).OrderAsc(t => t.Updatetime).Page(pageSize, currentPage).QueryPage();
            mesg.Result = SetMessageUser(mesg.Result);
            return mesg;
        }
        public SQLPage<MessageAcceptUser> GetAccepMessageByUserId(int userId, int pageSize, int currentPage, DateTime datetime)
        {
            var mesg = _messageAcceptUser.Exp().And(t => t.AcceptUserId == userId && t.Updatetime == datetime).And(t => t.IsDelete == false).OrderAsc(t => t.Updatetime).Page(pageSize, currentPage).QueryPage();
            mesg.Result = SetMessageUser(mesg.Result);
            return mesg;
        }
        public SQLPage<MessageAcceptUser> GetAccepMessageByUserId(int userId, int pageSize, int currentPage, string name)
        {
            var mesg = _messageAcceptUser.Exp().And(t => t.AcceptUserId == userId).And(t => t.IsDelete == false).And(t => t.Title.Like(name))
                .OrderAsc(t => t.Updatetime).Page(pageSize, currentPage).QueryPage();
            mesg.Result = SetMessageUser(mesg.Result);
            return mesg;
        }
        public SQLPage<MessageAcceptUser> GetAccepMessageByUserId(int userId, int pageSize, int currentPage)
        {
            var mesg = _messageAcceptUser.Exp().And(t => t.AcceptUserId == userId).And(t => t.IsDelete == false).OrderAsc(t => t.Updatetime).Page(pageSize, currentPage).QueryPage();
            mesg.Result = SetMessageUser(mesg.Result);
            return mesg;
        }
        public void Update(MessageAcceptUser messageAcceptUser)
        {
            _messageAcceptUser.Update(messageAcceptUser);
        }
        public void Delete(MessageAcceptUser messageAcceptUser)
        {
            _messageAcceptUser.Delete(messageAcceptUser);
        }
        public IEnumerable<Message> GetMessageByMessageCode(string messageCode)
        {
            return SetMessageUser(_message.Exp().And(t => t.MessageCode == messageCode).OrderAsc(t => t.Id).Query());
        }
        public void MessageAcceptUserImportant(int id, string important = "Important")
        {
            _messageAcceptUser.Exec($"update messageAcceptUser set Stats='{important}' where id={id}");
        }
        public SQLPage<MessageAcceptUser> GetAccepMessageByUserIdImportant(int userId, int pageSize, int currentPage, string name)
        {
            var mesg = _messageAcceptUser.Exp().And(t => t.AcceptUserId == userId && t.Stats == "Important").And(t => t.IsDelete == false).And(t => t.Title.Like(name))
               .OrderAsc(t => t.Updatetime).Page(pageSize, currentPage).QueryPage();
            mesg.Result = SetMessageUser(mesg.Result);
            return mesg;
        }
        public SQLPage<MessageAcceptUser> GetAccepMessageByUserIdImportant(int userId, int pageSize, int currentPage)
        {
            var mesg = _messageAcceptUser.Exp().And(t => t.AcceptUserId == userId && t.Stats == "Important").And(t => t.IsDelete == false)
              .OrderAsc(t => t.Updatetime).Page(pageSize, currentPage).QueryPage();
            mesg.Result = SetMessageUser(mesg.Result);
            return mesg;
        }
        public SQLPage<MessageAcceptUser> GetAccepMessageByUserIdDelete(int userId, int pageSize, int currentPage, string name)
        {
            var mesg = _messageAcceptUser.Exp().And(t => t.AcceptUserId == userId).And(t => t.IsDelete == true).And(t => t.Title.Like(name))
                .OrderAsc(t => t.Updatetime).Page(pageSize, currentPage).QueryPage();
            mesg.Result = SetMessageUser(mesg.Result);
            return mesg;
        }
        public SQLPage<MessageAcceptUser> GetAccepMessageByUserIdDelete(int userId, int pageSize, int currentPage)
        {
            var mesg = _messageAcceptUser.Exp().And(t => t.AcceptUserId == userId).And(t => t.IsDelete == true)
              .OrderAsc(t => t.Updatetime).Page(pageSize, currentPage).QueryPage();
            mesg.Result = SetMessageUser(mesg.Result);
            return mesg;
        }
        public void Remove(int id)
        {
            _messageAcceptUser.Exec($"update messageAcceptUser set IsDelete=1 where id={id}");
        }
    }
}
