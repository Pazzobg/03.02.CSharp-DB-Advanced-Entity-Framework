namespace Stations.DataProcessor.Dto.Import.TicketInnerDtos
{
    using System.ComponentModel.DataAnnotations;
    using System.Xml.Serialization;

    [XmlType("Trip")]
    public class TripDto
    {
        [Required]
        public string OriginStation { get; set; }

        [Required]
        public string DestinationStation { get; set; }

        [Required]
        public string DepartureTime { get; set; }
    }
}
