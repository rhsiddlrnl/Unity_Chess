using NUnit.Framework;
using System.Collections.Generic;
using System.Net;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using static Queen;

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
    public Canvas Canvas;

    public TMP_Text SelectedPieceText;
    public GameObject gameOverPanel;
    public TMP_Text resultText;
    public Button restartButton;
    public GameObject attackEffectUIPrefab;
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
            Debug.Log("턴 종료 버튼 비활성화.");
            endTurnButton.SetActive(false);
        }

        if (gameOverPanel != null)
        {
            Debug.Log("게임패널 비활성화.");
            gameOverPanel.SetActive(false);
        }
        foreach (Piece p in FindObjectsByType<Piece>(FindObjectsSortMode.None))
        {
            if (p is Queen queen)
            {
                queen.ToggleMode();
                queen.UpdateQueenSprite();
            }
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

            foreach (Piece p in FindObjectsByType<Piece>(FindObjectsSortMode.None))
            {
                var col = p.GetComponent<Collider2D>();
                if (col != null)
                    col.enabled = true;
            }

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

            if (selectedPiece is Knight && targetPiece != null && targetPiece.color == selectedPiece.color && !targetPiece.isMounted)
            {
                Debug.Log($"[나이트 탑승] {targetPiece.type}이(가) 나이트를 탑승합니다.");

                targetPiece.isMounted = true;

                var sr = targetPiece.GetComponent<SpriteRenderer>();
                if (sr != null && targetPiece.mountedSprite != null)
                    sr.sprite = targetPiece.mountedSprite;

                tiles[selectedPiece.boardPosition.x, selectedPiece.boardPosition.y].occupyingPiece = null;
                Destroy(selectedPiece.gameObject);

                selectedPiece = null;
                hasMovedThisTurn = true;
                UpdateSelectionUI(null);
                ClearMoveHighlights();

                if (endTurnButton != null)
                    endTurnButton.SetActive(true);

                Endturn();
                return;
            }

            if (selectedPiece is King king && king.isMounted && targetPiece != null && targetPiece.color != selectedPiece.color)
            {
                Debug.Log("[킹(말탑승) 상태] 적 기물 위로는 이동할 수 없습니다. 공격은 따로 수행하세요.");
                return;
            }

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
            //if (!(selectedPiece is King kingR) || kingR.isMounted)
            //{
            //    EnterAttackMode();
            //}

            //if (selectedPiece is King)
            //{
            //    selectedPiece = null;
            //    UpdateSelectionUI(null);
            //}

            if (selectedPiece is King kingCheck)
            {
                if (!kingCheck.isMounted)
                {
                    selectedPiece = null;
                    UpdateSelectionUI(null);
                }
                else
                {
                    EnterAttackMode();
                }
            }
            //else if(selectedPiece is Knight)
            //{
            //    Endturn();
            //}
            else
            {
                EnterAttackMode();
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
            if (selectedPiece is Knight)
                Endturn();

        }
        else
        {
            Debug.Log("[DEBUG] 선택된 위치는 유효한 이동 칸이 아닙니다.");
        }
    }

    public void Endturn()
    {
        foreach (Piece p in FindObjectsByType<Piece>(FindObjectsSortMode.None))
        {
            if (p is Queen queen && queen.color == currentTurn)
            {
                queen.ToggleMode();
                queen.UpdateQueenSprite();
            }
        }

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

            //if (occupant != null && occupant.color == selectedPiece.color)
            //    continue;

            if (!(selectedPiece is Knight))
            {
                if (occupant != null && occupant.color == selectedPiece.color)
                    continue;
            }

            if (selectedPiece is King king && king.isMounted)
            {
                if (occupant != null && occupant.color != selectedPiece.color)
                    continue;
            }


            Vector3 worldPos = new Vector3(pos.x + boardOrigin.x, pos.y + boardOrigin.y, -0.5f);
            GameObject highlight = Instantiate(moveHighlightPrefab, worldPos, Quaternion.identity);
            var renderer = highlight.GetComponent<SpriteRenderer>();

            if (selectedPiece is King kingR && !kingR.isMounted && occupant != null && occupant.color != selectedPiece.color)
            {
                if (renderer != null)
                    renderer.color = Color.red;
            }
            else if (selectedPiece is Knight && occupant != null && occupant.color == selectedPiece.color && !occupant.isMounted)
            {
                
                if (renderer != null)
                    renderer.color = Color.cyan;
            }
            else if (selectedPiece is Knight && occupant != null && occupant.color == selectedPiece.color)
            {
                Destroy(highlight);
                continue;
            }

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
    //void ShowAttackHighlights(List<Vector3Int> positions)
    //{
    //    ClearAttackHighlights();
    //    foreach (var pos in positions)
    //    {
    //        if (!IsInBounds(pos)) continue;

    //        Tile tile = tiles[pos.x, pos.y];
    //        Piece occupant = tile.occupyingPiece;

    //        if (selectedPiece is Queen)
    //        {
    //            if (occupant != null && occupant.color == selectedPiece.color && occupant.shield <= 0)
    //            {
    //                Vector3 worldPosQ = new Vector3(pos.x + boardOrigin.x, pos.y + boardOrigin.y, -0.4f);
    //                GameObject highlightQ = Instantiate(attackHighlightPrefab, worldPosQ, Quaternion.identity);
    //                attackHighlights.Add(highlightQ);
    //            }

    //            continue;
    //        }

    //        if (occupant != null && occupant.color == selectedPiece.color)
    //            continue;


    //        Vector3 worldPos = new Vector3(pos.x + boardOrigin.x, pos.y + boardOrigin.y, -0.4f);
    //        GameObject highlight = Instantiate(attackHighlightPrefab, worldPos, Quaternion.identity);

    //        attackHighlights.Add(highlight);
    //    }
    //}
    void ShowAttackHighlights(List<Vector3Int> positions)
    {
        ClearAttackHighlights();

        foreach (var pos in positions)
        {
            if (!IsInBounds(pos)) continue;

            Tile tile = tiles[pos.x, pos.y];
            Piece occupant = tile.occupyingPiece;

            Vector3 worldPos = new Vector3(pos.x + boardOrigin.x, pos.y + boardOrigin.y, -0.4f);

            if (selectedPiece is Queen queen)
            {
                GameObject highlightQ = Instantiate(attackHighlightPrefab, worldPos, Quaternion.identity);
                var renderer = highlightQ.GetComponent<SpriteRenderer>();

                if (queen.currentMode == QueenMode.Heal)
                {
                    // 힐 모드: 아군 또는 빈 칸일 때만 연두색
                    if (occupant == null || occupant.color == queen.color)
                    {
                        if (renderer != null)
                            renderer.color = new Color(0.5f, 1f, 0.5f);  // 연두색
                        attackHighlights.Add(highlightQ);
                    }
                    else
                    {
                        Destroy(highlightQ);  // 적군일 경우 제거
                    }
                }
                else // 공격 모드
                {
                    // 공격 모드: 적군 또는 빈 칸이면 빨간색
                    if (occupant == null || occupant.color != queen.color)
                    {
                        if (renderer != null)
                            renderer.color = Color.red;
                        attackHighlights.Add(highlightQ);
                    }
                    else
                    {
                        Destroy(highlightQ);  // 아군일 경우 제거
                    }
                }

                continue;
            }

            // 퀸이 아닌 경우: 적군만 표시
            if (occupant != null && occupant.color == selectedPiece.color)
                continue;

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
                    ShowAttackEffectUI(selectedPiece);
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


        if (selectedPiece is King king)
        {
            if(!king.isMounted)
            {
                return;
            }
            var attackTiles = selectedPiece.GetAttackableTiles(tiles);

            Vector3Int targetK = tile.boardPosition;
            targetK.z = 0;

            if (attackTiles.Contains(targetK))
            {
                var targetPiece = tiles[targetK.x, targetK.y].occupyingPiece;

                if (targetPiece != null)
                {
                    Debug.Log($"[킹 타겟 정보] type: {targetPiece.type}, color: {targetPiece.color}, 내 색상: {selectedPiece.color}");

                    if (targetPiece.color != selectedPiece.color)
                    {
                        ShowAttackEffectUI(selectedPiece);
                        Debug.Log($"[킹 즉사 공격] {targetPiece.type} 파괴됨");
                        targetPiece.TakeDamage(999);
                        //instant kill
                    }
                    else
                    {
                        Debug.Log("[킹 공격 실패] 아군 기물입니다.");
                        return;
                    }
                }
                else
                {
                    Debug.Log("[킹 공격 실패] 해당 위치에 기물이 없습니다.");
                    return;
                }
            }
            else
            {
                Debug.Log($"[킹 공격 실패] attackTiles에 위치 {targetK} 없음");
                return;
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

        //if (selectedPiece is Queen)
        //{
        //    var healableTiles = selectedPiece.GetAttackableTiles(tiles);
        //    Vector3Int targetQ = tile.boardPosition;
        //    targetQ.z = 0;

        //    if (healableTiles.Contains(targetQ))
        //    {
        //        var ally = tiles[targetQ.x, targetQ.y].occupyingPiece;
        //        if (ally != null && ally.color == selectedPiece.color)
        //        {
        //            if (ally.currentHealth < ally.maxHealth)
        //            {
        //                ally.currentHealth++;
        //                Debug.Log($"[퀸 치유] {ally.type}의 체력을 1 회복했습니다.");
        //            }
        //            else if (ally.shield == 0)
        //            {
        //                ally.shield = 1;

        //                Debug.Log($"[퀸 보호막] {ally.type}에게 1회 방어막을 부여했습니다.");
        //            }
        //            else
        //            {
        //                Debug.Log($"[퀸 보호막 무시] 이미 방어막이 있습니다.");
        //                return;
        //            }

        //            hasAttackedThisTurn = true;
        //            ExitAttackMode();
        //            selectedPiece = null;
        //            UpdateSelectionUI(null);
        //            Endturn();
        //        }
        //    }

        //    return;
        //}

        if (selectedPiece is Queen queen)
        {
            var targetQ = tiles[tile.boardPosition.x, tile.boardPosition.y].occupyingPiece;

            if (queen.currentMode == QueenMode.Heal)
            {
                if (targetQ != null && targetQ.color == queen.color)
                {
                    if (targetQ.currentHealth < targetQ.maxHealth)
                    {
                        targetQ.currentHealth++;
                        Debug.Log($"[퀸 치유] {targetQ.type} 체력 +1");
                    }
                    else if (targetQ.shield == 0)
                    {
                        targetQ.shield = 1;
                        Debug.Log($"[퀸 쉴드] {targetQ.type} 방어막 +1");
                    }
                    else
                    {
                        Debug.Log("[퀸 힐 무효] 체력도 쉴드도 가득 참");
                        return;
                    }
                }
                else
                {
                    return;
                }
            }
            else
            {
                if (targetQ != null && targetQ.color != queen.color)
                {
                    targetQ.TakeDamage(queen.damage);
                    Debug.Log($"[퀸 공격] {targetQ.type}에게 피해");
                }
                else
                {
                    return;
                }
            }

            hasAttackedThisTurn = true;
            ExitAttackMode();
            UpdateSelectionUI(null);
            selectedPiece = null;
            Endturn();

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
                ShowAttackEffectUI(selectedPiece);
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

    public void EndGame(PieceColor winner)
    {
        Debug.Log($"게임 종료! 승리: {winner}");

        hasMovedThisTurn = true;
        hasAttackedThisTurn = true;
        isInAttackMode = false;

        selectedPiece = null;
        UpdateSelectionUI(null);

        ClearMoveHighlights();
        ClearAttackHighlights();

        if (endTurnButton != null)
            endTurnButton.SetActive(false);

        if (gameOverPanel != null)
        {
            Debug.Log($"게임 판넬 호출");
            gameOverPanel.SetActive(true);
            Debug.Log($"게임 판넬 성공");
            if (resultText != null)
                resultText.text = $"{winner} Wins!";
        }
    }

    public void RestartGame()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(
            UnityEngine.SceneManagement.SceneManager.GetActiveScene().name
        );
    }

    public void ShowAttackEffectUI(Piece attacker)
    {
        if (attacker.attackEffectSprite == null)
        {
            Debug.LogWarning($"[경고] {attacker.name}의 attackEffectSprite가 비어 있습니다.");
            return;
        }

        GameObject effect = Instantiate(attackEffectUIPrefab, Canvas.transform);
        var image = effect.GetComponent<UnityEngine.UI.Image>();
        image.sprite = attacker.attackEffectSprite;

        RectTransform rt = effect.GetComponent<RectTransform>();
        rt.anchoredPosition = Vector2.zero;

        effect.transform.SetAsLastSibling();
        Destroy(effect, 0.7f);
    }
}
