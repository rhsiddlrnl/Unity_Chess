using UnityEngine;

public class Tile : MonoBehaviour
{
    public Vector3Int boardPosition;

    public Piece occupyingPiece;

    public bool HasNoPiece()
    {
        return occupyingPiece == null;
    }
    public void SetPosition(int x, int y)
    {
        boardPosition = new Vector3Int(x, y, 0);
    }

    private void OnMouseDown()
    {
        Debug.Log($"Tile Clicked: {boardPosition}");
        BoardManager bm = FindFirstObjectByType<BoardManager>();
        bm.OnTileClicked(this);
    }
}
