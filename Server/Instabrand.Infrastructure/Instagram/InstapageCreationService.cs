using Instabrand.Domain.Instapage;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Instabrand.Infrastructure.Instagram
{
    public sealed class InstapageCreationService
    {
        private readonly InstagramApi _instagramApi;
        private readonly InstagramGraphApi _instagramGraphApi;

        public InstapageCreationService(InstagramApi instagramApi, InstagramGraphApi instagramGraphApi)
        {
            _instagramApi = instagramApi;
            _instagramGraphApi = instagramGraphApi;
        }

        public async Task<Instapage> CreateInstapage(Guid userId, string code, CancellationToken cancellationToken)
        {
            var authData = await _instagramApi.Auth(code, cancellationToken);

            var instagramLogin = await _instagramGraphApi.GetUsername(authData.AccessToken, cancellationToken);

            return new Instapage(
                id: Guid.NewGuid(),
                userId: userId, 
                instagramLogin: instagramLogin.Username,
                instagramId: authData.UserId,
                accessToken: authData.AccessToken);
        }
    }
}
