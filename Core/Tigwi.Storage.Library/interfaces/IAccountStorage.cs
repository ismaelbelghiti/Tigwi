﻿#region copyright
// Copyright (c) 2012, TIGWI
// All rights reserved.
// Distributed under  BSD 2-Clause license
#endregion
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Tigwi.Storage.Library
{
    public interface IAccountStorage
    {
        /// <summary>
        /// Return the id of an account given its name
        /// </summary>
        /// <exception cref="AccountNotFound">if no account has this name</exception>
        Guid GetId(string name);
        /// <summary>
        /// Return the infos related to the given account
        /// </summary>
        /// <exception cref="AccountNotFound">if no account has this ID</exception>
        IAccountInfo GetInfo(Guid accountId);
        /// <summary>
        /// Update informations about the given account
        /// </summary>
        /// <exception cref="AccountNotFound">if no account has ID=accountId</exception>
        void SetInfo(Guid accountId, string description);

        /// <summary>
        /// Get the users who can post with the given account
        /// </summary>
        /// <exception cref="AccountNotFound"> if no account has this id</exception>
        HashSet<Guid> GetUsers(Guid accountId);

        /// <summary>
        /// Get the admin of the given account
        /// </summary>
        /// <exception cref="AccountNotFound">if no account has this ID</exception>
        Guid GetAdminId(Guid accountId);
        /// <summary>
        /// Set the admin of the given account
        /// </summary>
        /// <exception cref="AccountNotFound">if no account has this ID</exception>
        /// <exception cref="UserNotFound">if no user has this ID</exception>
        void SetAdminId(Guid accountId, Guid userId);
        
        /// <summary>
        /// Give the given user the right to post with the given account
        /// </summary>
        /// <exception cref="AccountNotFound">if no account has this id</exception>
        /// <exception cref="UserNotFound">if no user has this id</exception>
        void Add(Guid accountId, Guid userId);
        /// <summary>
        /// Remove the right to the given user to post with the given account
        /// Doesn't do anything if the user is not in the user group
        /// </summary>
        /// <exception cref="UserIsAdmin">if you try to remove the administrator from the user groups</exception>
        void Remove(Guid accountId, Guid userId);

        /// <summary>
        /// Create an account and return its ID
        /// </summary>
        /// <exception cref="UserNotFound">if no user has this ID</exception>
        /// <exception cref="AccountAlreadyExists">if the name is already used</exception>
        Guid Create(Guid adminId, string name, string description, bool bypassNameReservation = false);
        /// <summary>
        /// Delete the given account
        /// Doesn't do anything if the account doesn't exists
        /// </summary>
        void Delete(Guid accountId);

        /// <summary>
        /// Allow you to reserve an account name for futur use
        /// If you want to use this name, you will have to set bypassNameReservation to true in account.create
        /// </summary>
        /// <returns>false if the name was already reserved, true overwise</returns>
        bool ReserveAccountName(string accountName);

        HashSet<string> Autocompletion(string nameBegining, int maxNameNumber);
    }
}
