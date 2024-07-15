using System;
using TMPro;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.Tilemaps;

public class CrossedBorders
{
    public bool top = false;
    public bool bottom = false;
    public bool left = false;
    public bool right = false;
}

public class Board : MonoBehaviour
{
    public Tilemap tilemap { get; private set; }
    public Figure activeFigure { get; private set; }
    public Tetromino nextTetromino { get; private set; }
    public int score {  get; private set; }

    public TextMeshProUGUI textScore;
    public float defaultStepDelay = 1f;
    public Vector3Int spawnPosition = new Vector3Int(-1, 10, 0);
    public Vector3Int nextTetrominoPosition = new Vector3Int(12, 7, 0);

    public Vector2Int boardSize = new Vector2Int(10, 20);
    public RectInt Bounds
    {
        get
        {
            Vector2Int position =
                new Vector2Int(-boardSize.x / 2, -boardSize.y / 2);
            return new RectInt(position, boardSize);
        }
    }

    private void Awake()
    {
        tilemap = GetComponentInChildren<Tilemap>();
        activeFigure = GetComponentInChildren<Figure>();
        nextTetromino = GetRandomTetromino();
        score = 0;
    }

    private void Start()
    {
        SpawnFigure();
        ShowScore();
    }

    private Tetromino GetRandomTetromino()
    {
        Array tetrominoValues = Enum.GetValues(typeof(Tetromino));
        return (Tetromino)tetrominoValues.GetValue(
            UnityEngine.Random.Range(0, tetrominoValues.Length));
    }

    public void SpawnFigure()
    {
        activeFigure.Initialize(this, nextTetromino, spawnPosition);
        Set(activeFigure);
        GenNextTetromino();
    }

    public void GenNextTetromino()
    {
        for (int i = 0; i < Data.Cells[nextTetromino].Length; i++)
        {
            Vector3Int tilePosition =
                (Vector3Int)Data.Cells[nextTetromino][i] +
                nextTetrominoPosition;
            tilemap.SetTile(tilePosition, null);
        }
        nextTetromino = GetRandomTetromino();
        for (int i = 0; i < Data.Cells[nextTetromino].Length; i++)
        {
            Vector3Int tilePosition =
                (Vector3Int)Data.Cells[nextTetromino][i] +
                nextTetrominoPosition;
            tilemap.SetTile(tilePosition, activeFigure.tile);
        }
    }

    public void UpdateScore(int clearedRows)
    {
        score += clearedRows;
        ShowScore();
    }

    public void ShowScore()
    {
        textScore.text = $"Score: {score}";
    }

    public void GameOver()
    {
        print("GAME OVER");
        tilemap.ClearAllTiles();
        GenNextTetromino();
        SpawnFigure();
        activeFigure.stepDelay = defaultStepDelay;
        score = 0;
        ShowScore();
    }

    public void Set(Figure figure)
    {
        RectInt bounds = Bounds;
        for (int i = 0; i < figure.cells.Length; i++)
        {
            Vector3Int tilePosition = figure.cells[i] + figure.position;
            if (bounds.Contains((Vector2Int)tilePosition))
                tilemap.SetTile(tilePosition, figure.tile);
        }
    }

    public void Clear(Figure figure)
    {
        RectInt bounds = Bounds;
        for (int i = 0; i < figure.cells.Length; i++)
        {
            Vector3Int tilePosition = figure.cells[i] + figure.position;
            if (bounds.Contains((Vector2Int)tilePosition))
                tilemap.SetTile(tilePosition, null);
        }
    }

    public CrossedBorders GetCrossedBorders(Figure figure,
                                            Vector3Int position)
    {
        CrossedBorders crossedBorders = new CrossedBorders();
        RectInt bounds = Bounds;

        for (int i = 0; i < figure.cells.Length; i++)
        {
            Vector3Int tilePosition = figure.cells[i] + position;
            crossedBorders.top    |= tilePosition.y >= bounds.yMax;
            crossedBorders.bottom |= tilePosition.y <  bounds.yMin;
            crossedBorders.left   |= tilePosition.x <  bounds.xMin;
            crossedBorders.right  |= tilePosition.x >= bounds.xMax;
        }

        return crossedBorders;
    }

    public bool IsOverlay(Figure figure, Vector3Int position)
    {
        for (int i = 0; i < figure.cells.Length; i++)
        {
            Vector3Int tilePosition = figure.cells[i] + position;
            if (tilemap.HasTile(tilePosition))
                return true;
        }
        return false;
    }

    public bool IsValidPosition(Figure figure, Vector3Int position)
    {
        CrossedBorders crossedBorders = GetCrossedBorders(figure, position);
        return !(crossedBorders.left || crossedBorders.right ||
            crossedBorders.bottom || IsOverlay(figure, position));
    }

    public int ClearRows()
    {
        RectInt bounds = Bounds;
        int row, col, count;
        count = 0;
        for (row = bounds.yMin; row < bounds.yMax; row++)
        {
            bool rowIsFull = true;
            for (col = bounds.xMin; col < bounds.xMax; col++)
            {
                Vector3Int position = new Vector3Int(col, row, 0);
                TileBase tile = tilemap.GetTile(position);
                if (tile == null)
                    rowIsFull = false;
                if (count > 0)
                {
                    Vector3Int belowPosition =
                        new Vector3Int(col, row - count, 0);
                    tilemap.SetTile(belowPosition, tile);
                    tilemap.SetTile(position, null);
                }
                else if (!rowIsFull)
                {
                    break;
                }
            }
            if (rowIsFull)
                count++;
        }
        return count;
    }
}
