using System;

namespace Game.Input
{
    public class MobileInput : IInput
    {
        public event Action ClickStarted;
        public event Action ClickCanceled;

        private InputSystem_Actions _inputActionMap;

        public MobileInput()
        {
            _inputActionMap = new InputSystem_Actions();

            RegisterCallbacks();

            _inputActionMap.Gameplay.Enable();
        }

        private void RegisterCallbacks()
        {
            _inputActionMap.Gameplay.Click.started += ((_) => 
            {
                ClickStarted?.Invoke();
            });

            _inputActionMap.Gameplay.Click.canceled += ((_) =>
            {
                ClickCanceled?.Invoke();
            });
        }
    }
}
