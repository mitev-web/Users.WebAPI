namespace User.Business
{
    using User.Repository;
    using User.Shared;
    using User.Shared.Models;
    using User.Shared.Extensions;
    using System.Collections.Generic;
    /// <summary>
    /// Users Business Service
    /// </summary>
    /// <seealso cref="User.Business.IUserService" />
    public class UserService : IUserService
    {
        /// <summary>
        /// The _repository
        /// </summary>
        private CachedUserRepository _repository;

        /// <summary>
        /// Initializes a new instance of the <see cref="UserService"/> class.
        /// </summary>
        public UserService()
        {
            _repository = new CachedUserRepository();
        }

        /// <summary>
        /// Adds the or update.
        /// </summary>
        /// <param name="user">The user.</param>
        /// <param name="relatedId">The related identifier.</param>
        /// <returns>Operation result</returns>
        public OperationResult AddOrUpdate(UserBusinessModel user, int? relatedId = default(int?))
        {
            var operationResult =  _repository.AddOrUpdateUser(user, relatedId);

            var userViewModel = (operationResult.Data as UserBusinessModel);

            if (userViewModel == null)
            {
                return new OperationResult { Success = false, Message = "Invalid response" };
            }

            operationResult.Data = userViewModel.ToViewModel(operationResult.RelatedId);

            return operationResult;
        }

        /// <summary>
        /// Deletes the specified identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>Operation result</returns>
        public OperationResult Delete(int id)
        {
            return this._repository.RemoveUser(id);
        }

        /// <summary>
        /// Gets the specified identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>Operation result</returns>
        public OperationResult Get(int id)
        {
            return this._repository.GetUser(id);
        }

        /// <summary>
        /// Gets all existing users
        /// </summary>
        /// <returns>Operation result</returns>
        public OperationResult GetAll()
        {
            var operationResult =  this._repository.GetUsers();

            var userBusinessModels = operationResult.Data as Dictionary<int, UserBusinessModel>;

            var userViewModels = new List<UserViewModel>();

            // maps business users to viewmodels
            if (userBusinessModels != null)
            {
                foreach (var userBusinessModel in userBusinessModels)
                {
                    userViewModels.Add(userBusinessModel.Value.ToViewModel(userBusinessModel.Key));
                }
            }

            operationResult.Data = userViewModels;

            return operationResult;
        }
    }
}
