namespace MicroLine.Services.Airline.Tests.Integration.Common;

public static class Constants
{
    public const string ExceptionCode = "exceptionCode";

    /// <summary>
    /// RFC 9110: <see href="https://tools.ietf.org/html/rfc9110" />
    /// </summary>
    public static class Rfc9110
    {
        public static class StatusCodes
        {
            /// <summary>
            /// RFC 9110 - Section 15.5.1 ( status code): <see href="https://tools.ietf.org/html/rfc9110#section-15.5.1" />
            /// </summary>
            public const string BadRequest400 = "https://tools.ietf.org/html/rfc9110#section-15.5.1";

            /// <summary>
            /// RFC 9110 - Section 15.5.5 (404 Not Found status code): <see href="https://tools.ietf.org/html/rfc9110#section-15.5.5" />
            /// </summary>
            public const string NotFound404 = "https://tools.ietf.org/html/rfc9110#section-15.5.5";
        }

        public static class Titles
        {
            public const string BadRequest = "Bad Request";
            public const string NotFound = "Not Found";
        }

    }

}
