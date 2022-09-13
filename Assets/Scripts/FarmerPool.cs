using System.Collections;
using System.Collections.Generic;
using Units;
using UnityEngine;

public class FarmerPool<T> where T : MonoBehaviour
{
    private List<T> _activeElements = new List<T>();
    private List<T> _inactiveElements = new List<T>();

    public T Get()
    {
        T element;
        if (_inactiveElements.Count == 0)
        {
            element = GenerateNew();
            return element;
        }
        else
        {
            element = _inactiveElements[0];
            element.gameObject.SetActive(true);
            
            _inactiveElements.Remove(element);
            _activeElements.Add(element);
            return element;
        }
    }

    public void Release(T element)
    {
        element.gameObject.SetActive(false);
        
        _activeElements.Remove(element);
        _inactiveElements.Add(element);
    }

    private T GenerateNew()
    {
    //    T element;
   //     _activeElements.Add(element);
        return null;
    }
}