using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardTiles : MonoBehaviour
{
    [SerializeField] private Color baseColor;
    [SerializeField] private Color offsetColor;
    [SerializeField] private SpriteRenderer spriteRenderer;

    [SerializeField] private GameObject highlight;
    
    public void CheckOffsetTile(bool isOffset)
    {
        if (isOffset)
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
        GameManager.singleton.SpawnPawnsOnBoard(this.transform.position);
    }
}
