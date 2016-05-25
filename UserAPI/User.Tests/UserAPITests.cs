namespace User.Tests
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Shared.Models;
    using UserAPI.Controllers;

    /// <summary>
    /// Tests for the User Web API
    /// </summary>
    [TestClass]
    public class UserAPITests
    {
        /// <summary>
        /// Assures the is correctly added by the API
        /// </summary>
        [TestMethod]
        [Description("Ensures that user is added correctly")]
        public void UserIsCorrectlyAdded()
        {
            var foreNameToAdd = Faker.NameFaker.Name();
            var surNameToAdd = Faker.NameFaker.LastName();
            var emailToAdd = Faker.InternetFaker.Email();

            var newUserModel = new UserViewModel
            {
                ForeName = foreNameToAdd,
                SurName = surNameToAdd,
                Email = emailToAdd
            };

            using (var controller = new UsersController())
            {
                var addUserResult = controller.PostUser(newUserModel);

                var addedUserId = addUserResult.RelatedId;

                var result = controller.GetUser(addedUserId);

                var user = result.Data as UserViewModel;

                Assert.IsTrue(result.Success);
                Assert.AreNotEqual(user, null);
                Assert.AreEqual(user.ForeName, foreNameToAdd);
                Assert.AreEqual(user.SurName, surNameToAdd);
                Assert.AreEqual(user.Email, emailToAdd);
            }
        }

        /// <summary>
        /// Assure the user is correctly updated by the API
        /// </summary>
        [TestMethod]
        [Description("Ensures that user is updated correctly")]
        public void UserIsCorrectlyUpdated()
        {
            var addedUserId = 0;

            using (var controller = new UsersController())
            {
                var addUserResult = controller.PostUser(this.GetNewUserModel());
                addedUserId = addUserResult.RelatedId;
            }

            // update the new user after being added
            var updatedForeName = "James";
            var updatedSurName = "Brown";

            var updateUserViewModel = new UserViewModel
            {
                Id = addedUserId,
                ForeName = updatedForeName,
                SurName = updatedSurName,
                Email = "james.brown@gmail.com"
            };

            // Assure the user has been updated with the new values
            using (var controller = new UsersController())
            {
                var updateUserResult = controller.PutUser(updateUserViewModel);

                Assert.IsTrue(updateUserResult.Success);

                var user = updateUserResult.Data as UserViewModel;

                Assert.AreNotEqual(user, null);
                Assert.AreEqual(user.ForeName, updatedForeName);
                Assert.AreEqual(user.SurName, updatedSurName);
            }
        }

        /// <summary>
        /// Assure the user is correctly deleted by the API
        /// </summary>
        [TestMethod]
        [Description("Ensures that the user is correctly deleted")]
        public void UserIsCorrectlyDeleted()
        {
            using (var controller = new UsersController())
            {
                var addUserResult = controller.PostUser(this.GetNewUserModel());

                var deleteUserResult = controller.DeleteUser(addUserResult.RelatedId);

                // assure delete request passed successfully
                Assert.IsTrue(deleteUserResult.Success);

                var getDeletedUser = controller.GetUser(addUserResult.RelatedId);

                // assure there is no such user anmymore
                Assert.IsFalse(getDeletedUser.Success);
            }
        }

        /// <summary>
        /// Assure the user is correctly returned by the API
        /// </summary>
        [TestMethod]
        [Description("Ensures that the user is correctly returned")]
        public void UserIsCorrectlyReturned()
        {
            using (var controller = new UsersController())
            {
                var userModelToAdd = this.GetNewUserModel();

                var addUserResult = controller.PostUser(userModelToAdd);

                var getUserResult = controller.GetUser(addUserResult.RelatedId);

                // assure get request passed successfully
                Assert.IsTrue(getUserResult.Success);

                // assure the properties of the user match the added user
                var returnedUser = getUserResult.Data as UserViewModel;

                // assure correct user model is returned
                Assert.AreNotEqual(returnedUser, null);

                // assure all the user properties match with the added user
                Assert.AreEqual(userModelToAdd.ForeName, returnedUser.ForeName);
                Assert.AreEqual(userModelToAdd.SurName, returnedUser.SurName);
                Assert.AreEqual(userModelToAdd.Email, returnedUser.Email);
            }
        }

        /// <summary>
        /// Used for adding users used for testing
        /// </summary>
        /// <returns>UserViewModel</returns>
        private UserViewModel GetNewUserModel()
        {
            // add new user
            var foreNameToAdd = Faker.NameFaker.Name();
            var surNameToAdd = Faker.NameFaker.LastName();
            var emailToAdd = Faker.InternetFaker.Email();

            var newUserModel = new UserViewModel
            {
                ForeName = foreNameToAdd,
                SurName = surNameToAdd,
                Email = emailToAdd
            };

            return newUserModel;
        }
    }
}
