namespace Module.AssessmentFunctions.Commands
{
    public sealed class BadResponse
    {
        public BadResponse(string error)
        {
            Error = error;
        }
        public string Error { get; set; }
    }
}
