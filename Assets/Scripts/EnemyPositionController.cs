using System.Collections.Generic;
using Enemy;
using Towers;
using Units;
using UnityEngine;
using Zenject;
using Zenject.Signals;

public class EnemyPositionController : MonoBehaviour
{
    private EnemyManager _enemyManager;
    private TowerManager _towerManager;
    private EnemyPositionGrid _positionGrid;
    private SignalBus _signalBus;
    
    [Inject]
    private void Construct(EnemyManager enemyManager, TowerManager towerManager, SignalBus signalBus)
    {
        _enemyManager = enemyManager;
        _towerManager = towerManager;
        _signalBus = signalBus;

        _positionGrid = new EnemyPositionGrid();
        
        _signalBus.Subscribe<EnemyKilledSignal>(OnEnemyDied);
    }

    private void MoveEnemy(AbstractEnemy enemy)
    {
        var newPos = enemy.GetNextStepPosition();
        _positionGrid.OnEnemyMoved(enemy, newPos);
        enemy.Move(newPos);
    }

    private void OnEnemyDied(EnemyKilledSignal signal)
    {
        _positionGrid.OnEnemyMoved(signal.Enemy, _enemyManager.GetSpawnPointPosition());
    }

    private void Update()
    {
        foreach (var enemy in _enemyManager.Enemies)
        {
            if (enemy.CanMove())
            {
               MoveEnemy(enemy);
            }
            _positionGrid.DebugValidateAlgorithm(enemy.ID);
        }
        
        foreach (var tower in _towerManager.Towers)
        {
            var enemies = _positionGrid.GetEnemiesInRadius(tower.transform.position, tower.Params.Radius);
            enemies.Sort(new EnemyDistanceComparer());
            _signalBus.Fire(new EnemyInRadiusSignal(tower.TowerId, enemies));
        }
    }

    // Sorting enemies by distance from tower
    // Enemy that closer to leave towerArea will be in priority
    private class EnemyDistanceComparer : IComparer<AbstractEnemy>
    {
        public int Compare(AbstractEnemy x, AbstractEnemy y)
        {
            if (x.GetWaypointId() > y.GetWaypointId())
            {
                return -1;
            }

            if (x.GetWaypointId() < y.GetWaypointId())
            {
                return 1;
            }

            if (x.GetWaypointId() == y.GetWaypointId())
            {
                if (x.GetDistanceToWaypoint() > y.GetDistanceToWaypoint())
                {
                    return 1;
                }

                return -1;
            }

            return 0;
        }
    }
}
