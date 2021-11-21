using System;
namespace UMateModel.Models
{
    /// <summary>
    /// 로그인 POST 모델
    /// </summary>
    public class EmailLoginPostData
    {
        /// <summary>
        /// 이메일
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// 비밀번호
        /// </summary>
        public string Password { get; set; }
    }
}
