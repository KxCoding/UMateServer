using System;
namespace UMateModel.Models
{
    public class LoginResponse: CommonResponse
    {
        public string UserId { get; set; }
        public string Token { get; set; }
    }
}
