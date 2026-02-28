using UnityEngine;
using UnityEngine.InputSystem;
using System;

public class InputManager : MonoBehaviour
{
    public static InputManager Instance { get; private set; }

    public InputAction touchAction;
    public InputAction touchPositionAction;

    public event Action<Vector2Int> OnGridTouch;
    public event Action OnCancelButton;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        touchAction.Enable();
        touchPositionAction.Enable();
    }

    void Update()
    {
        if (touchAction.WasPressedThisFrame())
        {
            Vector2 pos = touchPositionAction.ReadValue<Vector2>();
            Vector3 worldPos = Camera.main.ScreenToWorldPoint(pos);
            Vector2Int gridPos = GridManager.Instance.WorldToGrid(worldPos);
            if (gridPos != new Vector2Int(-1, -1))
            {
                OnGridTouch?.Invoke(gridPos);
            }
        }
    }
    public void CancelButton()
    {
        OnCancelButton?.Invoke();
    }
}