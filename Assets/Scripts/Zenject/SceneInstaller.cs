using UnityEngine;
using Zenject;
using Zenject.Signals;

public class SceneInstaller : MonoInstaller
{
    [SerializeField] private EnemyManager _enemyManager;
    [SerializeField] private Player _player;
    [SerializeField] private WaypointManager _waypointManager;
    public override void InstallBindings()
    {
        Container.Bind<EnemyManager>().FromInstance(_enemyManager).AsSingle();
        Container.Bind<Player>().FromInstance(_player).AsSingle();
        Container.Bind<WaypointManager>().FromInstance(_waypointManager).AsSingle();
        
        SignalBusInstaller.Install(Container);
        Container.DeclareSignal<SpawnEnemySignal>().OptionalSubscriber();
    }
}