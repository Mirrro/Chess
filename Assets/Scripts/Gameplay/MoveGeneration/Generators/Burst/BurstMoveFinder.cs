using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Gameplay.Execution.Models;
using Gameplay.Execution.Moves;
using Gameplay.Execution.Moves.Steps;
using Unity.Collections;
using Unity.Jobs;
using Unity.Mathematics;
using UnityEngine;

namespace Gameplay.MoveGeneration.Generators
{
    public class BurstMoveFinder
    {
        public List<IGameplayMove> RunJob(GameplayStateModel gameplayStateModel, bool isColor, bool isAi)
        {
            var moveList = new NativeList<MoveData>(512, Allocator.TempJob);
            var board = ToBoardData(gameplayStateModel, Allocator.TempJob);
            // var sw = Stopwatch.StartNew();
            
            var job = new GenerateAllMovesJob
            {
                Board = board,
                IsAi = isAi,
                IsColor = isColor,
                OutputMoves = moveList.AsParallelWriter()
            };
            var handle = job.Schedule(board.Pieces.Length, 1);
            handle.Complete();

            var results = ConvertToGameplayMoves(moveList);
            //
            // sw.Stop();
            // Debug.Log($"Move generation took {sw.Elapsed.TotalMilliseconds:F8} ms");
            moveList.Dispose();
            board.Pieces.Dispose();
            return results;
        }

        private static BoardData ToBoardData(GameplayStateModel model, Allocator allocator)
        {
            var pieceArray = new NativeArray<PieceData>(model.PieceMap.Count, allocator);
            int i = 0;
            foreach (var piece in model.PieceMap.Values)
            {
                pieceArray[i++] = new PieceData
                {
                    Id = piece.Id,
                    IsColor = piece.IsColor,
                    PieceType = piece.PieceType,
                    Position = new int2(piece.Position.x, piece.Position.y),
                    HasMoved = piece.HasMoved,
                };
            }

            return new BoardData
            {
                Pieces = pieceArray,
                TurnCount = model.TurnCount,
                EnPassantPieceId = model.EnPassantPieceId,
                EnPassantTurn = model.EnPassantTurn,
                EnPassantTrapPosition = new int2(model.EnPassantTrapPosition.x, model.EnPassantTrapPosition.y)
            };
        }


        private static List<IGameplayMove> ConvertToGameplayMoves(NativeList<MoveData> jobResults)
        {
            var result = new List<IGameplayMove>();

            foreach (var move in jobResults)
            {
                var steps = new List<IGameplayStep>();

                if (move.MoveStepData.HasValue)
                {
                    MoveStepData data = move.MoveStepData.Value;
                    int2 targetPosition = data.TargetPosition;
                    
                    steps.Add(new MovePieceStep(data.PieceToMoveId, new Vector2Int(targetPosition.x, targetPosition.y)));
                }

                if (move.EnPassantStepData.HasValue)
                {
                    EnPassantStepData data = move.EnPassantStepData.Value;
                    int2 targetPosition = data.TargetPosition;
                    
                    steps.Add(new SetEnPassantTrapStep(new Vector2Int(targetPosition.x, targetPosition.y), data.TargetPieceId));
                }

                if (move.CaptureStepData.HasValue)
                {
                    CaptureStepData data = move.CaptureStepData.Value;
                    
                    steps.Add(new CapturePieceStep(data.PieceCapturingId, data.PieceToCaptureId));
                }

                if (move.PromoteStepData.HasValue)
                {
                    PromoteStepData data = move.PromoteStepData.Value;

                    if (data.PromotionType == PromotionTypes.Undefined)
                    {
                        steps.Add(new PlayerPromotionStep(data.PieceToPromoteId));
                    }
                    else
                    {
                        steps.Add(new PromotePieceStep(data.PieceToPromoteId, ToPieceType(data.PromotionType)));
                    }
                }

                if (move.CastlingMoveData.HasValue)
                {
                    CastlingMoveData data = move.CastlingMoveData.Value;
                    int2 kingPosition = data.KingPosition;
                    int2 rookPosition = data.RookPosition;
                    
                    steps.Add(new MovePieceStep(data.KingToCastlingId, new Vector2Int(kingPosition.x, kingPosition.y)));
                    steps.Add(new MovePieceStep(data.RookToCastlingId, new Vector2Int(rookPosition.x, rookPosition.y)));
                }

                result.Add(new GameplayMove(new Vector2Int(move.FromPosition.x, move.FromPosition.y), new Vector2Int(move.TargetPosition.x, move.TargetPosition.y) , steps));
            }

            return result;
        }

        private static PieceType ToPieceType(PromotionTypes promotionTypes)
        {
            return promotionTypes switch
            {
                PromotionTypes.Knight => PieceType.Knight,
                PromotionTypes.Rook => PieceType.Rook,
                PromotionTypes.Bishop => PieceType.Bishop,
                PromotionTypes.Queen => PieceType.Queen,
                _ => throw new ArgumentOutOfRangeException(nameof(promotionTypes), promotionTypes, null)
            };
        }

    }
}