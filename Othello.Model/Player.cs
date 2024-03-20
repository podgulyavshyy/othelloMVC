namespace Othello.Model;


public class Player
{
    public string Name { get; }
    
    public bool isBot { get; set; } 

    public Player(string name)
    {
        Name = name;
    }
}