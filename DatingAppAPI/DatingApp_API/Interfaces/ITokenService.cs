using DatingApp_API.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DatingApp_API.Interfaces
{
    public interface ITokenService
    {
        string CreateTokenSerive(AppUser appUser);
    }
}
