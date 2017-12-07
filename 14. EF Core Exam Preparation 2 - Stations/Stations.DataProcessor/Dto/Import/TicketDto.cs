namespace Stations.DataProcessor.Dto.Import
{
    using System.ComponentModel.DataAnnotations;
    using Stations.DataProcessor.Dto.Import.TicketInnerDtos;
    using Stations.Models;
    using System.Xml.Serialization;

    [XmlType("Ticket")]
    public class TicketDto
    {
        [Required]
        [XmlAttribute("price")]
        [Range(typeof(decimal), "0", "79228162514264337593543950335")]
        public decimal Price { get; set; }

        [Required]
        [MaxLength(8)]
        [XmlAttribute("seat")]
        [RegularExpression(@"^[A-Z]{2}\d{1,6}$")]
        public string Seat { get; set; }

        [Required]
        public TicketInnerDtos.TripDto Trip { get; set; }

        public TicketInnerDtos.CardDto Card { get; set; }
    }
}
