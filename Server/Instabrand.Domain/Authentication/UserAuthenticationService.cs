using System.Threading;
using System.Threading.Tasks;

namespace Instabrand.Domain.Authentication
{
    public sealed class UserAuthenticationService
    {
        private readonly IUserGetter _userRepository;
        private readonly IRefreshTokenStore _refreshTokenStore;
        private readonly IAccessTokenFactory _accessTokenFactory;
        private readonly IPasswordHasher _passwordHasher;

        public UserAuthenticationService(IUserGetter userRepository,
            IRefreshTokenStore refreshTokenStore,
            IAccessTokenFactory accessTokenFactory,
            IPasswordHasher passwordHasher)
        {
            _userRepository = userRepository;
            _refreshTokenStore = refreshTokenStore;
            _accessTokenFactory = accessTokenFactory;
            _passwordHasher = passwordHasher;
        }

        public async Task<Token> AuthenticationByPassword(string email, string password,
            CancellationToken cancellationToken)
        {
            var user = await _userRepository.FindByEmail(email, cancellationToken);

            if (user == null)
                throw new UnauthorizedException();

            if (user.EmailState == EmailState.Unconfirmed)
                throw new UnconfirmedException();

            if (!_passwordHasher.VerifyHashedPassword(user.PasswordHash, password))
                throw new UnauthorizedException();

            var refreshToken = await _refreshTokenStore.Add(user.Id, cancellationToken);

            var accessToken = await _accessTokenFactory.Create(user, cancellationToken);

            return new Token(accessToken.Value, accessToken.ExpiresIn, refreshToken);
        }

        public async Task<Token> AuthenticationByRefreshToken(string refreshToken, CancellationToken cancellationToken)
        {
            var newRefreshToken = await _refreshTokenStore.Reissue(refreshToken, cancellationToken);

            if (newRefreshToken == null)
                throw new UnauthorizedException();

            var user = await _userRepository.Get(newRefreshToken.UserId, cancellationToken);

            var accessToken = await _accessTokenFactory.Create(user, cancellationToken);

            return new Token(accessToken.Value, accessToken.ExpiresIn, newRefreshToken.Value);
        }
    }
}
