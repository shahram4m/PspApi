using System;

namespace Sample.Application.ViewModels
{
    /// <summary>
    /// The custom exception class.
    /// </summary>
    public class CustomException : Exception
    {
        /// <summary>
        /// Gets or sets the error code.
        /// </summary>
        public ErrorCodes ErrorCode { get; set; }

        /// <summary>
        /// Gets or sets the error message.
        /// </summary>
        public String ErrorMessage { get; set; }

        /// <summary>
        /// Gets or sets the request.
        /// </summary>
        public String Request { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="CustomException"/> class.
        /// </summary>
        /// <param name="errCode">The errCode.</param>
        /// <param name="errorMessage">The errorMessage.</param>
        /// <param name="inner">The inner.</param>
        public CustomException(ErrorCodes errCode, String errorMessage, Exception inner) :

            base(inner.Message, inner)
        {

            ErrorCode = errCode;

            ErrorMessage = errorMessage;

        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CustomException"/> class.
        /// </summary>
        /// <param name="errCode">The errCode.</param>
        /// <param name="errorMessage">The errorMessage.</param>
        /// <param name="inner">The inner.</param>
        /// <param name="request">The request.</param>
        public CustomException(ErrorCodes errCode, String errorMessage, Exception inner, string request) :

            base(inner.Message, inner)
        {

            ErrorCode = errCode;

            ErrorMessage = errorMessage;
            Request = request;

        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CustomException"/> class.
        /// </summary>
        /// <param name="errCode">The errCode.</param>
        /// <param name="errorMessage">The errorMessage.</param>
        public CustomException(ErrorCodes errCode, String errorMessage)
        {

            ErrorCode = errCode;

            ErrorMessage = errorMessage;

        }
    }
}
