using NUnit.Framework;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using TMPro;

public class BoardManager : MonoBehaviour
{
    public GameObject tilePrefab;
    private int size = 8;
    public Tile[,] tiles = new Tile[8, 8];

    public GameObject whitePawnPrefab;
    public GameObject blackPawnPrefab;
    public GameObject whiteRookPrefab;
    public GameObject blackRookPrefab;
    public GameObject whiteKnightPrefab;
    public GameObject blackKnightPrefab;
    public GameObject whiteBishopPrefab;
    public GameObject blackBishopPrefab;
    public GameObject whiteQueenPrefab;
    public GameObject blackQueenPrefab;
    public GameObject whiteKingPrefab;
    public GameObject blackKingPrefab;

    private Piece selectedPiece = null;

    public Vector2 boardOrigin = new Vector2(-3.5f, -3.5f);


    public PieceColor currentTurn = PieceColor.White;
    private bool hasMovedThisTurn = false;
    private bool hasAttackedThisTurn = false;
    public GameObject endTurnButton;
    private bool isInAttackMode = false;

    //moveable
    public GameObject moveHighlightPrefab;
    private List<GameObject> activeHighlights = new List<GameObject>();

    //attackable
    public GameObject attackHighlightPrefab;
    private List<GameObject> attackHighlights = new List<GameObject>();

    //UI
    public TMP_Text SelectedPieceText;

    public bool IsInAttackMode() => isInAttackMode;
    void Start()
    {
        GenerateBoard();

        CreatePiece(whitePawnPrefab, PieceType.Pawn, PieceColor.White, 0, 1);
        CreatePiece(whitePawnPrefab, PieceType.Pawn, PieceColor.White, 1, 1);
        CreatePiece(whitePawnPrefab, PieceType.Pawn, PieceColor.White, 2, 1);
        CreatePiece(whitePawnPrefab, PieceType.Pawn, PieceColor.White, 3, 1);
        CreatePiece(whitePawnPrefab, PieceType.Pawn, PieceColor.White, 4, 1);
        CreatePiece(whitePawnPrefab, PieceType.Pawn, PieceColor.White, 5, 1);
        CreatePiece(whitePawnPrefab, PieceType.Pawn, PieceColor.White, 6, 1);
        CreatePiece(whitePawnPrefab, PieceType.Pawn, PieceColor.White, 7, 1);

        CreatePiece(whiteRookPrefab, PieceType.Rook, PieceColor.White, 0, 0);
        CreatePiece(whiteRookPrefab, PieceType.Rook, PieceColor.White, 7, 0);

        CreatePiece(whiteKnightPrefab, PieceType.Knight, PieceColor.White, 1, 0);
        CreatePiece(whiteKnightPrefab, PieceType.Knight, PieceColor.White, 6, 0);

        CreatePiece(whiteBishopPrefab, PieceType.Bishop, PieceColor.White, 2, 0);
        CreatePiece(whiteBishopPrefab, PieceType.Bishop, PieceColor.White, 5, 0);

        CreatePiece(whiteQueenPrefab, PieceType.Queen, PieceColor.White, 3, 0);

        CreatePiece(whiteKingPrefab, PieceType.King, PieceColor.White, 4, 0);

        CreatePiece(blackPawnPrefab, PieceType.Pawn, PieceColor.Black, 0, 6);
        CreatePiece(blackPawnPrefab, PieceType.Pawn, PieceColor.Black, 1, 6);
        CreatePiece(blackPawnPrefab, PieceType.Pawn, PieceColor.Black, 2, 6);
        CreatePiece(blackPawnPrefab, PieceType.Pawn, PieceColor.Black, 3, 6);
        CreatePiece(blackPawnPrefab, PieceType.Pawn, PieceColor.Black, 4, 6);
        CreatePiece(blackPawnPrefab, PieceType.Pawn, PieceColor.Black, 5, 6);
        CreatePiece(blackPawnPrefab, PieceType.Pawn, PieceColor.Black, 6, 6);
        CreatePiece(blackPawnPrefab, PieceType.Pawn, PieceColor.Black, 7, 6);

        CreatePiece(blackRookPrefab, PieceType.Rook, PieceColor.Black, 0, 7);
        CreatePiece(blackRookPrefab, PieceType.Rook, PieceColor.Black, 7, 7);

        CreatePiece(blackKnightPrefab, PieceType.Knight, PieceColor.Black, 1, 7);
        CreatePiece(blackKnightPrefab, PieceType.Knight, PieceColor.Black, 6, 7);

        CreatePiece(blackBishopPrefab, PieceType.Bishop, PieceColor.Black, 2, 7);
        CreatePiece(blackBishopPrefab, PieceType.Bishop, PieceColor.Black, 5, 7);

        CreatePiece(blackQueenPrefab, PieceType.Queen, PieceColor.Black, 3, 7);

        CreatePiece(blackKingPrefab, PieceType.King, PieceColor.Black, 4, 7);

        if (endTurnButton != null)
        {
            endTurnButton.SetActive(false);
        }
    }

