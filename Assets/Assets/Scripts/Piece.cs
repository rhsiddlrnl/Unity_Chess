using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;

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

    public virtual List<Vector3Int> GetAvailableMoves(Tile[,] tiles)
    {
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
   
}
