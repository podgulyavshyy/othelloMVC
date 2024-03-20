namespace Othello.Model;

public interface IView
{
    void ShowView(Cell[,] field);
    void FinishGame();
}