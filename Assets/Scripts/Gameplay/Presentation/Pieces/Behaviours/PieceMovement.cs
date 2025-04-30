using System;
using UnityEngine;

namespace Gameplay.Presentation.Pieces.Behaviours
{
    /// <summary>
    /// Abstract base class for animating a chess piece's movement to a new position.
    /// </summary>
    public abstract class PieceMovement : MonoBehaviour
    {
        public abstract void Move(Vector3 position, Action onCompleted);
    }
}