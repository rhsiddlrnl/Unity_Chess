using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class Rook : Piece
{
    public override List<Vector3Int> GetAvailableMoves(Tile[,] tiles)
    {
        if (isMounted)
            return KnightStyleMoves(tiles);

        List<Vector3Int> moves = new List<Vector3Int>();
        Vector2Int[] directions =
        {
            new Vector2Int(0, 1),
            new Vector2Int(1, 0),
            new Vector2Int(0, -1),
            new Vector2Int(-1, 0)
        };

        foreach(var dir in directions)
        {
            int x = boardPosition.x + dir.x;
            int y = boardPosition.y + dir.y;

            while (IsInBounds(x, y))
            {
                if (tiles[x, y].HasNoPiece())
                {
                    moves.Add(new Vector3Int(x, y, 0));
                }
                else
                {
                    if (tiles[x, y].occupyingPiece.color != color)
                        //    moves.Add(new Vector3Int(x, y, 0));

                        //break;
                        break;
                    
                }

                x += dir.x;
                y += dir.y;
            }
        }
        return moves;
    }

    public override List<Vector3Int> GetAttackableTiles(Tile[,] tiles)
    {
        List<Vector3Int> attacks = new List<Vector3Int>();
        Vector2Int[] directions =
        {
            new Vector2Int(0, 1),
            new Vector2Int(0, -1),
            new Vector2Int(1, 0),
            new Vector2Int(-1, 0),
            new Vector2Int(-1, 1),
            new Vector2Int(-1, -1),
            new Vector2Int(1, 1),
            new Vector2Int(1, -1),
        };

        foreach (var dir in directions)
        {
            int x = boardPosition.x + dir.x;
            int y = boardPosition.y + dir.y;

            if(IsInBounds(x, y))
            {
                attacks.Add(new Vector3Int(x, y, 0));
            }
        }
        return attacks;
    }
    private bool IsInBounds(int x, int y) => x >= 0 && x < 8 && y>=0 && y < 8;
}
