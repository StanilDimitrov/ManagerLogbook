////using ManagerLogbook.Data.Models;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Threading.Tasks;
//using ManagerLogbook.Services.Contracts;
//using ManagerLogbook.Services.DTOs;
//using ManagerLogbook.Web.Models;
//using ManagerLogbook.Web.Extensions;
//using ManagerLogbook.Web.Mappers;
//using ManagerLogbook.Web.Services.Contracts;
//using ManagerLogbook.Web.Utils;
//using Microsoft.AspNetCore.Authorization;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.AspNetCore.Mvc.Rendering;
//using Microsoft.Extensions.Caching.Memory;
//using ManagerLogbook.Web.Areas.Manager.Models;
//using ManagerLogbook.Services.CustomExeptions;
//using log4net;
//using Microsoft.AspNetCore.SignalR;
//using ManagerLogbook.Web.Hubs;
////using ManagerLogbook.Data.Models;
//using Microsoft.AspNetCore.Identity;

//namespace ManagerLogbook.Web.Services
//{
//    public class UserRapper
//    {
//        private readonly UserManager<jjjUser> userManager;

//        public UserRapper(UserManager<jjjUser> userManager)
//        {
//            this.userManager = userManager;
//        }

//        public async Task<int> GetLoggedUserIdAsync()
//        {
//            var userId = this.userManager.GetUserId(User);
//        }

//    }
//}
