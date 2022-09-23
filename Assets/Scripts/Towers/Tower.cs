using System;
using System.Collections;
using System.Collections.Generic;
using Towers.Executor;
using Units;
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

      [Inject]
      private void Construct(Player player, TowerManager towerManager)
      {
         _player = player;
         _towerManager = towerManager;
      }

      private void Init()
      {
         _towerId = _towerManager.Register(this);
         _params = _towerManager.GetParams(_towerType, _grade);
         _executor = ExecutorFabric.GetExecutor(_towerType);
         ChangeView();
      }

      private void Start()
      {
         Init();
         StartCoroutine(ExecutingCoroutine());
      }

      //TODO check if it possible to refactor, to avoid OnTrigger and do it by position analysis
      private void OnTriggerEnter(Collider other)
      {
         var enemy = other.GetComponent<AbstractEnemy>();
         if (enemy != null)
         {
            _executor.AddEnemy(enemy);
            enemy.OnEnemyDied += OnEnemyDied;
         }
      }

      private void OnTriggerExit(Collider other)
      {
         var enemy = other.GetComponent<AbstractEnemy>();
         if (enemy != null)
         {
            enemy.OnEnemyDied -= OnEnemyDied;
            _executor.RemoveEnemy(enemy);
         }
      }

      private void OnEnemyDied(AbstractEnemy enemy)
      {
         _executor.RemoveEnemy(enemy);
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
         _executor = ExecutorFabric.GetExecutor(_towerType);
      }

      public void ChangeView()
      {
         //TODO - assert logic
         if (_prefabRoot == null)
         {
            Debug.LogError("NO PREFAB ROOT DETECTED");
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