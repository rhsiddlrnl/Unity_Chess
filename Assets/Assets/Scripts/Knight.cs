using System.Collections.Generic;
using UnityEngine;

public class Knight : Piece
{
    public override List<Vector3Int> GetAvailableMoves(Tile[,] tiles)
    {
        List<Vector3Int> moves = new List<Vector3Int>();

        Vector2Int[] jumpOffsets =
        {
            new Vector2Int(-2, -1), new Vector2Int(-2, 1),
            new Vector2Int(-1, -2), new Vector2Int(-1, 2),
            new Vector2Int(1, -2), new Vector2Int (1, 2),
            new Vector2Int(2, -1), new Vector2Int(2, 1)
        };

        foreach(var offset in jumpOffsets)
        {
            int x = boardPosition.x + offset.x;
            int y = boardPosition.y + offset.y;

            if(!IsInBounds(x, y)) continue;

            //if(IsInBounds(x, y))
            //{
            //    var targetTile = tiles[x, y];
            //    if(targetTile.HasNoPiece()||targetTile.occupyingPiece.color!= color)
            //    {
            //        moves.Add(new Vector3Int(x, y, 0));
            //    }
            //}

            var target = tiles[x, y].occupyingPiece;
            //if(target == null || (target.color == this.color && !target.isMounted))
            //{
            //    moves.Add(new Vector3Int(x, y, 0));
            //}

            //if (target == null || target.color != this.color ||
            //(target.color == this.color && !target.isMounted))
            //{
            //    moves.Add(new Vector3Int(x, y, 0));
            //}
            
            //without this, move like normal chess
            if (target != null && target.color != this.color)
                continue;

            if (target != null && target.color == this.color)
            {
                if (!target.isMounted && target.type != PieceType.Knight)
                {
                    moves.Add(new Vector3Int(x, y, 0));
                }
                continue;
            }

            moves.Add(new Vector3Int(x, y, 0));

        }
        return moves;
    }
    private bool IsInBounds(int x, int y) => x >= 0 && x < 8 && y >= 0 && y < 8;
}
