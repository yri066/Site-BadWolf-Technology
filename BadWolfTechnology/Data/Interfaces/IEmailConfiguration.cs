namespace BadWolfTechnology.Data.Interfaces
{
    public interface IEmailConfiguration
    {
        string From { get; set; }
        string SmtpServer { get; set; }
        int Port { get; set; }
        string Email { get; set; }
        string Password { get; set; }
    }
}
