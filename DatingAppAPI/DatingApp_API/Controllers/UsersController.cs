using DatingApp_API.Data;
using DatingApp_API.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DatingApp_API.Controllers
{
   
    public class UsersController : BaseApiController
    {
        public DataContext _dataContext { get; set; }
        public UsersController(DataContext dataContext)
        {
            _dataContext = dataContext;
        }
        [HttpGet]
        public ActionResult<IEnumerable< AppUser>> GetAll()
        {
            var user = _dataContext.Users.ToList();
            return user;
        }
        [Authorize]
        [HttpGet("{id}")]
        public ActionResult<AppUser> GetUsers(int id)
        {
            var user = _dataContext.Users.Find(id);
            return user;
        }

        
    }
}
