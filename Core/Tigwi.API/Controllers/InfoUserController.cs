using System;
using Tigwi.API.Models;

namespace Tigwi.API.Controllers
{
    public abstract class InfoUserController : ApiController
    {

        // parts of code that are common when accessing methods by login or by id

        protected Answer AnswerMainInfo(Guid userId)
        {
            var userInfo = Storage.User.GetInfo(userId);
            var userToReturn = new User(userInfo, userId);
            return new Answer(userToReturn);
        }

    }
}
