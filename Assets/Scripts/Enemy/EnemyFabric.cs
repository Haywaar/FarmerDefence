using System.Collections.Generic;
using Units;
using Zenject;

public class EnemyFabric
{
   Dictionary<EnemyType, EnemyPool> _enemyPools = new Dictionary<EnemyType, EnemyPool>();

   private EnemyData _enemyData;
   private DiContainer _container;
   
   public EnemyFabric(EnemyData enemyData, DiContainer container)
   {
       _enemyData = enemyData;
       _container = container;
   }

   public AbstractEnemy CreateEnemy(int grade, EnemyType enemyType)
   {
       EnemyPool pool;
       var enemyParams = _enemyData.GetParams(grade, enemyType);

       if (!_enemyPools.TryGetValue(enemyType, out pool))
       {
           pool = new EnemyPool(_container, enemyParams.Prefab);
           _enemyPools.Add(enemyType, pool);
       }

       var enemy = pool.Get();
       enemy.Init(enemyParams, enemyType);
       return enemy;
   }

   //TODO - это не фабрика уже получается
   public void DisposeEnemy(AbstractEnemy enemy)
   {
       var enemyType = enemy.EnemyType;
       var pool = _enemyPools.GetValueOrDefault(enemyType);
       pool.Release(enemy);
   }
}