    void GenerateBoard()
    {
        for(int y = 0; y<size; y++)
        {
            for(int x = 0; x<size; x++)
            {
                Vector3 spawnPosition = new Vector3(x + boardOrigin.x, y + boardOrigin.y, 0);
                GameObject tileObj = Instantiate(tilePrefab, spawnPosition, Quaternion.identity);
                tileObj.name = $"Tile_{x}_{y}";

                SpriteRenderer sr = tileObj.GetComponent<SpriteRenderer>();
                sr.color = (x + y) % 2 == 0 ? Color.white : Color.gray;

                Tile tileScript = tileObj.GetComponent<Tile>();
                tileScript.SetPosition(x, y);
                tiles[x, y] = tileScript;
            }
        }
    }

    void CreatePiece(GameObject prefab, PieceType type, PieceColor color, int x, int y)
    {
        GameObject obj = Instantiate(prefab, new Vector3(x, y, 0), Quaternion.identity);
        Piece piece = obj.GetComponent<Piece>();
        Vector3Int position = new Vector3Int(x, y, 0);
        Vector3 worldPosition = new Vector3(x + boardOrigin.x, y+ boardOrigin.y, -1);
        piece.SetData(type, color, position);
        piece.transform.position = worldPosition;

        tiles[x,y].occupyingPiece = piece;
    }

    public void OnPieceSelected(Piece piece)
    {
        if (hasMovedThisTurn || piece.color != currentTurn)
            return;

        foreach (Piece p in FindObjectsByType<Piece>(FindObjectsSortMode.None))
        {
            var col = p.GetComponent<Collider2D>();
            if (col != null)
                col.enabled = false;
        }

        var myCollider = piece.GetComponent<Collider2D>();
        if (myCollider != null)
            myCollider.enabled = true;


        if (selectedPiece == piece)
        {
            selectedPiece = null;
            ClearMoveHighlights();
            UpdateSelectionUI(null);
            return;
        }

        if (selectedPiece != null)
        {
            Debug.Log("[선택 무시] 다른 기물이 이미 선택되어 있습니다. 다시 클릭하여 해제하세요.");
            return;
        }

        selectedPiece = piece;

        var moves = piece.GetAvailableMoves(tiles);

        ShowMoveHighlights(moves);
        UpdateSelectionUI(piece);

        Debug.Log($"{piece.type} selected at {piece.boardPosition}. Possible moves: {moves.Count}");
    }

