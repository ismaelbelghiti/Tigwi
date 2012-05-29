#region copyright
// Copyright (c) 2012, TIGWI
// All rights reserved.
// Distributed under  BSD 2-Clause license
#endregion
namespace Tigwi.UI.Models.Storage
{
    using System;
    using System.Collections.Generic;

    using Tigwi.Storage.Library;

    public class StorageEntityRepository<T>
    {
        protected StorageEntityRepository(StorageContext storageContext)
        {
            this.StorageContext = storageContext;
            this.EntitiesMap = new Dictionary<Guid, T>();
        }

        protected StorageContext StorageContext { get; private set; }

        protected IStorage Storage
        {
            get
            {
                return this.StorageContext.StorageObj;
            }
        }

        protected IDictionary<Guid, T> EntitiesMap { get; private set; }
    }
}