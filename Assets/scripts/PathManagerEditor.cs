using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEditor;
using UnityEditor.TerrainTools;

[CustomEditor(typeof(PathManager))]
public class PathManagerEditor : Editor
{
    [SerializeField] private PathManager pathManager;
    [SerializeField] private List<waypoint> ThePath;
    private List<int> toDelete;

    private waypoint selectedPoint = null;
    private bool doRepaint = true;


    private void OnSceneGUI()
    {
        ThePath = pathManager.GetPath();
        DrawPath(ThePath);
    }

    private void OnEnable()
    {
        pathManager = target as PathManager;
        toDelete = new List<int>();
    }


    public override void OnInspectorGUI()
    {
        this.serializedObject.Update();
        ThePath = pathManager.GetPath();
        
        base.OnInspectorGUI();
        EditorGUILayout.BeginVertical();
        EditorGUILayout.LabelField("Path");

        DrawGUIForPoints();

        if (GUILayout.Button("Add point to path"))
        {
            pathManager.CreateAddPoint();
        }

        EditorGUILayout.EndVertical();
        SceneView.RepaintAll();

        void DrawGUIForPoints()
        {
            if (ThePath != null && ThePath.Count > 0)
            {
                for (int i = 0; i < ThePath.Count; i++)
                {
                    EditorGUILayout.BeginHorizontal();
                    waypoint p = ThePath[i];

                    Vector3 oldPos = p.GetPos();
                    Vector3 newPos = EditorGUILayout.Vector3Field("", oldPos);
                    
                    if (EditorGUI.EndChangeCheck()) p.SetPos(newPos);

                    if (GUILayout.Button("-", GUILayout.Width(25)))
                    {
                        toDelete.Add(i);
                    }
                    EditorGUILayout.EndHorizontal();
                }
            }

            if (toDelete.Count > 0)
            {
                foreach (int i in toDelete)
                    ThePath.RemoveAt(i);
                toDelete.Clear();
            }
            
        }

    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void DrawPath(List<waypoint> path)
    {
        if (path != null)
        {
            int current = 0;
            foreach (waypoint wp in path)
            {
                //draw current
                doRepaint = DrawPoint()(wp);
                int next = (current + 1) % path.Count;
                waypoint wpnext = path[next];
                //connet this oen to tnext 
                DrawPathLine(wp, wpnext);
                current += 1;
            }

            if (doRepaint) Repaint();
        }
    }

    public void DrawPathLine(waypoint p1, waypoint p2)
    {
       
    }

    public bool Drawpoint(waypoint p)
    {
        bool isChanged = false;
        if (selectedPoint == p)
        {
            Color c = Handles.color; 
            Handles.color = Color.green; 
            
            EditorGUI.BeginChangeCheck();
            Vector3 oldpos = p.GetPos();
            Vector3 newpos = Handles.PositionHandle(oldpos, Quaternion.identity);

            float handleSize = HandleUtility.GetHandleSize(newpos);
            Handles.SphereHandleCap(-1, newpos, Quaternion.identity, 0.4f * handleSize, EventType.Repaint);

            if (EditorGUI.EndChangeCheck())
            {
                p.SetPos(newpos);
            }
            
            Handles.color = c;
        }
        else
        {
            Vector3 currPos = p.GetPos();
            float handleSize = HandleUtility.GetHandleSize(currPos);
            if (Handles.Button(currPos, Quaternion.identity, 0.25f * handleSize, 0.25f * handleSize,
                    Handles.SphereHandleCap))
            {
                isChanged = true;
                selectedPoint = p;
            }
        }

        return isChanged;

    }
    
}
