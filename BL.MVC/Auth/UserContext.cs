using BL.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BL.Models;

namespace BL.MVC
{
    public class UserContext
    {
        public static UserInfo CurrentUser
        {
            get
            {
                IAuthenticationService authenticationService =DIContainer.ResolvePerHttpRequest<IAuthenticationService>();
                var currentUser = authenticationService.GetAuthenticatedUser();
                if (currentUser != null)
                    return currentUser;
                return null;
            }
        }
    }
}
