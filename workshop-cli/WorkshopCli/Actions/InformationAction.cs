﻿using Sharprompt;

namespace workshopCli;

public class InformationAction : IAction
{
    
    public void Execute()
    {
        Console.ForegroundColor = ConsoleColor.Yellow;
        ExerciseHelper.PromptAnswerAndConfirm( "Escreve 'proximo' ou 'p' para avançar (ou 'ajuda')" );
        
    }
}