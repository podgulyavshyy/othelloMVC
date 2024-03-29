namespace Othello.Controller;
using Othello.Model;
using System;

public class Controls
{
    private bool isBotGame;
    public void StartGame(Model.Game game)
    {
        
        string commandStart = Console.ReadLine();
        if (commandStart == "bot")
        {
            isBotGame = true;
            game.StartGame();
        }
        else if(commandStart == "pvp")
        {
            isBotGame = false;
            game.StartGame();
        }
        while (true)
        {
            string command = Console.ReadLine();

            var splitCommand = command.Split(" ");

            game.MakeMove(int.Parse(splitCommand[0]), int.Parse(splitCommand[1]));
            if (isBotGame == true)
            {
                Thread.Sleep(2000);
                BotMoves.MakeBotMove(game);
            }
        }
    }
}