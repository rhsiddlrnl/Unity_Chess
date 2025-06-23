using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Piece : MonoBehaviour
{
    public PieceType type;
    public PieceColor color;

    public Vector3Int boardPosition;
    private BoardManager boardManager;

    public int maxHealth = 1;
    public int currentHealth = 1;

    public int damage = 1;

    public bool moved = false;

    public bool isMounted = false;
    public Sprite normalSprite;
    public Sprite mountedSprite;
    public Sprite attackEffectSprite;

    public virtual List<Vector3Int> GetAvailableMoves(Tile[,] tiles)
    {
        if (isMounted)
        {
            return KnightStyleMoves(tiles);
        }
        return new List<Vector3Int>();
    }

    public virtual List<Vector3Int> GetAttackableTiles(Tile[,] tiles)
    {
        return new List<Vector3Int>();
    }
    protected void Start()
    {
        boardManager = FindAnyObjectByType<BoardManager>();
    }
    public void SetData(PieceType type, PieceColor color, Vector3Int position)
    {
        this.type = type;
        this.color = color;
        this.boardPosition = position;
        transform.position = new Vector3(position.x, position.y, position.z);
    }

    
    protected virtual void OnMouseDown()
    {
        BoardManager bm = FindFirstObjectByType<BoardManager>();
        bm.OnPieceSelected(this);
    }

    public virtual void TakeDamage(int dmg)
    {
        currentHealth -= dmg;

        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        if (sr != null)
            StartCoroutine(FlashEffect(sr));

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    protected virtual void Die()
    {
        Destroy(gameObject);
    }

    //public virtual List<Vector3Int> GetAttackableTiles(tile[,] tiles)
    //{
    //    return GetAttackableMoves(tiles);
    //}
   protected List<Vector3Int> KnightStyleMoves(Tile[,] tiles)
    {
        List<Vector3Int> moves = new List<Vector3Int>();
        Vector2Int[] jumps =
        {
            new Vector2Int(2, 1), new Vector2Int(1, 2),
            new Vector2Int(-1, 2), new Vector2Int(-2, 1),
            new Vector2Int(-2, -1), new Vector2Int(-1, -2),
            new Vector2Int(1, -2), new Vector2Int(2, -1)
        };

        foreach (var j in jumps)
        {
            int x = boardPosition.x + j.x;
            int y = boardPosition.y + j.y;

            if (x >= 0 && x < 8 && y >= 0 && y < 8)
            {
                var target = tiles[x, y].occupyingPiece;
                if (target == null)
                    moves.Add(new Vector3Int(x, y, 0));
            }
            //if (target == null || target.color != this.color) move like normal chess
        }
        return moves;
    }



    //animation
    IEnumerator FlashEffect(SpriteRenderer sr)
    {
        yield return new WaitForSeconds(0.2f);
        Color original = sr.color;

        sr.color = new Color(original.r, original.g, original.b, 0.5f); // Èå¸²
        yield return new WaitForSeconds(0.3f);
        sr.color = original;
    }
}
