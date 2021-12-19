namespace DataEvaluatorX
{
    public class EvaluatorResultMessage
    {
     

        public EvaluatorResultMessage(string message, Enums.EvaluatorMessageType type)
        {
            Message = message;
            Type = type;
        }

        public string Message { get; internal set; }

        public Enums.EvaluatorMessageType Type { get; internal set; }
    }
}