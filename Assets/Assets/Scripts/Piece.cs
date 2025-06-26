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
    public int shield = 0;

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
        //Debug.Log($"[Start 호출됨] {name}");
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
        if (shield > 0)
        {
            shield--;
            dmg--;
        }

        currentHealth -= dmg;

        StartCoroutine(HitEffectRoutine());

        if (currentHealth <= 0)
        {
            StartCoroutine(DieAfterDelay(2.0f)); // 연출이 끝난 뒤 파괴
            //Die();
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
    private IEnumerator HitEffectRoutine()
    {
        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        if (sr != null)
        {
            for (int i = 0; i < 3; i++)
            {
                sr.color = new Color(1.0f, 1.0f, 1.0f, 0.3f);
                yield return new WaitForSeconds(0.2f);
                sr.color = Color.white;
                yield return new WaitForSeconds(0.2f);
            }
        }
    }

    private IEnumerator DieAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        Die();
    }

    //public void UpdateShieldVisual()
    //{
    //    Debug.Log($"[{name}] 현재 쉴드 수치: {shield}");
    //    if (shieldOutline != null)
    //    {
    //        var sr = shieldOutline.GetComponent<SpriteRenderer>();
    //        bool active = (shield > 0);
    //        shieldOutline.SetActive(active);
    //        //Debug.Log($"[쉴드 표시] {name} → ShieldOutline 활성화 상태: {active}");


    //        if (sr != null)
    //        {
    //            sr.enabled = active;
    //            if (active)
    //            {
    //                shieldOutline.transform.localPosition = new Vector3(0, 0, -0.1f);  // 중심 정렬

    //                //shieldOutline.transform.localScale = new Vector3(1f, 1f, 1f);
    //            }

    //        }
    //        else
    //        {
    //            Debug.LogWarning($"[쉴드 표시 실패] {name}의 ShieldOutline에 SpriteRenderer가 없습니다.");
    //        }
    //    }
    //}

}
