using System.Collections.Generic;
using UnityEngine;

public class WaypointManager : MonoBehaviour
{
    [SerializeField] private List<Transform> _waypoints;

    public List<Transform> GetPositions()
    {
        return _waypoints;
    }
}