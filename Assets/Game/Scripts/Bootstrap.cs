using Game.Cursor;
using Game.Input;
using Game.Physics;
using UnityEngine;
using Utils.ServiceLocator;

public class Bootstrap : MonoBehaviour
{
    [SerializeField] private CursorSystem _cursorSystem;

    private void Awake()
    {
        ValidateDependencies();
        RegisterServices();
        InitializeSystems();
    }
    private void ValidateDependencies()
    {
        if (_cursorSystem == null)
        {
            Debug.LogError("Bootstrap: Missing system dependencies!");
        }
    }

    private void RegisterServices()
    {
        ServiceLocator.Initialize();

        ServiceLocator.Current.Register<IInput>(new MobileInput());
        ServiceLocator.Current.Register(new DepthCalculatorService());
        ServiceLocator.Current.Register<IPhysics>(new ObjectDropper());
    }

    private void InitializeSystems()
    {
        _cursorSystem.Initialize();
    }
}
