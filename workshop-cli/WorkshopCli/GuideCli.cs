using Sharprompt;


namespace workshop_cli;

public class GuideCli
{
    private readonly Guide guide;
    
    public GuideCli(Guide guide)
    {
        this.guide = guide;
    }

    public void Run()
    {
        Console.WriteLine("Bem vindo ao Workshop de Love2D");
        Console.WriteLine("Pra começar reponde a algumas perguntas");
        
       

        foreach (var step in guide.Steps)
        {
            Console.WriteLine(step.Message);
            

            switch (step.Type)
            {
                case "question":
                    var answer = Prompt.Input<string>(":");
                    Console.WriteLine($"Resposta: {answer}");
                    break;
                
                case "information":
                    Prompt.Confirm("Quando completares a taréfa Avança para a frente", false);
                    Console.WriteLine("");
                    break;


                case "exercise":
                    while (true)
                    {
                        //Prompt.Confirm("Já acabaste o exercicio? ", false);
                        var input = Prompt.Input<string>("Quando acabares O exercicio escreve 'acabei' na consola", "");
                        if (input.ToLower() != "y") continue;
                        Console.WriteLine("Great job!");
                        break;
                    }

                    break;
                case "challenge":
                    while (true)
                    {
                        var inputCh = Prompt.Input<string>("Quando acabares o desafio escreve 'acabei' na consola", "");
                        if (inputCh.ToLower() != "y") continue;
                        Console.WriteLine("Great job!");
                        break;
                    }

                    break;
            }

            Console.WriteLine();
        }

        Console.WriteLine("Congratulations, you have completed the workshop!");
    }
}