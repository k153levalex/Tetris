using UnityEngine;
using UnityEngine.Tilemaps;

public class Shadow : MonoBehaviour
{
    public Tile tile;
    public Board mainBoard;
    public Figure trackingFigure { get; private set; }
    public Tilemap tilemap { get; private set; }
    public Vector3Int[] cells { get; private set; }
    public Vector3Int position { get; private set; }

    private void Awake()
    {
        tilemap = GetComponentInChildren<Tilemap>();
        trackingFigure = mainBoard.GetComponentInChildren<Figure>();
        cells = new Vector3Int[4];
    }

    private void LateUpdate()
    {
        Clear();
        Copy();
        Drop();
        Set();
    }

    private void Clear()
    {
        RectInt bounds = mainBoard.Bounds;
        for (int i = 0; i < cells.Length; i++)
        {
            Vector3Int tilePosition = cells[i] + position;
            if (bounds.Contains((Vector2Int)tilePosition))
                tilemap.SetTile(tilePosition, null);
        }
    }

    private void Copy()
    {
        for (int i = 0; i < cells.Length; i++)
            cells[i] = trackingFigure.cells[i];
    }

    private void Drop()
    {
        Vector3Int position = trackingFigure.position;

        int current = position.y;
        int bottom = -mainBoard.boardSize.y / 2 - 1;

        mainBoard.Clear(trackingFigure);

        for (int row = current; row >= bottom; row--)
        {
            position.y = row;

            if (mainBoard.IsValidPosition(trackingFigure, position))
                this.position = position;
            else
                break;
        }

        mainBoard.Set(trackingFigure);
    }

    private void Set()
    {
        RectInt bounds = mainBoard.Bounds;
        for (int i = 0; i < cells.Length; i++)
        {
            Vector3Int tilePosition = cells[i] + position;
            if (bounds.Contains((Vector2Int)tilePosition))
                tilemap.SetTile(tilePosition, tile);
        }
    }

}
