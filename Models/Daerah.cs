namespace AduanMasyarakat.Models
{
    public class Daerah
    {
        public int Id { get; set; }
        public required string Provinsi { get; set; }
        public required string Kabupaten { get; set; }
        public required string Kota { get; set; }
        public required string Jalan { get; set; }
    }

}
