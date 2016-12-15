using Akka.Actor;

namespace WinTail
{
    #region Program
    class Program
    {
        public static ActorSystem MyActorSystem;

        static void Main(string[] args)
        {
            // initialize MyActorSystem
            // YOU NEED TO FILL IN HERE
            MyActorSystem = ActorSystem.Create("MyActorSystem");

            //Should not use ctor with typeof!
            //Props consoleWriterProps = Props.Create(typeof(ConsoleWriterActor));
            Props consoleWriterProps = Props.Create<ConsoleWriterActor>();
            IActorRef consoleWriterActor = MyActorSystem.ActorOf(consoleWriterProps, "consoleWriterActor");

            //Props validationActorProps = Props.Create(() => new ValidationActor(consoleWriterActor));
            //IActorRef validationActor = MyActorSystem.ActorOf(validationActorProps, "validationActor");

            //make tailCoordinatorActor
            Props tailCoordinatorProps = Props.Create(() => new TailCoordinatorActor());
            IActorRef tailCoordinatorActor = MyActorSystem.ActorOf(tailCoordinatorProps, "tailCoordinatorActor");

            //Props fileValidatorActorProps = Props.Create(() => new FileValidatorActor(consoleWriterActor, tailCoordinatorActor));
            Props fileValidatorActorProps = Props.Create(() => new FileValidatorActor(consoleWriterActor));
            IActorRef fileValidationActor = MyActorSystem.ActorOf(fileValidatorActorProps, "validationActor");

            //Props consoleReaderProps = Props.Create<ConsoleReaderActor>(validationActor);
            //Props consoleReaderProps = Props.Create<ConsoleReaderActor>(fileValidationActor); //with passing IActorRef
            Props consoleReaderProps = Props.Create<ConsoleReaderActor>(); //this reader actor uses actor selection to resolve an validation actor
            IActorRef consoleReaderActor = MyActorSystem.ActorOf(consoleReaderProps, "consoleReaderActor");

            // tell console reader to begin
            consoleReaderActor.Tell(ConsoleReaderActor.StartCommand);

            // blocks the main thread from exiting until the actor system is shut down
            MyActorSystem.WhenTerminated.Wait();
        }
    }
    #endregion
}
