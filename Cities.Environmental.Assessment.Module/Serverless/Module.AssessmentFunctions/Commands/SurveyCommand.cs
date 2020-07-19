using Google.Type;

namespace Module.AssessmentFunctions.Commands
{
    public sealed class SurveyCommand
    {
        public string State { get; set; }
        public string City { get; set; }
        public string Name { get; set; }
        public string FreeResponse { get; set; }
        public int Quality { get; set; }
        public int Political { get; set; }
        public int Crimes { get; set; }
    }
}
