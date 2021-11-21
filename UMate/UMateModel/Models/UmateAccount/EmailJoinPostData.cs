using System;
namespace UMateModel.Models
{
    /// <summary>
    /// 회원가입 POST 모델
    /// </summary>
    public class EmailJoinPostData: EmailLoginPostData
    {
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

        /// <summary>
        /// 가입 일자
        /// </summary>
        public DateTime JoinDate { get; set; }
    }
}
