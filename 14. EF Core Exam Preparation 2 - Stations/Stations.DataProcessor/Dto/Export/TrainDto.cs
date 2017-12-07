namespace Stations.DataProcessor.Dto.Export
{
    public class TrainDto
    {
        /*  "TrainNumber": "PU17333",
            "DelayedTimes": 2,
            "MaxDelayedTime": "23:02:00"
        */

        public string TrainNumber { get; set; }

        public int DelayedTimes { get; set; } = 0;

        public string MaxDelayedTime { get; set; }
    }
}
