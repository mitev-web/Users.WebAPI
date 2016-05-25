using System;

namespace User.Shared.Models
{
    /// <summary>
    /// User Business model 
    /// </summary>
    public class UserBusinessModel
    {
        /// <summary>
        /// Gets or sets the email.
        /// </summary>
        /// <value>
        /// The email.
        /// </value>
        public string Email { get; set; }

        /// <summary>
        /// Gets or sets the name of the fore.
        /// </summary>
        /// <value>
        /// The name of the fore.
        /// </value>
        public string ForeName { get; set; }

        /// <summary>
        /// Gets or sets the surname.
        /// </summary>
        /// <value>
        /// The surname.
        /// </value>
        public string Surname { get; set; }

        /// <summary>
        /// Gets or sets the created.
        /// </summary>
        /// <value>
        /// The created.
        /// </value>
        public DateTime Created { get; set; }
    }
}
