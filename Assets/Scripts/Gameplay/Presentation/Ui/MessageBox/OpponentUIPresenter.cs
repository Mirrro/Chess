using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace Gameplay.Presentation.UI
{
    public class OpponentUIPresenter : ITickable
    {
        private readonly OpponentUIView view;
        private readonly OpponentUIModel model;
        
        private readonly MessageBoxPresenterPool messageBoxPresenterPool;

        public OpponentUIPresenter(OpponentUIView view, OpponentUIModel model, MessageBoxPresenterPool messageBoxPresenterPool)
        {
            this.view = view;
            this.model = model;
            this.messageBoxPresenterPool = messageBoxPresenterPool;
        }

        public void SetOpponentPicture(Sprite sprite)
        {
            model.Sprite = sprite;
            view.SetPicture(model.Sprite);
        }

        public void DisplayMessage(string message)
        {
            var messageBoxPresenter = messageBoxPresenterPool.Get();
            model.Messages.Add(new MessageData()
            {
                Message = message,
                RemainingDuration = 3f,
                Presenter = messageBoxPresenter
            });
            messageBoxPresenter.SetParent(view.MessageParent);
            messageBoxPresenter.ShowMessage(message);
        }

        public void Tick()
        {
            List<MessageData> toRemove = new ();

            foreach (var modelMessage in model.Messages)
            {
                modelMessage.RemainingDuration -= Time.deltaTime;
                if (modelMessage.RemainingDuration <= 0)
                {
                    toRemove.Add(modelMessage);
                }
            }

            foreach (var modelMessage in toRemove)
            {
                model.Messages.Remove(modelMessage);
                modelMessage.Presenter.Hide(() => OnMessageBoxPresenterHide(modelMessage));
            }
        }

        private void OnMessageBoxPresenterHide(MessageData data)
        {
            data.Presenter.SetParent(null);
            messageBoxPresenterPool.Return(data.Presenter);
        }
    }
}