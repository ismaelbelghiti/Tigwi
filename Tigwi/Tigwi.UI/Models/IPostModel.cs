namespace Tigwi.UI.Models
{
    using System;

    public interface IPostModel
    {
        DateTime PostDate { get; }

        string Content { get; }

        IAccountModel Poster { get; }
    }
}