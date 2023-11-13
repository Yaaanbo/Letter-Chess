using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardManager : MonoBehaviour
{
    public static BoardManager instace { get; set; }

    [Header("Board Info")]
    [SerializeField] private int width;
    public int GetWidth { get { return width; } }

    [SerializeField] private int height;
    public int GetHeight { get { return height; } }

    [SerializeField] private BoardTiles tilesPrefab;
    [SerializeField] private Transform tilesParent;

    [Header("Cam position")]
    [SerializeField] private Transform cam;
    [SerializeField] private Vector3 cameraOffset;


    private void Awake()
    {
        if (instace == null)
            instace = this;
        else
            Destroy(this);
    }

    // Start is called before the first frame update
    void Start()
    {
        GenerateBoard();
    }

    private void GenerateBoard()
    {
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                Vector3 tilePos = new Vector3(x, y);
                var tile = Instantiate(tilesPrefab, tilePos, Quaternion.identity);

                tile.transform.SetParent(tilesParent);
                tile.name = $"Tile {x} {y}";

                bool isOffset = (x % 2 == 0 && y % 2 != 0) || (x % 2 != 0 && y % 2 == 0);

                tile.CheckOffsetTile(isOffset);
            }
        }

        Vector3 camPos = new Vector3(cameraOffset.x, cameraOffset.y, -10);
        cam.position = camPos;
    }
}
