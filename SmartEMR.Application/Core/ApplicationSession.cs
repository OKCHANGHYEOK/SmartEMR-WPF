using SmartEMR.Domain.DTOs;
using SmartEMR.Domain.Entities;
using SmartEMR.Infrastructure;

namespace SmartEMR.Application.Core
{
    public class ApplicationSession : ITokenProvider
    {
        private TokenResponse _token = new();
        public MemberUser MemberUser { get; private set; } = new();

        public TokenResponse GetToken()
        {
            return _token;
        }

        public void SetToken(TokenResponse token)
        {
            if (token == null) return;
            _token = token;
        }

        // 유저 정보 세팅
        public void SetMemberUser(MemberUser? item)
        {
            if (item == null) return;
            this.MemberUser = item;
        }
    }
}