using System;

namespace Sample.Application.ViewModels
{
    /// <summary>
    /// The custom result class.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class CustomResult<T>
    {
        private String _errorDetail = "";

        /// <summary>
        /// Gets or sets the error detail.
        /// </summary>
        public String ErrorDetail
        {
            get { return _errorDetail; }
            set { _errorDetail = value; }
        }

        private String _errorMessage = "";

        /// <summary>
        /// Gets or sets the error message.
        /// </summary>
        public String ErrorMessage
        {
            get { return _errorMessage; }
            set
            {
                _errorMessage = value;
                ErrorExists = true;
            }
        }
        
        private ErrorCodes _errorCode;

        /// <summary>
        /// Gets or sets the error code.
        /// </summary>
        public ErrorCodes ErrorCode
        {
            get { return _errorCode; }
            set
            {
                _errorCode = value;
                ErrorExists = true;
            }
        }

        /// <summary>
        /// Gets or sets the error exists.
        /// </summary>
        public Boolean ErrorExists { get; set; } = false;

        public string Detail { get; set; }

        /// <summary>
        /// Gets or sets the result.
        /// </summary>
        public T Result { get; set; }
    }
}
