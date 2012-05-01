namespace Tigwi.UI.Models
{
    using System;
    using System.Collections.Generic;

    public interface IUserModel
    {
        ICollection<IAccountModel> Accounts { get; }

        string Avatar { get; set; }

        string Email { get; set; }

        string Login { get; }

        Guid Id { get; }
    }
}