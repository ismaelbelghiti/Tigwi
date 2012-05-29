#region copyright
// Copyright (c) 2012, TIGWI
// All rights reserved.
// Distributed under  BSD 2-Clause license
#endregion
namespace Tigwi.UI.Models.Storage
{
    using System;

    using Tigwi.Storage.Library;

    using System.Web.Mvc;

    public class StoragePostModel : IPostModel
    {
        public StoragePostModel(StorageContext storageContext, IMessage message)
        {
            this.PostDate = message.Date;
            this.Content = message.Content;
            this.Id = message.Id;
            this.Poster =
                storageContext.InternalAccounts.InternalFind(message.PosterId).PopulateWith(
                    name: message.PosterName, avatar: message.PosterAvatar);
        }

        public DateTime PostDate { get; private set; }

        public string Content { get; private set; }

        public IAccountModel Poster { get; private set; }

        public Guid Id { get; private set; }
    }
}