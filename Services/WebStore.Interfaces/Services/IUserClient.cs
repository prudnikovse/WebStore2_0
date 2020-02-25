using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;
using WebStore.Domain.Entities.Identity;

namespace WebStore.Interfaces.Services
{
    public interface IUserClient : 
        IUserRoleStore<User>, 
        IUserPasswordStore<User>, 
        IUserEmailStore<User>,
        IUserPhoneNumberStore<User>,
        IUserTwoFactorStore<User>,
        IUserLockoutStore<User>,
        IUserClaimStore<User>,
        IUserLoginStore<User>
    {
    }
}
