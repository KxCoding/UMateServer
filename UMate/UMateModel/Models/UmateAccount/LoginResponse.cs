using System;
namespace UMateModel.Models
{
    /// <summary>
    /// 로그인 응답 모델
    /// </summary>
    public class LoginResponse: CommonResponse
    {
        /// <summary>
        /// 유저 ID
        /// </summary>
        public string UserId { get; set; }

        /// <summary>
        /// 이메일
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// 토큰
        /// </summary>
        public string Token { get; set; }

        /// <summary>
        /// 사용자 이름
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// 닉네임
        /// </summary>
        public string NickName { get; set; }

        /// <summary>
        /// 입학 연도
        /// </summary>
        public string YearOfAdmission { get; set; }
    }
}
