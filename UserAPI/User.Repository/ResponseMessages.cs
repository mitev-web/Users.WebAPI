namespace User.Repository
{
    /// <summary>
    /// Response messages enumeration
    /// </summary>
    public enum ResponseMessages
    {   
        OperationSuccess = 0,
        UserNonExistant,
        UserAddedSuccessfully,
        UserUpdatedSuccessfully,
    }

    /// <summary>
    /// Parser for ResponseMessages enum
    /// </summary>
    public static class ResponseParser
    {
        /// <summary>
        /// Gets the message response message by the response enum
        /// </summary>
        /// <param name="response">The response.</param>
        /// <returns>The response message</returns>
        public static string GetMessageResponse(this ResponseMessages response)
        {
            switch (response)
            {
                case ResponseMessages.OperationSuccess:
                    return "Operation Success!";

                case ResponseMessages.UserAddedSuccessfully:
                    return "User added successfuly!";

                case ResponseMessages.UserNonExistant:
                    return "This user does not exist!";

                case ResponseMessages.UserUpdatedSuccessfully:
                    return "User has been updated successfully!";
            }

            return string.Empty;
        }
    }
}
