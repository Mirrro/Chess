using System.Collections.Generic;
using UnityEngine;

namespace Gameplay.Presentation.UI
{
    public class MessageBoxPresenterPool
    {
        private readonly MessageBoxPresenter.Factory messageBoxPresenterFactory;
        private readonly GameViewContainer gameViewContainer;
        
        private List<MessageBoxPresenter> freePresenters = new ();
        
        public MessageBoxPresenterPool(MessageBoxPresenter.Factory messageBoxPresenterFactory, GameViewContainer gameViewContainer)
        {
            this.messageBoxPresenterFactory = messageBoxPresenterFactory;
            this.gameViewContainer = gameViewContainer;
        }

        public MessageBoxPresenter Get()
        {
            if (freePresenters.Count == 0)
            {
                freePresenters.Add(messageBoxPresenterFactory.Create(Object.Instantiate(gameViewContainer.MessageBoxPrefab), new MessageBoxModel()));
            }
            
            var presenter = freePresenters[0];
            freePresenters.Remove(presenter);
            return presenter;
        }

        public void Return(MessageBoxPresenter presenter)
        {
            presenter.Clear();
            freePresenters.Add(presenter);
        }
    }
}