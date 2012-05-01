namespace Tigwi.UI.Models.Storage
{
    using System;

    using StorageLibrary;

    public abstract class StorageEntityModel
    {
        protected StorageEntityModel(IStorage storage, IStorageContext storageContext, Guid id)
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

        protected IStorageContext StorageContext { get; set; }

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
        }

        internal abstract void Repopulate();
    }
}