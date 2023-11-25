using Cysharp.Threading.Tasks;
using UnityEngine;
using Random = UnityEngine.Random;

public class Main : MonoBehaviour
{
    [SerializeField] private BoardView view;
    public BoardState activeState = new ();
    
    private bool isColor = true;
    private BoardEvaluator evaluator = new ();
    private ChessAi ai = new ();
    private Player player;
    
    void Start()
    {
        Init();
        StartGame();
    }

    private void Init()
    {
        ai = new ChessAi();
        player = new Player(view);
        PopulateBoard();
    }

    private async UniTask StartGame()
    {
        await view.VisualizeBoardState(activeState);
        GameLoop().Forget();
    }

    private async UniTask GameLoop()
    {
        if (isColor)
        {
            activeState = await player.WaitForMove(activeState);
        }
        else
        {
            BoardState[] nextMoves = await ai.FindBestMove(activeState, false, 3);
            activeState = nextMoves[Random.Range(0, nextMoves.Length)];
        }
        
        await view.VisualizeBoardState(activeState);

        isColor = !isColor;

        if (Application.isPlaying && !evaluator.IsGameOver(activeState))
        {
            GameLoop().Forget();
        }
    }

    private void PopulateBoard()
    {
        activeState.Fields = new BoardField[8,8]
        {
            { new (), new (), new (), new (), new (), new (), new (), new ()},
            { new (), new (), new (), new (), new (), new (), new (), new ()},
            { new (), new (), new (), new (), new (), new (), new (), new ()},
            { new (), new (), new (), new (), new (), new (), new (), new ()},
            { new (), new (), new (), new (), new (), new (), new (), new ()},
            { new (), new (), new (), new (), new (), new (), new (), new ()},
            { new (), new (), new (), new (), new (), new (), new (), new ()},
            { new (), new (), new (), new (), new (), new (), new (), new ()},
        };
        // White
        activeState.Fields[0, 0].Piece = new RuckPiece(isWhite: true);
        activeState.Fields[1, 0].Piece = new KnightPiece(isWhite: true);
        activeState.Fields[2, 0].Piece = new BishopPiece(isWhite: true);
        activeState.Fields[3, 0].Piece = new KingPiece(isWhite: true);
        activeState.Fields[4, 0].Piece = new QueenPiece(isWhite: true);
        activeState.Fields[5, 0].Piece = new BishopPiece(isWhite: true);
        activeState.Fields[6, 0].Piece = new KnightPiece(isWhite: true);
        activeState.Fields[7, 0].Piece = new RuckPiece(isWhite: true);
        
        activeState.Fields[0, 1].Piece = new PawnPiece(isWhite: true);
        activeState.Fields[1, 1].Piece = new PawnPiece(isWhite: true);
        activeState.Fields[2, 1].Piece = new PawnPiece(isWhite: true);
        activeState.Fields[3, 1].Piece = new PawnPiece(isWhite: true);
        activeState.Fields[4, 1].Piece = new PawnPiece(isWhite: true);
        activeState.Fields[5, 1].Piece = new PawnPiece(isWhite: true);
        activeState.Fields[6, 1].Piece = new PawnPiece(isWhite: true);
        activeState.Fields[7, 1].Piece = new PawnPiece(isWhite: true);
        
        // Black
        activeState.Fields[0, 7].Piece = new RuckPiece(isWhite: false);
        activeState.Fields[1, 7].Piece = new KnightPiece(isWhite: false);
        activeState.Fields[2, 7].Piece = new BishopPiece(isWhite: false);
        activeState.Fields[3, 7].Piece = new KingPiece(isWhite: false);
        activeState.Fields[4, 7].Piece = new QueenPiece(isWhite: false);
        activeState.Fields[5, 7].Piece = new BishopPiece(isWhite: false);
        activeState.Fields[6, 7].Piece = new KnightPiece(isWhite: false);
        activeState.Fields[7, 7].Piece = new RuckPiece(isWhite: false);
        
        activeState.Fields[0, 6].Piece = new PawnPiece(isWhite: false);
        activeState.Fields[1, 6].Piece = new PawnPiece(isWhite: false);
        activeState.Fields[2, 6].Piece = new PawnPiece(isWhite: false);
        activeState.Fields[3, 6].Piece = new PawnPiece(isWhite: false);
        activeState.Fields[4, 6].Piece = new PawnPiece(isWhite: false);
        activeState.Fields[5, 6].Piece = new PawnPiece(isWhite: false);
        activeState.Fields[6, 6].Piece = new PawnPiece(isWhite: false);
        activeState.Fields[7, 6].Piece = new PawnPiece(isWhite: false);
    }
}