using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridTile : MonoBehaviour
{
    [SerializeField] private SpriteRenderer _spriteRenderer;

    private void Start()
    {
        if (_spriteRenderer == null)
        {
            _spriteRenderer = GetComponent<SpriteRenderer>();
        }
    }

    public void UpdateColor(Color color)
    {
        _spriteRenderer.color = color;
        _spriteRenderer.size = new Vector2(2, 2);
    }
}
