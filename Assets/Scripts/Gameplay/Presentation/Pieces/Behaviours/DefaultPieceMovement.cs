using System;
using DG.Tweening;
using UnityEngine;

namespace Gameplay.Presentation.Pieces.Behaviours
{
    /// <summary>
    /// Animates a piece's movement to a new position using a linear tween.
    /// </summary>
    public class DefaultPieceMovement : PieceMovement
    {
        [SerializeField] private AudioSource audioSource;
        [SerializeField] private AudioClip audioClip;
        private Sequence moveSequence;

        public override void Move(Vector3 position, Action onCompleted)
        {
            moveSequence?.Kill();
            moveSequence = DOTween.Sequence();
            audioSource.PlayOneShot(audioClip);
            moveSequence.Append(transform.DOMove(position, 0.5f));
            moveSequence.OnComplete(() => onCompleted?.Invoke());
        }
    }
}