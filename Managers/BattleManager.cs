using UnityEngine;
using System.Collections.Generic;
using System.Xml.XPath;
using System.Linq;
public class BattleManager : MonoBehaviour
{
    public static BattleManager Instance { get; private set; }
    public List<Unit> turnOrder = new();
    public int currentTurnIndex = 0;
    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }
    void Start()
    {
        SortTurnOrder();
        StartTurn();
    }
    public void RegisterUnit(Unit unit)
    {
        GridManager.Instance.RegisterUnit(unit);
        turnOrder.Add(unit);
    }
    public void UnregisterUnit(Unit unit)
    {
        GridManager.Instance.UnRegisterUnit(unit);
        int index = turnOrder.IndexOf(unit);
        if (index == -1) return;
        turnOrder.RemoveAt(index);
        if (index <= currentTurnIndex)
        {
            currentTurnIndex--;
        }
    }
    void StartTurn()
    {
        if (turnOrder.Count == 0) return;
        Unit currentUnit = turnOrder[currentTurnIndex];
        currentUnit.StartTurn();
    }
    void SortTurnOrder()
    {
        turnOrder = turnOrder.OrderBy(unit => unit.speed).ToList();
    }
    public void NextTurn()
    {
        currentTurnIndex++;
        if (currentTurnIndex >= turnOrder.Count) currentTurnIndex = 0;
        if (currentTurnIndex == 0) SortTurnOrder();
        StartTurn();
    }
}
