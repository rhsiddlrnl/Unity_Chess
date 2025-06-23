using System.Collections.Generic;
using UnityEngine;

public class Pawn : Piece
{
    public override List<Vector3Int> GetAvailableMoves(Tile[,] tiles)
    {
        List<Vector3Int> moves = new List<Vector3Int>();
        int dir = (color == PieceColor.White) ? 1 : -1;
        Vector3Int forward = new Vector3Int(boardPosition.x, boardPosition.y + dir, 0);

        if (IsInBounds(forward) && tiles[forward.x, forward.y].HasNoPiece())
        {
            moves.Add(forward);

            // 첫 위치에 있는 경우 → 2칸 전진 가능
            bool isAtStart = (color == PieceColor.White && boardPosition.y == 1) ||
                             (color == PieceColor.Black && boardPosition.y == 6);

            if (isAtStart)
            {
                Vector3Int twoForward = new Vector3Int(boardPosition.x, boardPosition.y + dir * 2, 0);
                if (IsInBounds(twoForward) && tiles[twoForward.x, twoForward.y].HasNoPiece())
                {
                    moves.Add(twoForward);
                }
            }
        }

        return moves;
    }

    public override List<Vector3Int> GetAttackableTiles(Tile[,] tiles)
    {
        List<Vector3Int> attacks = new List<Vector3Int>();
        int dir = (color == PieceColor.White) ? 1 : -1;

        Vector3Int forward = new Vector3Int(boardPosition.x, boardPosition.y + dir, 0);
        if (IsInBounds(forward))
            attacks.Add(forward);

        Vector3Int diagLeft = new Vector3Int(boardPosition.x - 1, boardPosition.y + dir, 0);
        Vector3Int diagRight = new Vector3Int(boardPosition.x + 1, boardPosition.y + dir, 0);

        if (IsInBounds(diagLeft))
            attacks.Add(diagLeft);
        if (IsInBounds(diagRight))
            attacks.Add(diagRight);

        return attacks;
    }


    private bool IsInBounds(Vector3Int pos)
    {
        return pos.x >= 0 && pos.x < 8 && pos.y >= 0 && pos.y < 8;
    }
}
