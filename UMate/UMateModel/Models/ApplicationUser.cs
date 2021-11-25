using System;
using Microsoft.AspNetCore.Identity;

namespace UMateModel.Models
{
    public class ApplicationUser: IdentityUser
    {
        /// <summary>
        /// 닉네임
        /// </summary>
        public string NickName { get; set; }

        /// <summary>
        /// 입학 연도
        /// </summary>
        public string YearOfAdmission { get; set; }

        /// <summary>
        /// 대학교 아이디
        /// </summary>
        public int? UniversityId { get; set; }

        /// <summary>
        /// 학교 인증 플래그
        /// </summary>
        public bool UniversityConfirmed { get; set; }

        /// <summary>
        /// 선택된 프로필 이미지
        /// </summary>
        public string SelectedProfileImage { get; set; }

        /// <summary>
        /// 가입 일자
        /// </summary>
        public DateTime JoinDate { get; set; }

        /// <summary>
        /// 업데이트 일자
        /// </summary>
        public DateTime UpdateDate { get; set; }
    }
}
