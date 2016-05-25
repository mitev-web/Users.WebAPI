namespace UserAPI.Controllers
{
    using System.Web.Http;
    using User.Business;
    using User.Shared;
    using User.Shared.Extensions;
    using User.Shared.Models;

    /// <summary>
    /// Users Controller
    /// </summary>
    /// <seealso cref="System.Web.Http.ApiController" />
    public class UsersController : ApiController
    {
        /// <summary>
        /// The User Business service
        /// </summary>
        private readonly UserService _userService;

        /// <summary>
        /// Initializes a new instance of the <see cref="UsersController"/> class.
        /// </summary>
        public UsersController()
        {
            _userService = new UserService();
        }

        /// <summary>
        /// Gets a single user
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        /// <returns></returns>
        [HttpGet]
        [Route("api/users/{userId}")]
        public OperationResult GetUser(int userId)
        {
            var operationResult = _userService.Get(userId);

            var userModel = operationResult.Data as UserBusinessModel;

            if (userModel != null)
            {
                operationResult.Data = userModel.ToViewModel(operationResult.RelatedId);
            }

            return operationResult;
        }

        /// <summary>
        /// Gets all existing users
        /// </summary>
        [HttpGet]
        [Route("api/users")]
        public OperationResult GetUsers()
        {
            return _userService.GetAll();
        }

        /// <summary>
        /// Deletes existing user
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        [HttpGet]
        [Route("api/users/delete/{userId}")]
        public OperationResult DeleteUser(int userId)
        {
            return _userService.Delete(userId);
        }

        /// <summary>
        /// Creates a new user
        /// </summary>
        /// <param name="userModel">The user model.</param>
        [HttpPost]
        [Route("api/users/add")]
        public OperationResult PostUser([FromBody] UserViewModel userModel)
        {
            return _userService.AddOrUpdate(userModel.ToBiz());
        }

        /// <summary>
        /// Updates existing user
        /// </summary>
        /// <param name="userModel">The user model.</param>
        /// <returns></returns>
        [HttpPut]
        [Route("api/users/update")]
        public OperationResult PutUser([FromBody] UserViewModel userModel)
        {
            return _userService.AddOrUpdate(userModel.ToBiz(), userModel.Id);
        }
    }
}
