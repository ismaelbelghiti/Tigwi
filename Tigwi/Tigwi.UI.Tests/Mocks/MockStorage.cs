// -----------------------------------------------------------------------
// <copyright file="MockStorage.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Tigwi.UI.Tests.Mocks
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    using Moq;

    using StorageLibrary;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class MockStorage : IStorage
    {
        private IUserStorage user;

        #region Implementation of IStorage

        public IUserStorage User
        {
            get
            {
                if (this.user == null)
                {
                    var mock = new Mock<IUserStorage>();

                    var map = new Dictionary<Guid, IUserInfo>();
                    var loginMap = new Dictionary<string, Guid>();

                    mock.Setup(storage => storage.Create(It.IsAny<string>(), It.IsAny<string>(), string.Empty)).Returns(
                        (string login, string email) =>
                        {
                            var id = Guid.NewGuid();
                            var mockUser = new Mock<IUserInfo>();
                            mockUser.SetupAllProperties();
                            var userInfo = mockUser.Object;
                            userInfo.Email = email;
                            userInfo.Login = login;
                            if (map.ContainsKey(id))
                            {
                                throw new UserAlreadyExists();
                            }

                            map.Add(id, userInfo);
                            loginMap.Add(login, id);

                            return id;
                        });

                    mock.Setup(storage => storage.Delete(It.IsAny<Guid>())).Callback<Guid>(
                        id =>
                        {
                            IUserInfo userInfo;
                            if (!map.TryGetValue(id, out userInfo))
                            {
                                return;
                            }

                            loginMap.Remove(userInfo.Login);
                            map.Remove(id);
                        });

                    mock.Setup(storage => storage.GetId(It.IsAny<string>())).Returns(
                        (string login) =>
                        {
                            Guid id;
                            if (loginMap.TryGetValue(login, out id))
                            {
                                return id;
                            }

                            throw new UserNotFound();
                        });

                    mock.Setup(storage => storage.GetInfo(It.IsAny<Guid>())).Returns(
                        (Guid id) =>
                        {
                            IUserInfo userInfo;
                            if (map.TryGetValue(id, out userInfo))
                            {
                                return userInfo;
                            }

                            throw new UserNotFound();
                        });

                    mock.Setup(storage => storage.SetInfo(It.IsAny<Guid>(), It.IsAny<string>())).Callback<Guid, string>(
                        (id, email) =>
                        {
                            IUserInfo userInfo;
                            if (map.TryGetValue(id, out userInfo))
                            {
                                userInfo.Email = email;
                            }
                            else
                            {
                                throw new UserNotFound();
                            }
                        });

                    this.user = mock.Object;
                }

                return this.user;
            }
        }

        public IAccountStorage Account
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public IListStorage List
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public IMsgStorage Msg
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        #endregion
    }
}
