namespace User.Repository
{
    using User.Shared;
    using User.Shared.Models;

    /// <summary>
    /// Interface for Cached users repository
    /// </summary>
    interface ICachedUserRepository
    {
        /// <summary>
        /// Adds the or update user.
        /// </summary>
        /// <param name="user">The user.</param>
        /// <param name="relatedId">The related identifier.</param>
        OperationResult AddOrUpdateUser(UserBusinessModel user, int? relatedId = null);

        /// <summary>
        /// Gets the user.
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        OperationResult GetUser(int userId);

        /// <summary>
        /// Gets the users.
        /// </summary>
        OperationResult GetUsers();

        /// <summary>
        /// Removes the user.
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        OperationResult RemoveUser(int userId);
    }
}
