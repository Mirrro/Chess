using UnityEngine;

namespace Gameplay.Presentation.Pieces.Behaviours
{
    /// <summary>
    /// Visual component for showing or hiding a capture indicator during move previews.
    /// </summary>
    public class PieceCapturePreview : MonoBehaviour
    {
        [SerializeField] private GameObject captureIndicator;

        public void PreviewCapture()
        {
            captureIndicator.SetActive(true);
        }

        public void ClearPreview()
        {
            captureIndicator.SetActive(false);
        }

        private void Awake()
        {
            ClearPreview();
        }
    }
}