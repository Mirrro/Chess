using System;
using DG.Tweening;
using UnityEngine;

namespace Gameplay.Presentation.Pieces.Behaviours
{
    /// <summary>
    /// Animates a piece with a jumping motion when moving to a new board position.
    /// </summary>
    public class JumpingPieceMovement : PieceMovement
    {
        private Sequence moveSequence;

        public override void Move(Vector3 position, Action onCompleted)
        {
            moveSequence?.Kill();
            moveSequence = DOTween.Sequence();
            moveSequence.Append(transform.DOJump(position, 1, 1, 0.5f));
            moveSequence.OnComplete(() => onCompleted?.Invoke());
        }
    }
}