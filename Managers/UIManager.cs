using UnityEngine;
using TMPro;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }
    public TextMeshProUGUI hpText;
    public TextMeshProUGUI turnText;
    public TextMeshProUGUI phaseText;
    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }
    public void UpdateHP(Unit unit)
    {
        hpText.text = "HP: " + unit.currentHP;
    }
    public void UpdateTurn(Unit unit)
    {
        turnText.text = unit.name + " ÇÃÉ^Å[Éì";
    }
    public void UpdatePhase(Phase phase)
    {
        phaseText.text = "Phase: " + phase.ToString();
    }
}