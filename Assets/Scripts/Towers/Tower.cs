using System;
using System.Collections;
using System.Collections.Generic;
using Units;
using UnityEngine;

namespace Towers
{
   public class Tower : MonoBehaviour
   {
      [SerializeField] private TowerType _towerType;
      [SerializeField] private int _grade = 1;
      [SerializeField] private Transform _prefabRoot;
      public int Grade => _grade;

      protected TowerParams _params;
      public TowerType TowerType => _towerType;

      public TowerParams Params => _params;

      protected List<AbstractEnemy> _targetList = new List<AbstractEnemy>();
      private int _towerId;

      public int TowerId => _towerId;

      private bool isSelected;

      public bool IsSelected => isSelected;

      public void Init()
      {
         _towerId = TowerManager.Instance.Register(this);
         _params = TowerManager.Instance.GetParams(_towerType, _grade);
         //Get executor
         ChangeView();
      }

      protected void Execute()
      {
         //TODO
         // shoot
         // do nothing
         // slow area
         
      }
      private void Start()
      {
         Init();
         StartCoroutine(ExecutingCoroutine());
      }

      private void OnTriggerEnter(Collider other)
      {
         var enemy = other.GetComponent<AbstractEnemy>();
         if (enemy != null)
         {
            _targetList.Add(enemy);
            //TODO подумать, может отказаться от триггеров и пробегать по зонам и расположениям башен и определять списки там
            enemy.OnEnemyDied += OnEnemyDied;
         }
      }

      private void OnEnemyDied(AbstractEnemy enemy)
      {
         if (_targetList.Contains(enemy))
         {
            _targetList.Remove(enemy);
         }
      }

      private void OnTriggerExit(Collider other)
      {
         var enemy = other.GetComponent<AbstractEnemy>();
         if (enemy != null)
         {
            enemy.OnEnemyDied -= OnEnemyDied;
            _targetList.Remove(enemy);
         }
      }

      private IEnumerator ExecutingCoroutine()
      {
         while (Player.Instance.IsAlive())
         {
            yield return new WaitForSeconds(_params.AttackCooldown);
            //TODO не нравится, надо бы общий менеджер написать

            if (_targetList.Count > 0)
            {
               Execute();
            }
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
         _towerType = TowerType.Empty;
         _params = TowerManager.Instance.GetParams(TowerType.Empty, 1);
         _grade = 1;
         ChangeView();
         SetSelected(false);
      }

      public void ChangeType(TowerType towerType)
      {
         _towerType = towerType;
      }

      public void ChangeView()
      {
         //TODO - assert?
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