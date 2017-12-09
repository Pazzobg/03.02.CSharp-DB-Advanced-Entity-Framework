namespace Instagraph.DataProcessor.Dto.Import
{
    using System.ComponentModel.DataAnnotations;

    public class PictureDto
    {
        [Required]
        public string Path { get; set; }

        public decimal Size { get; set; }
    }
}
