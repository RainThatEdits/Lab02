using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathManager : MonoBehaviour
{
    [HideInInspector] [SerializeField] public List<waypoint> path;

    public List<waypoint> GetPath()
    {
        if (path == null)
            path = new List<waypoint>();

        return path;
    }

    public void CreateAddPoint()
    {
        waypoint go = new waypoint();
        path.Add(go);

    }
}
