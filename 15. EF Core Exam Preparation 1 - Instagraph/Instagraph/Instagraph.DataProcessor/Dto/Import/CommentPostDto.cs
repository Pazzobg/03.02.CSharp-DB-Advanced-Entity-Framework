namespace Instagraph.DataProcessor.Dto.Import
{
    using System.Xml.Serialization;

    public class CommentPostDto
    {
        [XmlAttribute("id")]
        public int Id { get; set; }
    }
}
