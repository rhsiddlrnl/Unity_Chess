using System.Collections.Generic;
using UnityEngine;

public class Queen : Piece
{
    public enum QueenMode
    {
        Heal,
        Attack
    }

    public QueenMode currentMode = QueenMode.Heal;

    public Sprite healSprite;
    public Sprite attackSprite;
    public Sprite mountedHealSprite;
    public Sprite mountedAttackSprite;

    public void ToggleMode()
    {
        currentMode = (currentMode == QueenMode.Heal) ? QueenMode.Attack : QueenMode.Heal;
        //UpdateQueenSprite();
    }

    public void UpdateQueenSprite()
    {
        var sr = GetComponent<SpriteRenderer>();
        if (sr != null)
        {
            sr.sprite = (currentMode == QueenMode.Heal) ? healSprite : attackSprite;
        }

        if (isMounted)
        {
            sr.sprite = (currentMode == QueenMode.Heal) ? mountedHealSprite : mountedAttackSprite;
        }
    }

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
            new Vector2Int(-1, -1),
            new Vector2Int(0, 1),
            new Vector2Int(1, 0),
            new Vector2Int(0, -1),
            new Vector2Int(-1, 0)
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
                    //if (tiles[x, y].occupyingPiece.color != color)
                    //    moves.Add(new Vector3Int(x, y, 0));

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
        List<Vector3Int> targets = new List<Vector3Int>();
        Vector2Int[] directions =
        {
            new Vector2Int(1, 0), new Vector2Int(-1, 0),
            new Vector2Int(0, 1), new Vector2Int(0, -1),
            new Vector2Int(1, 1), new Vector2Int(-1, -1),
            new Vector2Int(1, -1), new Vector2Int(-1, 1)
        };

        foreach (var dir in directions)
        {
            int x = boardPosition.x + dir.x;
            int y = boardPosition.y + dir.y;

            while (IsInBounds(x, y))
            {
                var target = tiles[x, y].occupyingPiece;
                //if (target != null && target.color == this.color)
                //{
                //    targets.Add(new Vector3Int(x, y, 0));
                //    break;
                //}
                //else if (target != null)
                //{
                //    targets.Add(new Vector3Int(x, y, 0));
                //    break;
                //}
                targets.Add(new Vector3Int(x, y, 0));

                if (target != null)
                    break;


                x += dir.x;
                y += dir.y;
            }
        }

        return targets;
    }

    private bool IsInBounds(int x, int y) => x >= 0 && x < 8 && y >= 0 && y < 8;
}
