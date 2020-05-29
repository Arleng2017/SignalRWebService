using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using SignalRWebService.Models;

namespace SignalRWebService.SignalRService
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class ChatGroup : Hub
    {
        public static List<PersonModel> personList = new List<PersonModel>();
        /// <summary>
        /// test this messsese
        /// </summary>
        /// <param name="user"></param>
        /// <param name="message"></param>
        /// <returns></returns>

        [HttpGet]
        public async Task Login(string id)
        {
            if (personList.Any())
            {
                personList.RemoveAll(it => it.DisplayName == id);
            }
            personList.Add(new PersonModel
            {
                Id = Context.ConnectionId,
                DisplayName = id
            });
            await Clients.Client(personList.FirstOrDefault().Id).SendAsync("LoginSuccess");
        }

        [HttpPost]
        public async Task Logout()
        {
            personList.RemoveAll(it => it.DisplayName == "admin");
            await Clients.Client(Context.ConnectionId).SendAsync("LogoutSuccess");
        }

        [HttpGet]
        public Task CancelOrder(string name)
        {
            var adminId = personList.Where(it => it.DisplayName == name).Select(it=>it.Id).FirstOrDefault(); ;
            return Clients.Client(adminId).SendAsync("SendCancelOrderSuccess","Ok");
        }
    }
}
