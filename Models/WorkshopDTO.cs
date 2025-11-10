using System.Text.Json;

namespace TecnomBoxes.Models
{
    public class WorkshopDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string Email { get; set; }
        public string Whatsapp { get; set; }
        public bool Active { get; set; }

        //    public AddressDetail? ParsedAddress
        //    {
        //        get
        //        {
        //            if (string.IsNullOrWhiteSpace(Address)) return null;
        //            try
        //            {
        //                return JsonSerializer.Deserialize<AddressDetail>(Address);
        //            }
        //            catch
        //            {
        //                return null;
        //            }
        //        }
        //    }
        //}

        //public class AddressDetail
        //{
        //    public string? Formatted_Address { get; set; }
        //    public Geometry? Geometry { get; set; }
        //}

        //public class Geometry
        //{
        //    public Location? Location { get; set; }
        //}

        //public class Location
        //{
        //    public double Lat { get; set; }
        //    public double Lng { get; set; }
        //}
    }

}
