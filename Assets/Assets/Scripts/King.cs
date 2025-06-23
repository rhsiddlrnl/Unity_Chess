using System.Collections.Generic;
using UnityEngine;

public class King : Piece
{
    public override List<Vector3Int> GetAvailableMoves(Tile[,] tiles)
    {
        if (isMounted)
            return KnightStyleMoves(tiles);

        List<Vector3Int> moves = new List<Vector3Int>();

        Vector2Int[] jumpOffsets =
        {
            new Vector2Int(-1, -1), new Vector2Int(-1, 1),
            new Vector2Int(1, -1), new Vector2Int(1, 1),
            new Vector2Int(1, 0), new Vector2Int (-1, 0),
            new Vector2Int(0, -1), new Vector2Int(0, 1)
        };

        foreach (var offset in jumpOffsets)
        {
            int x = boardPosition.x + offset.x;
            int y = boardPosition.y + offset.y;

            if(!IsInBounds(x, y))
                continue;

            var target = tiles[x, y].occupyingPiece;

            if (target != null)
            {
                Debug.Log($"[ŷ �̵� üũ] Ÿ�� ����: {target.type}, ��: {target.color}, �� ��: {color}");
            }


            if (target == null || target.color != color)
            {
                moves.Add(new Vector3Int(x, y, 0));
            }
        }
        return moves;
    }

    public override List<Vector3Int> GetAttackableTiles(Tile[,] tiles)
    {
        List<Vector3Int> attacks = new List<Vector3Int>();
        Vector2Int[] directions =
        {
            new Vector2Int(-1, -1), new Vector2Int(0, -1), new Vector2Int(1, -1),
            new Vector2Int(-1,  0),                        new Vector2Int(1,  0),
            new Vector2Int(-1,  1), new Vector2Int(0,  1), new Vector2Int(1,  1)
        };

        foreach (var dir in directions)
        {
            int x = boardPosition.x + dir.x;
            int y = boardPosition.y + dir.y;

            if (!IsInBounds(x, y)) continue;

            var target = tiles[x, y].occupyingPiece;
            if (target != null && target.color != color)
            {
                attacks.Add(new Vector3Int(x, y, 0));
            }
        }

        return attacks;
    }

    //public void OnSuccessfulAttack(Piece target)
    //{
    //    Debug.Log($"[ŷ Ư������ ����] {target.type} ���� �� ���� ���� ����");
    //}
    private bool IsInBounds(int x, int y) => x >= 0 && x < 8 && y >= 0 && y < 8;
}
