namespace Module.AssessmentFunctions.Commands
{
    public sealed class SuccessResponse
    {
        public SuccessResponse(string message)
        {
            Message = message;
        }
        public string Message { get; set; }
    }
}
