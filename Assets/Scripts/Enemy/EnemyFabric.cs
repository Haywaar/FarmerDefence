using System.Collections;
using System.Collections.Generic;
using Units;
using UnityEngine;
using UnityEngine.Pool;

public class EnemyFabric : MonoBehaviour
{
   Dictionary<EnemyType, ObjectPool<AbstractEnemy>> _objectPools;
   
   public void CreateEnemy(AbstractEnemy enemy)
   {
      
   }
}
