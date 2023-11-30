using System;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;

public class PieceView : MonoBehaviour, IPieceView
{
    [SerializeField] private AudioSource source;
    [SerializeField] private AudioClip clipMove;
    [SerializeField] private AudioClip clipArrive;
    [SerializeField] private Renderer renderer;

    private void Awake()
    {
        renderer.material.color = new Color(0,0,0,0);
    }

    public async UniTask Move(Vector2Int position)
    {
        source.PlayOneShot(clipMove);
        await transform.DOJump(
            new Vector3(position.x, .3f, position.y), 
            .2f, 
            1, 
            .4f).AsyncWaitForCompletion();
        source.PlayOneShot(clipArrive);
        await transform.DOMove(new Vector3(position.x, 0, position.y), .1f).SetEase(Ease.OutExpo).AsyncWaitForCompletion();
    }

    public async UniTask Show()
    {
        Color currentColor = renderer.material.color;
        await renderer.material.DOColor(
            new Color(currentColor.r, currentColor.g, currentColor.b, 1), 
            .01f).AsyncWaitForCompletion();
    }

    public async UniTask Hide()
    {
        Color currentColor = renderer.material.color;
        await renderer.material.DOColor(
            new Color(currentColor.r, currentColor.g, currentColor.b, 0), 
            .01f).AsyncWaitForCompletion();
    }

    public void SetColor(Color color)
    {
        renderer.material.color = color;
    }
}
