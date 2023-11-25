using System;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;

public class BoardTileView : MonoBehaviour
{
    public event Action Clicked;

    private Renderer renderer;
    private Color color;
    private void Awake()
    {
        renderer = GetComponent<Renderer>();
    }

    public async UniTask Highlight(Color color)
    {
        await renderer.material.DOColor(color, .2f).AsyncWaitForCompletion();
    }
    
    public async UniTask Unhighlight()
    {
        await renderer.material.DOColor(color, .2f).AsyncWaitForCompletion();
    }

    public void SetColor(Color color)
    {
        this.color = color;
        renderer.material.color = color;
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                if (hit.collider.gameObject == gameObject)
                {
                    Clicked?.Invoke();
                }
            }
        }
    }
}
