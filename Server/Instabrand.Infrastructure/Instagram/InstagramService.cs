using Instabrand.Domain;
using Newtonsoft.Json.Linq;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Instabrand.Infrastructure.Instagram
{
    public sealed class InstagramService : IInstragramService
    {
        private readonly HttpClient _httpClient;

        public InstagramService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<InstagramUser> GetUser(string username, CancellationToken cancellationToken)
        {
            var user = new InstagramUser(username);

            try
            {
                using var response = await _httpClient.GetAsync($"{username}/?__a=1", cancellationToken);

                if (!response.IsSuccessStatusCode)
                {
                    if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                        return null;

                    throw new IInstragramService.ServiceUnavailableException();
                }

                JObject jObject = JObject.Parse(await response.Content.ReadAsStringAsync(cancellationToken));
                user.Name = jObject.SelectToken("graphql").SelectToken("user").SelectToken("full_name").Select(o => (string)o).SingleOrDefault();
            }
            catch (HttpRequestException)
            {
                throw new IInstragramService.ServiceUnavailableException();
            }

            return user;
        }
    }
}
