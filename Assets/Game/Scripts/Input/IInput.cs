using System;
using Utils.ServiceLocator;

namespace Game.Input
{
    public interface IInput : IService
    {
        public event Action ClickStarted;
        public event Action ClickCanceled;
    }
}
