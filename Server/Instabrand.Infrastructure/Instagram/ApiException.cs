using System;

namespace Instabrand.Infrastructure.Instagram
{
    public sealed class ApiException : Exception
    {
        public System.Net.HttpStatusCode StatusCode { get; init; }
        public string Reason { get; init; }
    }
}
