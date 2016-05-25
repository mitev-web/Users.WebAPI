namespace User.Shared.Extensions
{
    using User.Shared.Models;

    /// <summary>
    /// Client to business and business to client mappers
    /// </summary>
    public static class Mappers
    {
        /// <summary>
        /// Transforms current business object to presentation object
        /// </summary>
        /// <param name="obj">The business object.</param>
        /// <param name="relatedId">The related identifier.</param>
        /// <returns>UserViewModel</returns>
        public static UserViewModel ToViewModel(this UserBusinessModel obj, int relatedId)
        {
            return new UserViewModel
            {
                ForeName = obj.ForeName,
                SurName = obj.Surname,
                Created = obj.Created,
                Email = obj.Email,
                Id = relatedId
            };
        }

        /// <summary>
        /// Transforms current presentation object to business
        /// </summary>
        /// <param name="obj">The business object.</param>
        /// <returns>UserBusinessModel</returns>
        public static UserBusinessModel ToBiz(this UserViewModel obj)
        {
            return new UserBusinessModel
            {
                ForeName = obj.ForeName,
                Surname = obj.SurName,
                Created = obj.Created,
                Email = obj.Email
            };
        }
    }
}
