using System.Collections;
using System.Collections.Generic;
using Towers;
using UnityEngine;

public class InputController : MonoBehaviour
{

    [SerializeField] private GameObject _placeHolder;
    [SerializeField] private GameObject _tower;

    private InputState _state = InputState.None;
    private GameObject _placingGameobject;
    void Update()
    {
        ClickOnHolderLogic();
    }
    
    private void PlacingTowerLogic()
    {
        if (Input.GetKeyUp(KeyCode.Mouse0))
        {
            if (_state == InputState.None)
            {
                Debug.LogWarning("Starting to place tower!");
                _state = InputState.PositioningTower;
                //TODO - нужен пул и для башен и для снарядов
                _placingGameobject = GameObject.Instantiate(_placeHolder);
                return;
            }
            
            if (_state == InputState.PositioningTower)
            {
                Debug.LogWarning("placed!");
                //Place tower
                GameObject.Instantiate(_tower, _placingGameobject.transform.position, Quaternion.identity);
                Destroy(_placingGameobject);
                _placingGameobject = null;
                _state = InputState.None;
            }
        }

        if (_placingGameobject != null)
        {
            var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                Debug.LogWarning("pos " + hit.point);
                //TODO видимо прокинуть ссылку на плейн
                //_placingGameobject.transform.position = new Vector3(hit.point.x, 1, hit.point.z);
                _placingGameobject.transform.position = hit.point;
            }

        }
    }

    private void ClickOnHolderLogic()
    {
        // При клике - стреляем рейкастом и смотрим куда попали
        if (Input.GetKeyUp(KeyCode.Mouse0))
        {
            // Если попали в холдер, посылаем команду на MainScreen?
            var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                if (hit.transform.tag.Equals("Tower"))
                {
                    Debug.LogWarning("1");
                    // UI создать на MousePosition
                    var tower = hit.transform.GetComponentInParent<Tower>();
                    if (tower != null && !tower.IsSelected)
                    {
                        Debug.LogWarning("1");
                        TowerManager.Instance.TowerClicked(tower.TowerId);
                    }
                }
            }

        }

        // На MainScreen появляется кнопка с типом построения башни
        // При клике на кнопку в указанном холдере строится башня
    }
}




public enum InputState
{
    None,
    PositioningTower,
}
