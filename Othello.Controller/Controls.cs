namespace Othello.Controller;
using Othello.Model;
using System;

public class Controls
{
    public void StartGame(Model.Game game)
    {
        game.StartGame();
        while (true)
        {
            string command = Console.ReadLine();
            if (command == "bot")
            {
                game.MakeBotMove();
            }
            else
            {
                var splitCommand = command.Split(" ");

                game.MakeMove(int.Parse(splitCommand[0]), int.Parse(splitCommand[1]));
            }
        }
    }
}