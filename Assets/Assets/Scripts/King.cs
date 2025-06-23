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

            if (isMounted)
            {
                if (target == null)
                    moves.Add(new Vector3Int(x, y, 0));
            }
            else
            {
                if (target == null || target.color != this.color)
                    moves.Add(new Vector3Int(x, y, 0));
            }

            //if (target != null)
            //{
            //    Debug.Log($"[킹 이동 체크] 타겟 있음: {target.type}, 색: {target.color}, 내 색: {color}");
            //}


            //if (target == null || target.color != color)
            //{
            //    moves.Add(new Vector3Int(x, y, 0));
            //}
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

            attacks.Add(new Vector3Int(x, y, 0));

            //var target = tiles[x, y].occupyingPiece;
            //if (target != null)
            //{
            //    attacks.Add(new Vector3Int(x, y, 0));
            //}
        }

        return attacks;
    }

    //public void OnSuccessfulAttack(Piece target)
    //{
    //    Debug.Log($"[킹 특수공격 연출] {target.type} 제거 후 연출 실행 가능");
    //}
    private bool IsInBounds(int x, int y) => x >= 0 && x < 8 && y >= 0 && y < 8;

    protected override void Die()
    {
        base.Die();

        if (type == PieceType.King)
        {
            Debug.Log($"[게임 종료] {color} 왕 사망");
            FindAnyObjectByType<BoardManager>().EndGame(color == PieceColor.White ? PieceColor.Black : PieceColor.White);
        }
    }
}
