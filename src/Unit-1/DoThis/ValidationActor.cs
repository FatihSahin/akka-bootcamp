using Akka.Actor;

namespace WinTail
{
    public class ValidationActor : UntypedActor
    {
        private readonly IActorRef _consoleWriterActor;

        public ValidationActor(IActorRef consoleWriterActor)
        {
            _consoleWriterActor = consoleWriterActor;
        }

        protected override void OnReceive(object message)
        {
            var msg = message as string;

            if (string.IsNullOrEmpty(msg))
            {
                _consoleWriterActor.Tell(new Messages.NullInputError("No input received."));
            }
            else
            {
                bool valid = IsValid(msg);
                if (valid)
                {
                    _consoleWriterActor.Tell(new Messages.InputSuccess("Thanks you! It's a valid message."));
                }
                else
                {
                    _consoleWriterActor.Tell(new Messages.ValidationError("Invalid: input had odd number of characters."));
                }
            }

            Sender.Tell(new Messages.ContinueProcessing());
        }

        /// <summary>
        /// Validates <see cref="message"/>. 
        /// </summary>
        /// <param name="message">The message.</param>
        /// <returns></returns>
        private bool IsValid(string message)
        {
            return message.Length % 2 == 0;
        }
    }
}
