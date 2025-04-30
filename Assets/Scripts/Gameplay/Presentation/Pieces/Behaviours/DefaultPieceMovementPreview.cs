using UnityEngine;

namespace Gameplay.Presentation.Pieces.Behaviours
{
    /// <summary>
    /// Renders a line from the current piece position to a preview target for move visualization.
    /// </summary>
    public class DefaultPieceMovementPreview : PieceMovementPreview
    {
        [SerializeField] private LineRenderer lineRenderer;

        public override void PreviewMoveTo(Vector3 position)
        {
            lineRenderer.positionCount = 2;
            lineRenderer.SetPositions(new[] {transform.position, position});
        }

        public override void ClearPreview()
        {
            lineRenderer.positionCount = 0;
        }

        private void Start()
        {
            ClearPreview();
        }
    }
}