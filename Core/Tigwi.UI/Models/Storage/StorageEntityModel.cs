namespace Tigwi.UI.Models.Storage
{
    using System;

    using Tigwi.Storage.Library;

    public abstract class StorageEntityModel
    {
        protected StorageEntityModel(IStorage storage, StorageContext storageContext, Guid id)
        {
            this.Storage = storage;
            this.StorageContext = storageContext;
            this.Id = id;
        }

        public Guid Id { get; private set; }

        protected bool Populated { get; set; }

        protected abstract bool InfosUpdated { get; set; }

        protected IStorage Storage { get; set; }

        internal bool Deleted { get; set; }

        protected StorageContext StorageContext { get; set; }

        internal void MarkDeleted()
        {
            this.Deleted = true;
        }

        internal abstract void Save();

        internal void Populate()
        {
            if (this.Populated)
            {
                return;
            }

            this.Repopulate();
            this.Populated = true;
        }

        internal abstract void Repopulate();
    }
}