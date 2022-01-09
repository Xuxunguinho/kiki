namespace kiki
{
    public class KikiEvaluatorResultMessage
    {
     

        public KikiEvaluatorResultMessage(string message, KikiEvaluatorMessageType type)
        {
            Message = message;
            Type = type;
        }

        public string Message { get; internal set; }

        public KikiEvaluatorMessageType Type { get; internal set; }
    }
}