namespace Othello.Model;

public class Game
{
    private const int FieldSize = 8;

    private readonly Player firstPlayer;

    private readonly Player secondPlayer;

    private Cell[,] field;

    private IView _view;

    public Player CurrentPlayer { get; private set; }

    public Player Winner { get; private set; }

    public bool IsEnded { get; private set; }

    public Cell[,] GetField() => field.Clone() as Cell[,];

    public Player GetCellValue(int x, int y) => field[x, y].MarkedByPlayer;


    public Game(Player firstPlayer, Player secondPlayer, IView view)
    {
        this.firstPlayer = firstPlayer;
        this.secondPlayer = secondPlayer;
        _view = view;
    }

    public void StartGame()
    {
        CurrentPlayer = firstPlayer;
        PrepareField();
        _view.ShowView(field);
    }

    public void MakeMove(int x, int y)
    {
        if (field == null)
        {
            throw new Exception("Game wasn't started - call StartGame() at first'");
        }
        
        if (IsEnded)
        {
            throw new Exception($"Can not make move to ({x},{y}) - game is ended");
        }

        if (!MoveValid(x, y))
        {
            Console.WriteLine("Invalid move");
            return;
        }
    
        // Check if the cell is already marked
        if (!field[x, y].IsEmpty)
        {
            Console.WriteLine("Cell is already marked");
            return;
        }

        MarkCell(x, y, CurrentPlayer);
        FlipCells(x, y);
        SwitchPlayer();
        CheckGameEnd();
        _view.ShowView(field);
        if (IsEnded)
        {
            Console.WriteLine("Winner is " + Winner.Name);
            Console.WriteLine("Starting new game");
            StartGame();
        }
    }


    protected bool MoveValid(int x, int y)
    {
        // Check if the cell is empty
        if (!field[x, y].IsEmpty)
        {
            return false;
        }

        // Check if the move flips any pieces in any of the eight directions
        for (int rowDir = -1; rowDir <= 1; rowDir++)
        {
            for (int colDir = -1; colDir <= 1; colDir++)
            {
                // Skip the case when both rowDir and colDir are 0
                if (rowDir == 0 && colDir == 0)
                {
                    continue;
                }

                if (CheckDirection(x, y, rowDir, colDir))
                {
                    return true;
                }
            }
        }

        return false;
    }

    protected bool CheckDirection(int x, int y, int rowDir, int colDir)
    {
        int opponentX = x + rowDir;
        int opponentY = y + colDir;
        //int currentPlayerMark = (int)CurrentPlayer;

        // Check if the first adjacent cell is an opponent's cell
        if (opponentX >= 0 && opponentX < FieldSize && opponentY >= 0 && opponentY < FieldSize &&
            field[opponentX, opponentY].MarkedByPlayer == GetOpponent(CurrentPlayer))
        {
            // Continue checking in the same direction until we find the current player's cell
            while (opponentX >= 0 && opponentX < FieldSize && opponentY >= 0 && opponentY < FieldSize)
            {
                if (field[opponentX, opponentY].MarkedByPlayer == CurrentPlayer)
                {
                    return true; // Valid move, it flips pieces
                }
                else if (field[opponentX, opponentY].IsEmpty)
                {
                    return false; // Invalid move, no pieces are flipped
                }

                opponentX += rowDir;
                opponentY += colDir;
            }
        }

        return false; // Invalid move, no pieces are flipped
    }
    
    
    protected virtual void FlipCells(int x, int y)
    {
        for (int rowDir = -1; rowDir <= 1; rowDir++)
        {
            for (int colDir = -1; colDir <= 1; colDir++)
            {
                // Skip the case when both rowDir and colDir are 0
                if (rowDir == 0 && colDir == 0)
                {
                    continue;
                }

                FlipDirection(x, y, rowDir, colDir);
            }
        }
    }

    protected void FlipDirection(int x, int y, int rowDir, int colDir)
    {
        int currentX = x + rowDir;
        int currentY = y + colDir;

        // Initialize a flag to track if we found any cells to flip
        bool foundOpponentCell = false;

        // Check if the first adjacent cell is an opponent's cell
        if (currentX >= 0 && currentX < FieldSize && currentY >= 0 && currentY < FieldSize &&
            field[currentX, currentY].MarkedByPlayer == GetOpponent(CurrentPlayer))
        {
            // Continue checking in the same direction until we find the current player's cell
            while (currentX >= 0 && currentX < FieldSize && currentY >= 0 && currentY < FieldSize)
            {
                if (field[currentX, currentY].MarkedByPlayer == CurrentPlayer)
                {
                    // If we found at least one opponent's cell followed by the current player's cell, flip the cells
                    if (foundOpponentCell)
                    {
                        // Flip the cells between the current cell and the initial cell
                        while (currentX != x || currentY != y)
                        {
                            field[currentX, currentY].MarkBy(CurrentPlayer);
                            currentX -= rowDir;
                            currentY -= colDir;
                        }
                    }

                    break; // Exit the loop after flipping the cells
                }
                else if (field[currentX, currentY].IsEmpty)
                {
                    break; // Stop flipping if an empty cell is encountered
                }

                // Mark that we found an opponent's cell in this direction
                foundOpponentCell = true;

                currentX += rowDir;
                currentY += colDir;
            }
        }
    }



    

