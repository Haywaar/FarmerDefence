using UnityEngine;

namespace Towers.Executor
{
   public class ExecutorFabric : MonoBehaviour
   {
      public static Executor GetExecutor(TowerType type)
      {
         switch (type)
         {
            case TowerType.Empty:
               return new EmptyExecutor();
            case TowerType.FastTower:
            case TowerType.HeavyTower:
               return new ShootingExecutor();
            default:
               return new EmptyExecutor();
         }
      }
   }
}
