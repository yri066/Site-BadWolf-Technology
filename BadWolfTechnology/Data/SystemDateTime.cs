using BadWolfTechnology.Data.Interfaces;

namespace BadWolfTechnology.Data.Services
{
    public class SystemDateTime : IDateTime
    {
        public DateTime Now => DateTime.Now;
        public DateTime UtcNow => DateTime.UtcNow;
        public DateTime Today => DateTime.Today;
    }
}
