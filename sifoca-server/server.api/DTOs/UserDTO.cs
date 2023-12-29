namespace server.api.DTOs
{
    public class UserDTO
    {
        public string? NomeCompleto { get; set; }
        public string? Departamento { get; set; }
        public DateTime DataNascimento { get; set; }
        public string? Telefone { get; set; }
        public string? Email { get; set; }
        public string? Usuario { get; set; }
        public string? Senha { get; set; }
    }
}