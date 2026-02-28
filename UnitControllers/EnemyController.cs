using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour, IUnitController
{
    Unit unit;
    public void TakeTurn()
    {
        unit.isMyTurn = true;
        StartCoroutine(EnemyRoutine());
    }
    void Awake()
    {
        unit = GetComponent<Unit>();
    }
    IEnumerator EnemyRoutine()
    {
        unit.ChangePhase(Phase.Move);
        yield return new WaitForSeconds(1f);
        unit.Move(unit.gridPosition + Vector2Int.left);
        unit.ChangePhase(Phase.Attack);
        yield return new WaitForSeconds(1f);
        //Attack();
        unit.ChangePhase(Phase.Done);
        yield return new WaitForSeconds(1);
        unit.EndTurn();
    }
}
