using UnityEngine;

public enum Team
{
    Player,
    Enemy
}
public enum Phase
{
    Move,
    Attack,
    Done
}
public class GridData
{
    public Unit unit = null;
}
static public class GridColor
{
    public static readonly Color Default = new(1, 1, 1, 0.5f);
    public static readonly Color Move = new(0, 1, 0, 0.5f);
    public static readonly Color Attack = new(1, 0, 0, 0.5f);
}
public interface IUnitController
{
    void TakeTurn();
}