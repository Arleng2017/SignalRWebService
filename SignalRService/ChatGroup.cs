using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using SignalRWebService.Models;

namespace SignalRWebService.SignalRService
{
    public class ChatGroup : Hub
    {
        public static List<PersonModel> personList = new List<PersonModel>();
        /// <summary>
        /// test this messsese
        /// </summary>
        /// <param name="user"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        public async Task SendMessage(string user, string message)
        {
            await Clients.All.SendAsync("ReceiveMessage", user, message);
        }

        public Task SendMessageToGroup(string groupName, string message)
        {
            return Clients.Group(groupName).SendAsync("Send", $"{Context.ConnectionId}: {message}");
        }

        public async Task Login()
        {
            if (personList.Any())
            {
                personList.RemoveAll(it => it.DisplayName == "admin");
            }
            personList.Add(new PersonModel
            {
                Id = Context.ConnectionId,
                DisplayName = "admin"
            });
            await Clients.Client(personList.FirstOrDefault().Id).SendAsync("LoginSuccess");
        }

        public async Task Logout()
        {
            //var isLogout = personList.Remove(personList.Where(it=>it.Id==Context.ConnectionId).LastOrDefault());
            personList.RemoveAll(it => it.DisplayName == "admin");
            await Clients.Client(Context.ConnectionId).SendAsync("LogoutSuccess");
        }

        public Task CancelOrder()
        {
            //var adminId = personList.Where(it =>it.DisplayName == "admin")
            //    .Select(it=>it.Id)
            //    .FirstOrDefault();
            return Clients.Client(personList.FirstOrDefault().Id).SendAsync("SendCancelOrderSuccess","Ok");
        }

        public Task SendPrivateMessage(string user, string message)
        {
            var userSend = personList.Where(it => it.Id == Context.ConnectionId)
                .Select(it => it.DisplayName)
                .FirstOrDefault();

            var userReceive = personList.Where(it => it.DisplayName == user)
                .Select(it => it.Id)
                .FirstOrDefault();

            return Clients.Client(userReceive).SendAsync("Send", userSend, message);
        }

    }
}
