using UnityEngine;

public class GridCell : MonoBehaviour
{
    public Vector2Int gridPos;
    SpriteRenderer sr;
    void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
    }
    public void SetColor(Color color)
    {
        sr.color = color;
    }
}
