namespace Stations.App
{
    using System.Linq;
    using AutoMapper;
    using Stations.Models;
    using Stations.DataProcessor.Dto;
    using Stations.DataProcessor.Dto.Import;

    public class StationsProfile : Profile
    {
        // Configure your AutoMapper here if you wish to use it. If not, DO NOT DELETE THIS CLASS
        public StationsProfile()
        {
            CreateMap<TrainDto, Train>();
        }
    }
}
