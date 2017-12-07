namespace Stations.DataProcessor.Dto.Import
{
    using System.ComponentModel.DataAnnotations;
    using Stations.Models.Enums;

    public class TrainDto
    {
        [Required]
        [MaxLength(10)]
        public string TrainNumber { get; set; } 

        public string Type { get; set; } = "HighSpeed";

        public SeatDto[] Seats { get; set; } = new SeatDto[0];
    }
}
