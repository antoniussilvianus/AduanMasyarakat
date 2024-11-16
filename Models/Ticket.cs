namespace AduanMasyarakat.Models
{
    public class Ticket
    {
        public int Id { get; set; }
        public required string Nama { get; set; }
        public required string NIK { get; set; }
        public required string Provinsi { get; set; }
        public required string Kabupaten { get; set; }
        public required string Kota { get; set; }
        public required string PhoneNumber { get; set; }
        public required DateTime IncidentDate { get; set; }
        public required string Location { get; set; }
        public required string Description { get; set; }
        public string Status { get; set; } = "Berjalan";
    }

}