    public void OnTileClicked(Tile tile)
    {
        if (selectedPiece == null || hasMovedThisTurn)
        {
            return;
        }

        Vector3Int target = new Vector3Int(tile.boardPosition.x, tile.boardPosition.y, 0);

        var validMoves = selectedPiece.GetAvailableMoves(tiles);

        Debug.Log($"[DEBUG] Clicked tile: {target}, Moves count: {validMoves.Count}");
        foreach (var move in validMoves)
            Debug.Log($"Valid move: {move}");

        bool canMove = false;
        foreach(var move in validMoves)
        {
            if(move.x == target.x && move.y == target.y)
            {
                canMove = true;
                break;
            }
        }


        if (canMove)
        {
            Vector3Int prevPos = selectedPiece.boardPosition;


            //if (selectedPiece is King)
            //{
            //    var targetPiece = tiles[target.x, target.y].occupyingPiece;

            //    if (targetPiece != null && targetPiece.color != selectedPiece.color)
            //    {
            //        Debug.Log($"[킹 이동 공격] {targetPiece.type} 파괴됨!");
            //        Destroy(targetPiece.gameObject);

            //        hasAttackedThisTurn = true;

            //        selectedPiece.GetComponent<King>().OnSuccessfulAttack(targetPiece);
            //    }
            //}
            var targetPiece = tiles[target.x, target.y].occupyingPiece;

            if (targetPiece != null && targetPiece.color != selectedPiece.color)
            {
                Debug.Log($"[킹 이동 공격] {targetPiece.type} 제거됨");
                Destroy(targetPiece.gameObject);
            }

            tiles[prevPos.x, prevPos.y].occupyingPiece = null;
            tiles[target.x, target.y].occupyingPiece = selectedPiece;


            selectedPiece.boardPosition = target;

            Vector3 worldPos = new Vector3(target.x+boardOrigin.x, target.y+boardOrigin.y, -1);
            selectedPiece.transform.position = worldPos;
            
            hasMovedThisTurn = true;
            if(!(selectedPiece is King))
            {
                EnterAttackMode();
            }

            if (selectedPiece is King)
            {
                selectedPiece = null;
                UpdateSelectionUI(null);
            }

            //isInAttackMode = true;

            //var attackTiles = selectedPiece.GetAttackableTiles(tiles);
            //ShowAttackHighlights(attackTiles);

            //selectedPiece = null;

            ClearMoveHighlights();

            if(endTurnButton != null)
            {
                endTurnButton.SetActive(true);
            }
        }
        else
        {
            Debug.Log("[DEBUG] 선택된 위치는 유효한 이동 칸이 아닙니다.");
        }
    }

    public void Endturn()
    {
        currentTurn = (currentTurn == PieceColor.White) ? PieceColor.Black : PieceColor.White;
        hasMovedThisTurn= false;
        hasAttackedThisTurn = false;
        isInAttackMode= false;
        selectedPiece= null;

        ClearMoveHighlights();
        ClearAttackHighlights();

        if (endTurnButton != null)
        {
            endTurnButton.SetActive(false);
        }
        UpdateSelectionUI(null);
        ExitAttackMode();
        Debug.Log($"턴이 바뀜: {currentTurn}");
    }

    void ShowMoveHighlights(List<Vector3Int> positions)
    {
        ClearMoveHighlights();

        foreach(var pos in positions)
        {
            if (!IsInBounds(pos)) continue;

            Tile tile = tiles[pos.x, pos.y];
            Piece occupant = tile.occupyingPiece;

            if (occupant != null && occupant.color == selectedPiece.color)
                continue;

            Vector3 worldPos = new Vector3(pos.x + boardOrigin.x, pos.y + boardOrigin.y, -0.5f);
            GameObject highlight = Instantiate(moveHighlightPrefab, worldPos, Quaternion.identity);
            activeHighlights.Add(highlight);
        }
    }
    void ClearMoveHighlights()
    {
        foreach (var highlight in activeHighlights)
        {
            Destroy(highlight);
        }
        activeHighlights.Clear();
    }
    void ShowAttackHighlights(List<Vector3Int> positions)
    {
        ClearAttackHighlights();
        foreach (var pos in positions)
        {
            if (!IsInBounds(pos)) continue;

            Tile tile = tiles[pos.x, pos.y];
            Piece occupant = tile.occupyingPiece;

            if (occupant != null && occupant.color == selectedPiece.color)
                continue;

            Vector3 worldPos = new Vector3(pos.x + boardOrigin.x, pos.y + boardOrigin.y, -0.4f);
            GameObject highlight = Instantiate(attackHighlightPrefab, worldPos, Quaternion.identity);

            attackHighlights.Add(highlight);
        }
    }
    private bool IsInBounds(Vector3Int pos)
    {
        return pos.x >= 0 && pos.x < 8 && pos.y >= 0 && pos.y < 8;
    }

    void ClearAttackHighlights()
    {
        foreach (var h in attackHighlights)
            Destroy(h);
        attackHighlights.Clear();
    }

