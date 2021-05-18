using Instabrand.Domain;
using Instabrand.Domain.Instapage;
using Instabrand.Infrastructure.Instagram.ResponseViews;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Instabrand.Infrastructure.Instagram
{
    public sealed class InstapageCreationService
    {
        private readonly InstagramApi _instagramApi;
        private readonly InstagramGraphApi _instagramGraphApi;
        private readonly IFileStorage _fileStorage;

        public InstapageCreationService(InstagramApi instagramApi, InstagramGraphApi instagramGraphApi,
            IFileStorage fileStorage)
        {
            _instagramApi = instagramApi;
            _instagramGraphApi = instagramGraphApi;
            _fileStorage = fileStorage;
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

        public async Task<IEnumerable<DataView>> GetMedia(string accessToken, int limit, CancellationToken cancellationToken)
        {
            var media = await _instagramGraphApi.GetMedia(accessToken, cancellationToken);

            return media.Data
                .OrderByDescending(o => o.Timestamp)
                .Take(limit);
        }

        public async Task SaveMedia(string accessToken, string instagramLogin, string mediaId, CancellationToken cancellationToken)
        {
            using var imageStream = await _instagramGraphApi.DownloadMedia(accessToken, mediaId, cancellationToken);

            await _fileStorage.SaveInstapostImage(mediaId, instagramLogin, imageStream, cancellationToken);
        }
    }
}
