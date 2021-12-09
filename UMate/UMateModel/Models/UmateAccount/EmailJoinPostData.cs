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
        public string RealName { get; set; }

        /// <summary>
        /// 닉네임
        /// </summary>
        public string NickName { get; set; }

        /// <summary>
        /// 대학교 아이디
        /// </summary>
        public int UniversityId { get; set; }

        /// <summary>
        /// 입학 연도
        /// </summary>
        public string YearOfAdmission { get; set; }
    }
}