    protected Player GetOpponent(Player currentPlayer)
    {
        return currentPlayer == firstPlayer ? secondPlayer : firstPlayer;
    }


    protected virtual void MarkCell(int x, int y, Player player)
    {
        if (!field[x, y].IsEmpty)
        {
            Console.WriteLine($"Cell at ({x}, {y}) is already marked");
            throw new Exception($"Cell at ({x}, {y}) is already marked");
        }
        else
        {
            field[x, y].MarkBy(player);
        }
    }



    protected virtual void PrepareField()
    {
        field = new Cell[FieldSize, FieldSize];
        for (var x = 0; x < field.GetLength(0); x++)
        {
            for (var y = 0; y < field.GetLength(1); y++)
            {
                field[x, y] = new Cell();
            }
        }
        field[3, 3].MarkBy(firstPlayer);
        field[4, 4].MarkBy(firstPlayer);
        field[3, 4].MarkBy(secondPlayer);
        field[4, 3].MarkBy(secondPlayer);
    }

    private void CheckGameEnd()
    {
        var hasEmptyCells = false;

        foreach (var cell in field)
        {
            if (cell.IsEmpty)
            {
                hasEmptyCells = true;
            }
        }

        if (!hasEmptyCells)
        {
            EndGame();
        }
        /*
        foreach (var row in GetAllRows())
        {
            var player = row[0].MarkedByPlayer;
            if (player != null && row.All(cell => cell.MarkedByPlayer == player))
            {
                EndGame(player);
                break;
            }

            if (row.Any(cell => cell.IsEmpty))
            {
                hasEmptyCells = true;
            }
        }

        if (!hasEmptyCells)
        {
            EndGame();
        }
            
        IEnumerable<List<Cell>> GetAllRows()
        {
            yield return new List<Cell> {field[0, 0], field[0, 1], field[0, 2]};
            yield return new List<Cell> {field[1, 0], field[1, 1], field[1, 2]};
            yield return new List<Cell> {field[2, 0], field[2, 1], field[2, 2]};
                
            yield return new List<Cell> {field[0, 0], field[1, 0], field[2, 0]};
            yield return new List<Cell> {field[0, 1], field[1, 1], field[2, 1]};
            yield return new List<Cell> {field[0, 2], field[1, 2], field[2, 2]};
                
            yield return new List<Cell> {field[0, 0], field[1, 1], field[2, 2]};
            yield return new List<Cell> {field[0, 2], field[1, 1], field[2, 0]};
        }*/
    }

    protected virtual void EndGame()
    {
        //_view.FinishGame();
        IsEnded = true;
        int firstPlayerCounter = 0;
        int secondPlayerCounter = 0;
        foreach (var cell in field)
        {
            if (cell.MarkedByPlayer.Name == firstPlayer.Name)
            {
                firstPlayerCounter++;
            }
            if (cell.MarkedByPlayer.Name == secondPlayer.Name)
            {
                secondPlayerCounter++;
            }
        }

        if (firstPlayerCounter>secondPlayerCounter)
        {
            Winner = firstPlayer;
        } else if (secondPlayerCounter > firstPlayerCounter)
        {
            Winner = secondPlayer;
        }
    }

    private void SwitchPlayer()
    {
        CurrentPlayer = CurrentPlayer == firstPlayer ? secondPlayer : firstPlayer;
    }
    
    
    public void MakeBotMove()
    {

        List<(int, int)> validMoves = GetValidMoves();

        if (validMoves.Count == 0)
        {
            Console.WriteLine("No valid moves for the bot player.");
            SwitchPlayer();
            return;
        }

        Random random = new Random();
        int randomIndex = random.Next(validMoves.Count);
        (int x, int y) = validMoves[randomIndex];

        MakeMove(x, y);
    }

    private List<(int, int)> GetValidMoves()
    {
        List<(int, int)> validMoves = new List<(int, int)>();

        for (int x = 0; x < FieldSize; x++)
        {
            for (int y = 0; y < FieldSize; y++)
            {
                if (MoveValid(x, y))
                {
                    validMoves.Add((x, y));
                }
            }
        }

        return validMoves;
    }
    
}