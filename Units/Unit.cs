using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
public class Unit : MonoBehaviour
{
    [Header("Changable")]
    public int maxHP = 10;
    public int currentHP = 10;
    public int speed = 0;
    public int attackPower = 1;
    public Team team = Team.Player;
    public Vector2Int gridPosition = new(0, 0);
    public Vector2Int[] attackPos = { new(0, 1), new(0, -1), new(1, 0), new(-1, 0) };
    public Vector2Int[] movePos = { new(0, 1), new(0, -1), new(1, 0), new(-1, 0) };
    [Header("Unchangable")]
    public Phase currentPhase = Phase.Done;
    public bool isMyTurn = false;
    private IUnitController controller;
    void Awake()
    {
        controller = GetComponent<IUnitController>();
    }
    void Start()
    {
        transform.position = GridManager.Instance.GridToWorld(gridPosition);
        BattleManager.Instance.RegisterUnit(this);
    }

    public void StartTurn()
    {
        //if (team == Team.Enemy) EnemyAction();
        controller?.TakeTurn();
    }
    public void EndTurn()
    {
        isMyTurn = false;
        BattleManager.Instance.NextTurn();
    }
    public void Move(Vector2Int gridPos)
    {
        if (GridManager.Instance.MoveUnit(this, gridPos))
        {
            gridPosition = gridPos;
            transform.position = GridManager.Instance.GridToWorld(gridPosition);
        }
    }
    public void Attack(Unit unit)
    {
        unit.TakeDamage(attackPower);
    }
    public void TakeDamage(int damage)
    {
        currentHP -= damage;
        if (currentHP < 0) currentHP = 0;
        //UIManager.Instance.UpdateHP(this);
        if (currentHP == 0)
        {
            Die();
        }
    }
    void Die()
    {
        BattleManager.Instance.UnregisterUnit(this);
        Destroy(gameObject);
    }
    public void ChangePhase(Phase phase)
    {
        currentPhase = phase;
        GridManager.Instance.ClearGridColor();
        if (phase == Phase.Move)
        {
            GridManager.Instance.SetGridColor(GetMovePositions(), GridColor.Move);
        }
        if (phase == Phase.Attack)
        {
            GridManager.Instance.SetGridColor(GetAttackPositions(), GridColor.Attack);
        }
    }
    public List<Vector2Int> GetMovePositions()
    {
        List<Vector2Int> positions = new();
        for (int i = 0; i < movePos.Length; i++)
        {
            Vector2Int position = movePos[i] + gridPosition;
            if (GridManager.Instance.CanMove(position)) positions.Add(position);
        }
        return positions;
    }
    public List<Vector2Int> GetAttackPositions()
    {
        List<Vector2Int> positions = new();
        for (int i = 0; i < attackPos.Length; i++)
        {
            Vector2Int position = attackPos[i] + gridPosition;
            if (GridManager.Instance.IsInsideGrid(position)) positions.Add(position);
        }
        return positions;
    }
}
