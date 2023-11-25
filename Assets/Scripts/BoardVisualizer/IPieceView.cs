using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;

public interface IPieceView
{
    public UniTask Move(Vector2Int position);
    public UniTask Show();
    public UniTask Hide();

    public void SetColor(bool isWhite);
}
