namespace Stations.DataProcessor.Dto.Import.TicketInnerDtos
{
    using System.ComponentModel.DataAnnotations;
    using System.Xml.Serialization;

    [XmlType("Card")]
    public class CardDto
    {
        [Required]
        [MaxLength(128)]
        [XmlAttribute("Name")]
        public string Name { get; set; }
    }
}
