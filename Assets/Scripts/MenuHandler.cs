using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuHandler : MonoBehaviour
{
    private void Start()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }
    public void ExitGame()
    {
        //quits application
        Application.Quit();
        //or if running in editor then stop play mode
#if UNITY_EDITOR
        Debug.Log("ExitGame attempted.");
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}
