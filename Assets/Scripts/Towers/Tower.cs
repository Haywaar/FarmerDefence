using System.Collections;
using Towers.Executor;
using Towers.Meta;
using UnityEngine;
using Zenject;

namespace Towers
{
   public class Tower : MonoBehaviour
   {
      [SerializeField] private TowerType _towerType;
      [SerializeField] private int _grade = 1;
      [SerializeField] private Transform _prefabRoot;

      private TowerParams _params;
      public int Grade => _grade;
      public TowerType TowerType => _towerType;
      public TowerParams Params => _params;

      private Executor.Executor _executor;

      private int _towerId;
      public int TowerId => _towerId;

      private bool isSelected;
      public bool IsSelected => isSelected;

      private Player _player;
      private TowerManager _towerManager;
      private SignalBus _signalBus;

      [Inject]
      private void Construct(Player player, TowerManager towerManager, SignalBus signalBus)
      {
         _player = player;
         _towerManager = towerManager;
         _signalBus = signalBus;
      }

      private void Init()
      {
         _towerId = _towerManager.Register(this);
         _params = _towerManager.GetParams(_towerType, _grade);
         _executor = ExecutorFabric.GetExecutor(_towerId, _towerType, _signalBus);

         ChangeView();
      }

      private void Start()
      {
         Init();
         StartCoroutine(ExecutingCoroutine());
      }

      private IEnumerator ExecutingCoroutine()
      {
         while (_player.IsAlive())
         {
            yield return new WaitForSeconds(_params.AttackCooldown);
            //TODO every tower uses execute. Maybe it is possible to move it to 1 manager?
            _executor.Execute(_params, transform.position);
         }
      }

      public void Upgrade(TowerParams newParams)
      {
         _params = newParams;
         _grade = _params.Grade;
         ChangeView();
         SetSelected(false);
      }

      public void Salvage()
      {
         ChangeType(TowerType.Empty);
         _params = _towerManager.GetParams(TowerType.Empty, 1);
         _grade = 1;
         ChangeView();
         SetSelected(false);
      }

      public void ChangeType(TowerType towerType)
      {
         _towerType = towerType;
         _executor = ExecutorFabric.GetExecutor(_towerId, _towerType, _signalBus);
      }

      public void ChangeView()
      {
         //TODO - assert logic
         if (_prefabRoot == null)
         {
            Debug.LogError("No prefab root detected");
            return;
         }

         while (_prefabRoot.childCount > 0) {
            DestroyImmediate(_prefabRoot.GetChild(0).gameObject);
         }

         if (_params.Prefab != null)
         {
            var prefab = Instantiate(_params.Prefab, _prefabRoot.position, Quaternion.identity);
            prefab.transform.parent = _prefabRoot.transform;
         }
         else
         {
            Debug.LogError("params prefab is null");
         }
      }

      public void SetSelected(bool selected)
      {
         //TODO - activate some visual effect if selected
         isSelected = selected;
      }
   }
}