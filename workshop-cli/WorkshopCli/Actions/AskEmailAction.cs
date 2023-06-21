using System.Text.RegularExpressions;

namespace workshopCli;

public class AskEmailAction : IAction
{
    public GuideCli Cli;

    public AskEmailAction(GuideCli cli)
    {
        Cli = cli;
    }

    public void Execute()
    {
        string email;
        bool isValidEmail;

        do
        {
            email = ExerciseHelper.PromptAnswerAndPrint();
            isValidEmail = IsValidEmail(email);

            if (!isValidEmail || email == null)
            {
                Console.WriteLine("o e-mail inserido não é valido, pff insere novamente o e-mail!");
            }
        } while (!isValidEmail || email == null);

        Cli.session.Email = email;
    }

    private bool IsValidEmail(string email)
    {
        // Regular expression pattern for email validation
        string pattern = @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$";

        // Use Regex.IsMatch to check if the email matches the pattern
        return Regex.IsMatch(email, pattern);
    }
}

