using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour, IUnitController
{
    Unit unit;
    public void TakeTurn()
    {
        unit.isMyTurn = true;
        unit.ChangePhase(Phase.Move);
    }
    void Awake()
    {
        unit = GetComponent<Unit>();
    }
    void Start()
    {
        unit.OnGridTouch += HandleGridTouch;
        unit.OnCancelButton += HandleCancelButton;
    }
    void HandleGridTouch(Vector2Int gridPos)
    {
        if (unit == null || !unit.isMyTurn) return;
        if (unit.currentPhase == Phase.Move)
        {
            List<Vector2Int> movePositions = unit.GetMovePositions();
            if (movePositions.Contains(gridPos))
            {
                unit.Move(gridPos);
                unit.ChangePhase(Phase.Attack);
            }
        }
        else if (unit.currentPhase == Phase.Attack)
        {
            List<Vector2Int> attackPositions = unit.GetAttackPositions();
            if (attackPositions.Contains(gridPos))
            {
                Unit targetUnit = GridManager.Instance.GetUnit(gridPos);
                if (targetUnit != null && targetUnit.team != unit.team)
                {
                    unit.Attack(targetUnit);
                    unit.ChangePhase(Phase.Done);
                    unit.EndTurn();
                }
            }
        }
    }
    void HandleCancelButton()
    {
        if (unit == null || !unit.isMyTurn) return;
        if (unit.currentPhase == Phase.Move)
        {
            unit.ChangePhase(Phase.Attack);
        }
        else if (unit.currentPhase == Phase.Attack)
        {
            unit.ChangePhase(Phase.Done);
            unit.EndTurn();
        }
    }

}
