using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndBehaviour : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            NextScene();
        }
    }
    public void NextScene()
    {
        // Get the current scene
        Scene currentScene = SceneManager.GetActiveScene();
        // Load the next scene after the current scene
        SceneManager.LoadScene(currentScene.buildIndex + 1);
    }
}
