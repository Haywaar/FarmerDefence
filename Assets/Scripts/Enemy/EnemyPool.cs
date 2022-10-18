using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Units;
using UnityEngine;
using Zenject;

//TODO - make generic
public class EnemyPool 
{
    private List<AbstractEnemy> _objects;
    private DiContainer _container;
    private GameObject _prefab;


    public EnemyPool(DiContainer container,GameObject prefab)
    {
        _container = container;
        _prefab = prefab;
        _objects = new List<AbstractEnemy>();
    }

    public AbstractEnemy Get()
    {
        if (_objects.Any(x => !x.gameObject.activeSelf))
        {
            var enemy = _objects.FirstOrDefault(x => !x.gameObject.activeSelf);
            enemy.gameObject.SetActive(true);
            return enemy;
        }
        else
        {
            return Create();
        }
    }

    public void Release(AbstractEnemy enemy)
    {
        enemy.gameObject.SetActive(false);
    }

    private AbstractEnemy Create()
    {
        var enemy = _container.InstantiatePrefabForComponent<AbstractEnemy>(_prefab, Vector3.zero, Quaternion.identity, null);
        _objects.Add(enemy);
        return enemy;
    }
}
