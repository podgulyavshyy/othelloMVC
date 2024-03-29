using Othello.Model;

namespace Othello.Controller;

public class BotMoves
{
    public static void MakeBotMove(Game game)
    {

        List<(int, int)> validMoves = game.GetValidMoves();

        if (validMoves.Count == 0)
        {
            Console.WriteLine("No valid moves for the bot player.");
            game.SwitchPlayer();
            return;
        }

        Random random = new Random();
        int randomIndex = random.Next(validMoves.Count);
        (int x, int y) = validMoves[randomIndex];

        game.MakeMove(x, y);
    }
}