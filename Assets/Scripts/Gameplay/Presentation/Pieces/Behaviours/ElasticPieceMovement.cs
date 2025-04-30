using System;
using DG.Tweening;
using UnityEngine;

namespace Gameplay.Presentation.Pieces.Behaviours
{
    /// <summary>
    /// Animates a piece with elastic movement and material bending when moving to a new position.
    /// </summary>
    public class ElasticPieceMovement : PieceMovement
    {
        [SerializeField] private Renderer renderer;
        [SerializeField] private float bendAmount = -0.15f;

        private Sequence moveSequence;

        public override void Move(Vector3 position, Action onCompleted)
        {
            moveSequence?.Kill();
            moveSequence = DOTween.Sequence();
            moveSequence.Join(transform.DOMove(position, .5f).SetEase(Ease.InSine)
                .OnComplete(() => onCompleted?.Invoke()));
            moveSequence.Join(renderer.material.DOFloat(bendAmount, "_BendAmount", .5f).SetEase(Ease.InSine));
            moveSequence.Join(transform.DOLookAt(position, 0.1f));
            moveSequence.Append(renderer.material.DOFloat(0, "_BendAmount", 1f).SetEase(Ease.OutElastic));
        }
    }
}