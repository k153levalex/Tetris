using System.Collections.Generic;
using UnityEngine;

public enum Tetromino
{
    I, O, T, J, L, S, Z
}

public static class Data
{
    public static readonly float cos = Mathf.Cos(Mathf.PI / 2f);
    public static readonly float sin = Mathf.Sin(Mathf.PI / 2f);
    public static readonly float[] RotationMatrix =
        new float[] { cos, sin, -sin, cos };

    public static readonly Dictionary<Tetromino, Vector2Int[]> Cells =
        new Dictionary<Tetromino, Vector2Int[]>() {
            {
                Tetromino.I,
                new Vector2Int[] {
                    new Vector2Int(-1, 0),
                    new Vector2Int( 0, 0),
                    new Vector2Int( 1, 0),
                    new Vector2Int( 2, 0)
                }
            },
            {
                Tetromino.O,
                new Vector2Int[] {
                    new Vector2Int( 0, 1),
                    new Vector2Int( 1, 1),
                    new Vector2Int( 0, 0),
                    new Vector2Int( 1, 0)
                }
            },
            {
                Tetromino.T,
                new Vector2Int[] {
                    new Vector2Int( 0, 1),
                    new Vector2Int(-1, 0),
                    new Vector2Int( 0, 0),
                    new Vector2Int( 1, 0)
                }
            },
            {
                Tetromino.J,
                new Vector2Int[] {
                    new Vector2Int(-1, 1),
                    new Vector2Int(-1, 0),
                    new Vector2Int( 0, 0),
                    new Vector2Int( 1, 0)
                }
            },
            {
                Tetromino.L,
                new Vector2Int[] {
                    new Vector2Int( 1, 1),
                    new Vector2Int(-1, 0),
                    new Vector2Int( 0, 0),
                    new Vector2Int( 1, 0)
                }
            },
            {
                Tetromino.S,
                new Vector2Int[] {
                    new Vector2Int( 0, 1),
                    new Vector2Int( 1, 1),
                    new Vector2Int(-1, 0),
                    new Vector2Int( 0, 0)
                }
            },
            {
                Tetromino.Z,
                new Vector2Int[] {
                    new Vector2Int(-1, 1),
                    new Vector2Int( 0, 1),
                    new Vector2Int( 0, 0),
                    new Vector2Int( 1, 0)
                }
            },
        };
}
