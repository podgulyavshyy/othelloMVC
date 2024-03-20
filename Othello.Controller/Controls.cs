namespace Othello.Controller;
using Othello.Model;
using System;

public class Controls
{
    public void StartGame(Model.Game game)
    {
        
        string commandStart = Console.ReadLine();
        if (commandStart == "bot")
        {
            game.StartGame(true);
        }
        else if(commandStart == "pvp")
        {
            game.StartGame(false);
        }
        while (true)
        {
            string command = Console.ReadLine();

            var splitCommand = command.Split(" ");

            game.MakeMove(int.Parse(splitCommand[0]), int.Parse(splitCommand[1]));
            if (game.CurrentPlayer.isBot == true)
            {
                game.MakeBotMove();
            }
        }
    }
}