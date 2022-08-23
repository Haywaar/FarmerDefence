using System.Collections.Generic;
using UnityEngine;

public class WaypointManager : MonoBehaviour
{
    public static WaypointManager Instance;

    [SerializeField] private List<Transform> _waypoints;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Debug.LogError("Multiple waypoint manager detected");
            Destroy(this);
        }
    }

    public List<Transform> GetPositions()
    {
        return _waypoints;
    }
}