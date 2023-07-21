/*using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
[CustomEditor(typeof(Remote))]
public class RemoteCustomEditor : Editor
{
#if UNITY_EDITOR
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        Remote remote = (Remote) target;
        if (GUILayout.Button("Toggle TV"))
        {
            remote.TV(remote.tvON.activeSelf);
        }
    }
#endif
}
*/