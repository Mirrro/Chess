using System.Collections.Generic;
using UnityEngine;

public class HighlightView : MonoBehaviour
{
    [SerializeField] private MeshFilter meshFilter;
    [SerializeField] private List<MovementIndicator.DefinitionMeshPair> meshPairs;
    
    private MovementIndicator movementIndicator;
    private bool[,] highlighted = new bool[8, 8]; 

    private void Awake()
    {
        movementIndicator = new MovementIndicator(meshPairs);
    }

    public void HighlightTile(Vector2 position)
    {
        highlighted[(int)position.y, (int)position.x] = true;
        meshFilter.mesh = movementIndicator.GenerateMoveMesh(highlighted);
    }

    public void ClearHighlights()
    {
        highlighted = new bool[8, 8];
        meshFilter.mesh = null;
    }
}
