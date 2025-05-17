using System;
using UnityEngine;
using Zenject;
using Timer = System.Timers.Timer;

namespace Gameplay.Presentation.UI
{
    public class MessageBoxPresenter
    {
        private MessageBoxView view;
        private MessageBoxModel model;

        private Timer timer;
        
        public MessageBoxPresenter(MessageBoxView view, MessageBoxModel model)
        {
            this.view = view;
            this.model = model;
        }
        
        public void Clear()
        {
            model.Message = "";
            view.SetText(model.Message);
        }

        public void ShowMessage(string message)
        {
            view.gameObject.SetActive(true);
            model.Message = message;
            view.SetText(model.Message);
            view.Show(null);
        }

        public void Hide(Action callback)
        {
            view.Hide(() =>
            {
                Debug.Log("Hiding");
                view.gameObject.SetActive(false);
                callback?.Invoke();
            });
        }

        public void SetParent(Transform parent)
        {
            view.transform.SetParent(parent, false);
        }
        
        public class Factory : PlaceholderFactory<MessageBoxView, MessageBoxModel, MessageBoxPresenter>{}
    }
}

