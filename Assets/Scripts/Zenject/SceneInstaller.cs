using UnityEngine;
using Zenject;

public class SceneInstaller : MonoInstaller
{
    [SerializeField] private EnemyManager _enemyManager;
    [SerializeField] private Player _player;
    public override void InstallBindings()
    {
        Container.Bind<EnemyManager>().FromInstance(_enemyManager).AsSingle();
        Container.Bind<Player>().FromInstance(_player).AsSingle();
    }
}