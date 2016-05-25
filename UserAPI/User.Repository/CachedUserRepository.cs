using System;
using System.Web;
using System.Collections.Generic;
using System.Linq;
using User.Shared.Models;
using User.Shared;

namespace User.Repository
{
    /// <summary>
    /// Cache user repository
    /// </summary>
    /// <seealso cref="User.Repository.ICachedUserRepository" />
    public class CachedUserRepository : ICachedUserRepository
    {
        /// <summary>
        /// The cache lock object
        /// </summary>
        private static readonly object CacheLockObject = new object();

        /// <summary>
        /// The users
        /// </summary>
        private static Dictionary<int, UserBusinessModel> users;

        /// <summary>
        /// The user cache key used to store the Users
        /// Dictionary in Http Runtime Cache
        /// </summary>
        private const string USERS_CACHE_KEY = "Users";

        /// <summary>
        /// The cach e_ expiratio n_ minutes
        /// </summary>
        private const int CACHE_EXPIRATION_MINUTES = 60;

        /// <summary>
        /// The number of users to generate for test data
        /// </summary>
        private const int NUMBER_OF_USERS_TO_GENERATE = 3;

        public CachedUserRepository()
        {
            if (users == null)
            {
                InitializeSomeDummyUsers();
            }
        }


        /// <summary>
        /// Initializes some dummy users.
        /// </summary>
        private void InitializeSomeDummyUsers()
        {
            users = new Dictionary<int, UserBusinessModel>();

            for (int i = 1; i <= NUMBER_OF_USERS_TO_GENERATE; i++)
            {
                users.Add(i, new UserBusinessModel
                {
                    ForeName = Faker.NameFaker.FirstName(),
                    Surname = Faker.NameFaker.LastName(),
                    Email = Faker.InternetFaker.Email(),
                    Created = Faker.DateTimeFaker.DateTime(DateTime.Now.Subtract(new TimeSpan(100, 0, 0, 0)), DateTime.Now)
                });
            }

            // add dummy users to cache
            UpdateCache();
        }


        /// <summary>
        /// Adds or updates the user.
        /// </summary>
        /// <param name="user">The user.</param>
        /// <param name="relatedId">The related identifier.</param>
        /// <returns>Operation result</returns>
        public OperationResult AddOrUpdateUser(UserBusinessModel user, int? relatedId = null)
        {
            var cachedUsers = HttpRuntime.Cache[USERS_CACHE_KEY] as Dictionary<int, UserBusinessModel>;

            var operationResult = new OperationResult { Data = user, Success = true };

            // Update existing User
            if (relatedId.HasValue)
            {
                if (cachedUsers.ContainsKey(relatedId.Value))
                {
                    users[relatedId.Value].ForeName = user.ForeName;
                    users[relatedId.Value].Surname = user.Surname;
                    users[relatedId.Value].Email = user.Email;

                    operationResult.Message = ResponseMessages.UserUpdatedSuccessfully.GetMessageResponse();
                    operationResult.RelatedId = relatedId.Value;
                }
            }
            // Add new User
            else
            {
                var nextId = users.Max(x => x.Key);

                // create next user id
                ++nextId;

                user.Created = DateTime.Now;
                users.Add(nextId, user);

                operationResult.Message = ResponseMessages.UserAddedSuccessfully.GetMessageResponse();
                operationResult.RelatedId = nextId;
            }

            UpdateCache();

            return operationResult;
        }

        /// <summary>
        /// Gets the user.
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        /// <returns>Operation result</returns>
        public OperationResult GetUser(int userId)
        {
            var cachedUsers = HttpRuntime.Cache[USERS_CACHE_KEY] as Dictionary<int, UserBusinessModel>;

            var operationResult = new OperationResult { RelatedId = userId };
            operationResult.Success = false;
            operationResult.Message = ResponseMessages.UserNonExistant.GetMessageResponse();

            if (cachedUsers == null)
            {
                if (users.ContainsKey(userId))
                {
                    // update whole cache
                    UpdateCache();

                    operationResult.Success = true;
                    operationResult.Message = ResponseMessages.OperationSuccess.GetMessageResponse();
                    operationResult.Data = users[userId];
                }
            }
            else
            {
                lock (CacheLockObject)
                {
                    if (cachedUsers.ContainsKey(userId))
                    {
                        operationResult.Success = true;
                        operationResult.Message = ResponseMessages.OperationSuccess.GetMessageResponse();
                        operationResult.Data = cachedUsers[userId];
                    }
                }
            }

            return operationResult;
        }

        /// <summary>
        /// Removes the user.
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        /// <returns>Operation result</returns>
        public OperationResult RemoveUser(int userId)
        {
            var operationResult = new OperationResult { RelatedId = userId };

            if (users.ContainsKey(userId))
            {
                users.Remove(userId);

                UpdateCache();

                operationResult.Success = true;
                operationResult.Message = ResponseMessages.OperationSuccess.GetMessageResponse();
            }
            else
            {
                operationResult.Success = false;
                operationResult.Message = ResponseMessages.UserNonExistant.GetMessageResponse();
            }

            return operationResult;
        }

        /// <summary>
        /// Updates the cache.
        /// </summary>
        private void UpdateCache()
        {
            lock (CacheLockObject)
            {
                // update whole cache
                HttpRuntime.Cache.Insert(USERS_CACHE_KEY, 
                                            users, 
                                            null,
                                            DateTime.Now.AddMinutes(CACHE_EXPIRATION_MINUTES), 
                                            TimeSpan.Zero);
            }
        }

        /// <summary>
        /// Gets all existing users.
        /// </summary>
        /// <returns>Operation result</returns>
        public OperationResult GetUsers()
        {
            var operationResult = new OperationResult
            {
                Success = true,
                Message = ResponseMessages.OperationSuccess.GetMessageResponse(),
            };

            var cachedUsers = HttpRuntime.Cache[USERS_CACHE_KEY] as Dictionary<int, UserBusinessModel>;

            // if the cache is present
            if (cachedUsers != null && cachedUsers.Count == users.Count)
            {
                operationResult.Data = cachedUsers;
            }
            else
            {
                operationResult.Data = users;
                UpdateCache();
            }

            return operationResult;
        }
    }
}
