namespace Othello.View;
using Othello.Model;

public class Class1 : IView
{

    private Cell[,] field; 
    public void ShowView(Cell[,] field)
    {
        this.field = field;
        PrintField();
    }

    public void FinishGame()
    {
        Console.WriteLine("Game Finished");
    }

    public void PrintField()
    {
        int FieldSize = 8;
        // Print horizontal index line
        Console.Write("  ");
        for (int i = 0; i < FieldSize; i++)
        {
            Console.Write($"{i} ");
        }
        Console.WriteLine();

        // Print vertical index line and field content
        for (int y = 0; y < FieldSize; y++)
        {
            Console.Write($"{y} ");
            for (int x = 0; x < FieldSize; x++)
            {
                if (field[x, y].IsEmpty)
                {
                    Console.Write("E ");
                }
                else
                {
                    Console.Write(field[x, y].MarkedByPlayer.Name + " ");
                }
            }
            Console.WriteLine();
        }
    }

}