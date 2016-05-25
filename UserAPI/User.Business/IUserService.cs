namespace User.Business
{
    using User.Shared;
    using User.Shared.Models;

    /// <summary>
    /// Users business service
    /// </summary>
    public interface IUserService
    {
        /// <summary>
        /// Adds the or update.
        /// </summary>
        /// <param name="user">The user.</param>
        /// <param name="relatedId">The related identifier.</param>
        OperationResult AddOrUpdate(UserBusinessModel user, int? relatedId = null);

        /// <summary>
        /// Deletes the specified identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        OperationResult Delete(int id);

        /// <summary>
        /// Gets the specified identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        OperationResult Get(int id);

        /// <summary>
        /// Gets all users.
        /// </summary>
        OperationResult GetAll();
    }
}
