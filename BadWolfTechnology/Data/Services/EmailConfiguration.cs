using BadWolfTechnology.Data.Interfaces;

namespace BadWolfTechnology.Data.Services
{
    public class EmailConfiguration: IEmailConfiguration
    {
        public const string Position = "EmailConfiguration";
        public string From { get; set; }
        public string SmtpServer { get; set; }
        public int Port { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
    }
}
