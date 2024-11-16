namespace AduanMasyarakat.Models
{
    public class User
    {
        public int Id { get; set; }
        public required string Nama { get; set; }
        public required string NIK { get; set; }
        public required string Domisili { get; set; }
        public required string PhoneNumber { get; set; }
        public int ReportCount { get; set; } = 0;
        public bool IsBlocked { get; set; } = false;

        public string Role { get; set; }  // Admin, TeamLapangan, atau User
    }
}
