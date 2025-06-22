using NUnit.Framework;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

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
    public GameObject endTurnButton;


    public GameObject moveHighlightPrefab;
    private List<GameObject> activeHighlights = new List<GameObject>();
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
        if(piece.color != currentTurn || hasMovedThisTurn)
        {
            return;
        }
        selectedPiece = piece;

        var moves = piece.GetAvailableMoves(tiles);

        ShowMoveHighlights(moves);
        Debug.Log($"{piece.type} selected at {piece.boardPosition}. Possible moves: {moves.Count}");
    }

    public void OnTileClicked(Tile tile)
    {
        if (selectedPiece == null || hasMovedThisTurn)
        {
            return;
        }

        Vector3Int target = tile.boardPosition;
        target.z = 0;

        var validMoves = selectedPiece.GetAvailableMoves(tiles);

        Debug.Log($"[DEBUG] Clicked tile: {target}, Moves count: {validMoves.Count}");
        foreach (var move in validMoves)
            Debug.Log($"Valid move: {move}");

        if (validMoves.Contains(target))
        {
            Vector3Int prevPos = selectedPiece.boardPosition;

            tiles[prevPos.x, prevPos.y].occupyingPiece = null;
            tiles[target.x, target.y].occupyingPiece = selectedPiece;


            selectedPiece.boardPosition = target;

            Vector3 worldPos = new Vector3(target.x+boardOrigin.x, target.y+boardOrigin.y, -1);
            selectedPiece.transform.position = worldPos;
            
            hasMovedThisTurn = true;
            selectedPiece = null;

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
        selectedPiece= null;

        if(endTurnButton != null)
        {
            endTurnButton.SetActive(false);
        }

        Debug.Log($"턴이 바뀜: {currentTurn}");
    }

    void ShowMoveHighlights(List<Vector3Int> positions)
    {
        ClearMoveHighlights();

        foreach(var pos in positions)
        {
            if (!tiles[pos.x, pos.y].HasNoPiece())
                continue;

            Vector3 worldPos = new Vector3(pos.x + boardOrigin.x, pos.y + boardOrigin.y, -0.5f);
            GameObject highlight = Instantiate(moveHighlightPrefab, worldPos, Quaternion.identity);
            activeHighlights.Add(highlight);
        }
    }

    void ClearMoveHighlights()
    {
        foreach(var highlight in activeHighlights)
        {
            Destroy(highlight);
        }
        activeHighlights.Clear();
    }
}
