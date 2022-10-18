using Zenject;

namespace Towers.Executor
{
   public static class ExecutorFabric
   {
      public static Executor GetExecutor(int towerId, TowerType type, SignalBus signalBus)
      {
         switch (type)
         {
            case TowerType.Empty:
               return new EmptyExecutor(towerId, signalBus);
            case TowerType.FastTower:
            case TowerType.HeavyTower:
               return new ShootingExecutor(towerId, signalBus);
            default:
               return new EmptyExecutor(towerId, signalBus);
         }
      }
   }
}
