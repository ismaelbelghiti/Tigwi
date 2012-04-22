namespace Tigwi.UI.Models.Storage
{
    using System;

    using StorageLibrary;

    public class StoragePostModel
    {
        public StoragePostModel(IStorageContext storageContext, IMessage message)
        {
            this.PostDate = message.Date;
            this.Content = message.Content;
            this.Poster = storageContext.Accounts.Find(message.PosterId); //TODO: PopulateWith(name: message.PosterName, avatar: message.PosterAvatar)
        }

        public DateTime PostDate { get; private set; }

        public string Content { get; private set; }

        public StorageAccountModel Poster { get; private set; }
    }
}