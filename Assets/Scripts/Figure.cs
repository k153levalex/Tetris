using UnityEngine;
using UnityEngine.Tilemaps;

public class Figure : MonoBehaviour
{
    public Tile tile;
    public Board board { get; private set; }
    public Tetromino tetromino { get; private set; }
    public Vector3Int position { get; private set; }
    public Vector3Int[] cells { get; private set; }

    public float stepDelay = 1f;
    public float accelerationPercentage = 2f;
    public float moveDelay = 0.1f;

    private float stepTime;
    private float moveTime;

    public void Initialize(Board board, Tetromino tetromino,
                           Vector3Int position)
    {
        this.board = board;
        this.tetromino = tetromino;
        this.position = position;

        stepTime = Time.time + stepDelay;
        moveTime = Time.time + moveDelay;

        if (cells == null)
            cells = new Vector3Int[Data.Cells[tetromino].Length];

        for (int i = 0; i < cells.Length; i++)
            cells[i] = (Vector3Int)Data.Cells[tetromino][i];
    }

    private void Update()
    {
        board.Clear(this);

        if (Input.GetKeyDown(KeyCode.L))
            LeftRotate();
        if (Input.GetKeyDown(KeyCode.Space))
            HardDrop();
        HandleMoveInputs();
        if (Time.time > stepTime)
            Step();

        board.Set(this);
    }

    private void HandleMoveInputs()
    {
        if (Input.GetKeyDown(KeyCode.S))
        {
            Step();
            moveTime += moveDelay;
        }
        if (Input.GetKeyDown(KeyCode.A))
        {
            Move(Vector2Int.left);
            moveTime += moveDelay;
        }
        else if (Input.GetKeyDown(KeyCode.D))
        {
            Move(Vector2Int.right);
            moveTime += moveDelay;
        }

        if (Time.time > moveTime)
        {
            if (Input.GetKey(KeyCode.S))
                Step();
            if (Input.GetKey(KeyCode.A))
                Move(Vector2Int.left);
            else if (Input.GetKey(KeyCode.D))
                Move(Vector2Int.right);
        }
    }

    private void Step()
    {
        stepTime = Time.time + stepDelay;
        if (!Move(Vector2Int.down))
            Lock();
    }

    private void HardDrop()
    {
        while (Move(Vector2Int.down))
            ;
        Lock();
    }

    public void AccelerateStep()
    {
        stepDelay *= (100 - accelerationPercentage) / 100;
    }

    private void Lock()
    {
        board.Set(this);
        if (board.GetCrossedBorders(this, position).top)
        {
            board.GameOver();
            return;
        }
        int clearedRows = board.ClearRows();
        board.UpdateScore(clearedRows);
        if (clearedRows > 0)
            AccelerateStep();
        board.SpawnFigure();
    }

    private bool Move(Vector2Int translation)
    {
        Vector3Int newPosition = position;
        newPosition.x += translation.x;
        newPosition.y += translation.y;

        bool valid = board.IsValidPosition(this, newPosition);

        if (valid)
        {
            position = newPosition;
            moveTime = Time.time + moveDelay;
        }

        return valid;
    }

    private void LeftRotate()
    {
        ApplyRotationMatrix(-1);
        if (!board.IsValidPosition(this, position))
            ApplyRotationMatrix(1);
    }

    private void ApplyRotationMatrix(int direction)
    {
        float[] matrix = Data.RotationMatrix;

        for (int i = 0; i < cells.Length; i++)
        {
            Vector3 cell = cells[i];

            int x, y;

            switch (tetromino)
            {
                case Tetromino.I:
                case Tetromino.O:
                    cell.x -= 0.5f;
                    cell.y -= 0.5f;
                    x = Mathf.CeilToInt((cell.x * matrix[0] * direction) +
                        (cell.y * matrix[1] * direction));
                    y = Mathf.CeilToInt((cell.x * matrix[2] * direction) +
                        (cell.y * matrix[3] * direction));
                    break;

                default:
                    x = Mathf.RoundToInt((cell.x * matrix[0] * direction) +
                        (cell.y * matrix[1] * direction));
                    y = Mathf.RoundToInt((cell.x * matrix[2] * direction) +
                        (cell.y * matrix[3] * direction));
                    break;
            }

            cells[i] = new Vector3Int(x, y, 0);
        }
    }

}
