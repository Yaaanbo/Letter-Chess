using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardTiles : MonoBehaviour
{
    [SerializeField] private Color baseColor;
    [SerializeField] private Color offsetColor;
    [SerializeField] private SpriteRenderer spriteRenderer;

    [SerializeField] private GameObject highlight;
    [SerializeField] private float pawnDetectorRadius = .5f;
    [SerializeField] private LayerMask pawnMask;

    private bool isOccupied = false;

    private void Update()
    {
        isOccupied = Physics2D.OverlapCircle(this.transform.position, pawnDetectorRadius, pawnMask);
    }

    public void CheckOffsetTile(bool _isOffset)
    {
        if (_isOffset)
            spriteRenderer.color = offsetColor;
        else
            spriteRenderer.color = baseColor;
    }

    private void OnMouseEnter()
    {
        highlight.SetActive(true);
    }

    private void OnMouseExit()
    {
        highlight.SetActive(false);
    }

    private void OnMouseDown()
    {
        if (!isOccupied)
        {
            GameManager.singleton.SpawnPawnOnBoard(this.transform.position);
        }
    }
}
