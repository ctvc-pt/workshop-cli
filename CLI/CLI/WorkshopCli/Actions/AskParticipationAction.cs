namespace workshopCli
{
    public class AskParticipationAction : IAction
    {
        public GuideCli Cli;

        public AskParticipationAction(GuideCli cli)
        {
            Cli = cli;
        }

        public void Execute()
        {
            if (Cli.session != null && !string.IsNullOrEmpty(Cli.session.StepId))
            {
                return;
            }

            Cli.session.Participation = "sim";
        }
    }
}
