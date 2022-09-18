using Towers;
using UnityEngine;
using Zenject;

public class InputController : MonoBehaviour
{
    [SerializeField] private GameObject _placeHolder;
    [SerializeField] private GameObject _tower;

    private InputState _state = InputState.None;
    private GameObject _placingGameobject;

    private TowerManager _towerManager;

    [Inject]
    private void Construct(TowerManager towerManager)
    {
        _towerManager = towerManager;
    }

    void Update()
    {
        ClickOnHolderLogic();
    }

    private void ClickOnHolderLogic()
    {
        if (Input.GetKeyUp(KeyCode.Mouse0))
        {
            var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                if (hit.transform.tag.Equals("Tower"))
                {
                    var tower = hit.transform.GetComponentInParent<Tower>();
                    if (tower != null && !tower.IsSelected)
                    {
                        _towerManager.TowerClicked(tower.TowerId);
                    }
                }
                else
                {
                    _towerManager.TowerClicked(-1);
                }
            }
            else
            {
                _towerManager.TowerClicked(-1);
            }
        }
    }

    private void PlacingTowerLogic()
    {
        if (Input.GetKeyUp(KeyCode.Mouse0))
        {
            if (_state == InputState.None)
            {
                _state = InputState.PositioningTower;
                //TODO - нужен пул и для башен и для снарядов
                _placingGameobject = GameObject.Instantiate(_placeHolder);
                return;
            }
            
            if (_state == InputState.PositioningTower)
            {
                //Place tower
                Instantiate(_tower, _placingGameobject.transform.position, Quaternion.identity);
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
}

public enum InputState
{
    None,
    PositioningTower,
}
