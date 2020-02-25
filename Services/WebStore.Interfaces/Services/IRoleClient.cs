using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;
using WebStore.Domain.Entities.Identity;

namespace WebStore.Interfaces.Services
{
    public interface IRoleClient : IRoleStore<Role>
    {
    }
}
