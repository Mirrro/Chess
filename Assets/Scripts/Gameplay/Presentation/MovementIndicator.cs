using System;
using System.Collections.Generic;
using Gameplay.MoveGeneration.Utility;
using UnityEngine;

public class MovementIndicator
{
    private Dictionary<TileType, Mesh> meshLookup = new ();

    public MovementIndicator(List<DefinitionMeshPair> meshPairs)
    {
        foreach (var definitionMeshPair in meshPairs)
        {
            meshLookup.Add(definitionMeshPair.Definition, definitionMeshPair.Mesh);
        }
    }

    public Mesh GenerateMoveMesh(bool[,] movingData)
    {
        List<CombineInstance> combineInstances = new List<CombineInstance>();
        
        for (int y = 0; y < movingData.GetLength(0); y++)
        {
            for (int x = 0; x < movingData.GetLength(1); x++)
            {
                foreach (var combineInstance in CreateTile(movingData, new Vector2Int(x, y)))
                {
                    combineInstances.Add(combineInstance);
                }
            }
        } 
        
        Mesh result = new Mesh();
        result.CombineMeshes(combineInstances.ToArray());
        return result;
    }

    private List<CombineInstance> CreateTile(bool[,] data, Vector2Int position)
    {
        List<CombineInstance> corners = new List<CombineInstance>();

        if (!data[position.y, position.x])
        {
            return corners;
        }

        for (int i = 0; i < 4; i++)
        {
            (TileType type, Vector3 pos, float rot) tileDef;
            Side side = i switch
            {
                1 => Side.TopRight,
                2 => Side.BottomLeft,
                3 => Side.BottomRight,
                _ => Side.TopLeft
            };
            
            tileDef = side switch
            {
                Side.TopLeft => Define(data, position, 
                    offset: new Vector2(-.25f, .25f), 
                    horizonal: position.Left(),
                    vertical: position.Up(), 
                    diagonal: position.Up().Left(), 
                    rotation: 270f,
                    wallRotHorizontal: GetHorizontalWallRot(Side.TopLeft),
                    wallRotVertical: GetVerticalWallRot(Side.TopLeft)),
                Side.TopRight => Define(data, position, 
                    offset: new Vector2(.25f, .25f), 
                    horizonal: position.Right(),
                    vertical: position.Up(), 
                    diagonal: position.Up().Right(), rotation: 0f,
                    wallRotHorizontal: GetHorizontalWallRot(Side.TopRight),
                    wallRotVertical: GetVerticalWallRot(Side.TopRight)),
                Side.BottomLeft => Define(data, position, 
                    offset: new Vector2(-.25f, -.25f),
                    horizonal: position.Left(),
                    vertical: position.Down(), 
                    diagonal: position.Down().Left(), 
                    rotation: 180f,
                    wallRotHorizontal: GetHorizontalWallRot(Side.BottomLeft),
                    wallRotVertical: GetVerticalWallRot(Side.BottomLeft)),
                Side.BottomRight => Define(data, position, 
                    offset: new Vector2(.25f, -.25f),
                    horizonal: position.Right(), 
                    vertical: position.Down(), 
                    diagonal: position.Down().Right(),
                    rotation: 90f,
                    wallRotHorizontal: GetHorizontalWallRot(Side.BottomRight),
                    wallRotVertical: GetVerticalWallRot(Side.BottomRight)),
                _ => throw new ArgumentOutOfRangeException(nameof(side), side, null)
            };

            if (meshLookup.TryGetValue(tileDef.type, out Mesh mesh))
            {
                corners.Add(new CombineInstance()
                {
                    mesh = mesh,
                    transform = Matrix4x4.TRS(pos: tileDef.pos,
                        q: Quaternion.Euler(Vector3.up * tileDef.rot),
                        s: Vector3.one),
                });
            }
        }

        return corners;
    }

    private (TileType type, Vector3 pos, float rot) Define(bool [,] data, Vector2Int gridPosition, Vector2 offset, Vector2Int horizonal, Vector2Int vertical, Vector2Int diagonal, float rotation, float wallRotHorizontal, float wallRotVertical)
    {
        Vector3 position = new Vector3(gridPosition.x + offset.x, 0, gridPosition.y + offset.y);
        TileType type;

        bool horizontalNeighbour = GetDefinition(data, horizonal);
        bool verticalNeighbour = GetDefinition(data, vertical);
        
        if (!horizontalNeighbour && !verticalNeighbour)
        {
            type = TileType.Corner;
        }else if (!horizontalNeighbour)
        {
            type = TileType.Wall;
            rotation += wallRotVertical;
        }else if (!verticalNeighbour)
        {
            type = TileType.Wall;
            rotation += wallRotHorizontal;
        }
        else
        {
            bool diagonalNeighbour = GetDefinition(data, diagonal);
            type = diagonalNeighbour ? TileType.Floor : TileType.FloorWithConnector;
        }
        
        return (type, position, rotation);
    }
    
    private float GetHorizontalWallRot(Side side)
    {
        switch (side)
        {
            case Side.TopLeft:
                return 90;
            case Side.TopRight:
                return 0;
            case Side.BottomLeft:
                return 0;
            case Side.BottomRight:
                return 90;
            default:
                throw new ArgumentOutOfRangeException(nameof(side), side, null);
        }
    }

    private float GetVerticalWallRot(Side side)
    {
        switch (side)
        {
            case Side.TopLeft:
                return 0;
            case Side.TopRight:
                return 90;
            case Side.BottomLeft:
                return 90;
            case Side.BottomRight:
                return 0;
            default:
                throw new ArgumentOutOfRangeException(nameof(side), side, null);
        }
    }

    private bool GetDefinition(bool[,] data, Vector2Int position)
    {
        return IsInBounds(data, position) && data[position.y, position.x];
    }
    
    bool IsInBounds(bool[,] data, Vector2Int position)
    {
        return position.x >= 0 && position.x < data.GetLength(0) && position.y >= 0 && position.y < data.GetLength(1);
    }
    
    private enum Side
    {
        TopLeft,
        TopRight,
        BottomLeft,
        BottomRight
    }
    
    [Serializable]
    public enum TileType
    {
        Corner,
        Wall,
        FloorWithConnector,
        Floor
    }

    [Serializable]
    public class DefinitionMeshPair
    {
        public TileType Definition;
        public Mesh Mesh;
    }
}