using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Bishop : Piece
{
    public override List<Vector3Int> GetAvailableMoves(Tile[,] tiles)
    {
        if (isMounted)
            return KnightStyleMoves(tiles);

        List<Vector3Int> moves = new List<Vector3Int>();
        Vector2Int[] directions =
        {
            new Vector2Int(1, 1),
            new Vector2Int(1, -1),
            new Vector2Int(-1, 1),
            new Vector2Int(-1, -1)
        };
        foreach (var dir in directions)
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
                        moves.Add(new Vector3Int(x, y, 0));

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
        //var moves = GetAvailableMoves(tiles);

        //return moves.Where(pos =>
        //{
        //    var target = tiles[pos.x, pos.y].occupyingPiece;
        //    return target != null && target.color != color;
        //}).ToList();
        List<Vector3Int> attackTiles = new List<Vector3Int>();
        Vector2Int[] directions =
        {
        new Vector2Int(1, 1),
        new Vector2Int(1, -1),
        new Vector2Int(-1, 1),
        new Vector2Int(-1, -1)
    };

        foreach (var dir in directions)
        {
            int x = boardPosition.x + dir.x;
            int y = boardPosition.y + dir.y;

            while (x >= 0 && x < 8 && y >= 0 && y < 8)
            {
                var target = tiles[x, y].occupyingPiece;
                attackTiles.Add(new Vector3Int(x, y, 0));

                if (target != null)
                    break;

                x += dir.x;
                y += dir.y;
            }
        }

        return attackTiles;

    }
    private bool IsInBounds(int x, int y) => x >= 0 && x < 8 && y >= 0 && y < 8;
}
