namespace FCG.Application.DTOs
{
    public class LoginRequestDTO
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }

    public class AuthResultDTO
    {
        public bool Success { get; set; }
        public string Token { get; set; }
        public string Message { get; set; }
        public UserAuthResponseDTO User { get; set; }
        public DateTime ExpiresAt { get; set; }
    }

    public class UserTokenInfoDTO
    {
        public string Name { get; set; }
        public string Email { get; set; }
    }
}