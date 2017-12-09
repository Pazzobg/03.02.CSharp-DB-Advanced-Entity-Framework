namespace Instagraph.DataProcessor.Dto.Export
{
    using System.Xml.Serialization;

    [XmlType("user")]
    public class UserCommentDto
    {
        public string Username { get; set; }

        public int MostComments { get; set; }
    }
}
