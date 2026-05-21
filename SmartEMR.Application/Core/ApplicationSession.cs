using SmartEMR.Domain.DTOs;
using SmartEMR.Domain.Entities;
using SmartEMR.Infrastructure;

namespace SmartEMR.Application.Core
{
    public class ApplicationSession : ITokenProvider
    {
        private TokenResponse? _token = null;
        private Member? _member = null;
        private MemberUser? _user = null;

        public Member? Member => _member;
        public MemberUser? MemberUser => _user;

        public TokenResponse? GetToken()
        {
            if (_token == null)
            {
                _token = new();
            }

            return _token;
        }

        public void SetToken(TokenResponse token)
        {
            if (token == null) return;
            _token = token;
        }

        public void SetMember(Member? Item)
        {
            if (Item == null) return;
            _member = Item;
        }

        // 유저 정보 세팅
        public void SetMemberUser(MemberUser? item)
        {
            if (item == null) return;
            _user = item;
        }
    }
}