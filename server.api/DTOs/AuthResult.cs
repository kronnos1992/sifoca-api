namespace server.api.DTOs
{
    public class AuthResult
    {
        public bool Success { get; set; }
        public  string? Token { get; set; }
        public  string? FullUserName { get; set; }
        public  string? UserName { get; set; }
        public  bool IsMaster { get; set; }
        public  string? ErrorMessage { get; set; }
        public  string? SuccessMessage { get; set; }
    }
}