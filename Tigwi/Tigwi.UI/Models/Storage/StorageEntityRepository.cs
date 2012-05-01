namespace Tigwi.UI.Models.Storage
{
    using System;
    using System.Collections.Generic;

    using StorageLibrary;

    public class StorageEntityRepository<T>
    {
        protected StorageEntityRepository(IStorage storage, StorageContext storageContext)
        {
            this.Storage = storage;
            this.StorageContext = storageContext;
            this.EntitiesMap = new Dictionary<Guid, T>();
        }

        protected StorageContext StorageContext { get; private set; }

        protected IStorage Storage { get; private set; }

        protected IDictionary<Guid, T> EntitiesMap { get; private set; }
    }
}