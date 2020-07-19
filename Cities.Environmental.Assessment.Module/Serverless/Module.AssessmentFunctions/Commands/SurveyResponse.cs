namespace Module.AssessmentFunctions.Commands
{
    public sealed class SurveyResponse
    {
        public long AvgQuality { get; set; }
        public long AvgPolitical { get; set; }
        public long AvgCrimes { get; set; }
        public long TotalResponses { get; set; }
        public long TotalAvg => (AvgQuality + AvgPolitical + AvgCrimes) / 3;
    }
}