    void EnterAttackMode()
    {
        isInAttackMode = true;

        foreach(Piece p in FindObjectsByType<Piece>(FindObjectsSortMode.None))
        {
            var collider = p.GetComponent<Collider2D>();
            if (collider != null)
            {
                collider.enabled = false;
            }
        }
        var attackTiles = selectedPiece.GetAttackableTiles(tiles);
        ShowAttackHighlights(attackTiles);
    }

    void ExitAttackMode()
    {
        isInAttackMode = false;

        foreach (Piece p in FindObjectsByType<Piece>(FindObjectsSortMode.None))
        {
            var collider = p.GetComponent<Collider2D>();
            if (collider != null)
                collider.enabled = true;
        }

        ClearAttackHighlights();
    }


    public void OnAttackTileClicked(Tile tile)
    {
        Debug.Log($"[공격 시도] selected: {selectedPiece?.type}, 타겟 위치: {tile.boardPosition}, 공격모드: {isInAttackMode}");
        if (!isInAttackMode || selectedPiece == null || hasAttackedThisTurn) return;

        if (selectedPiece is Rook)
        {
            var attackTiles = selectedPiece.GetAttackableTiles(tiles);
            foreach (var pos in attackTiles)
            {
                if(!IsInBounds(pos)) continue;

                Tile tileR = tiles[pos.x, pos.y];
                if (tileR == null) continue;

                Piece targetPiece = tileR.occupyingPiece;
                if (targetPiece == null) continue;

                if (targetPiece != null && targetPiece.color != selectedPiece.color)
                {
                    targetPiece.TakeDamage(selectedPiece.damage);
                    Debug.Log($"[AOE공격] {selectedPiece.type}이(가) {targetPiece.type}에게 피해");
                }
            }

            hasAttackedThisTurn = true;
            //isInAttackMode = false;
            //ClearAttackHighlights();
            selectedPiece = null;
            UpdateSelectionUI(null);
            Endturn();
            //if (endTurnButton != null)
            //    endTurnButton.SetActive(true);

            return;
        }


        if (selectedPiece is King)
        {
            var attackTiles = selectedPiece.GetAttackableTiles(tiles);

            if (attackTiles.Contains(tile.boardPosition))
            {
                var targetPiece = tiles[tile.boardPosition.x, tile.boardPosition.y].occupyingPiece;

                if (targetPiece != null && targetPiece.color != selectedPiece.color)
                {
                    Debug.Log($"[킹 즉사 공격] {targetPiece.type} 파괴됨");
                    Destroy(targetPiece.gameObject);
                }
            }

            hasAttackedThisTurn = true;
            //isInAttackMode = false;
            //ClearAttackHighlights();
            UpdateSelectionUI(null);
            selectedPiece = null;
            Endturn();

            //if (endTurnButton != null)
            //    endTurnButton.SetActive(true);

            return;
        }




        Vector3Int target = tile.boardPosition;
        target.z = 0;

        var attackables = selectedPiece.GetAttackableTiles(tiles);

        if (attackables.Contains(target))
        {
            var targetPiece = tiles[target.x, target.y].occupyingPiece;
            if (targetPiece != null && targetPiece.color != selectedPiece.color)
            {
                targetPiece.TakeDamage(selectedPiece.damage);
                hasAttackedThisTurn = true;
                ExitAttackMode();
                //isInAttackMode = false;
                //ClearAttackHighlights();
                selectedPiece = null;

                UpdateSelectionUI(null);
                Endturn();
                //if (endTurnButton != null)
                //    endTurnButton.SetActive(true);
            }
        }
    }


    void UpdateSelectionUI(Piece selected)
    {

        foreach (Piece p in FindObjectsByType<Piece>(FindObjectsSortMode.None))
        {
            p.GetComponent<SpriteRenderer>().color = Color.white;
        }

        if (selected != null)
        {
            //selected.GetComponent<SpriteRenderer>().color = Color.green;
            SelectedPieceText.text = $"Current Piece: {selected.type} ({selected.color})";
        }
        else
        {
            SelectedPieceText.text = "";
        }
    }
}
