using DefaultNamespace;

[System.Serializable]
public class Player
{
    public TeamColor color;

    public Player(TeamColor color)
    {
        this.color = color;
    }
}
