using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Rendering.PostProcessing;

[System.Serializable]
public class MyGOEvent : UnityEvent<GameObject>
{
}

public class GameManager : MonoBehaviour
{
    public int pickUpCount = 10;
    public MyGOEvent pickUpEvent;
    public Camera mainCamera;

    private void Start()
    {
        mainCamera = GameObject.FindWithTag("MainCamera").GetComponent<Camera>();
        pickUpEvent = new MyGOEvent();
        pickUpEvent.AddListener(OnPickingUp);
    }

    private void OnPickingUp(GameObject arg0)
    {
        Mock(pickUpCount);
        pickUpCount++;
        Debug.Log("picked Up " + arg0);
    }

    public void Mock(int i)
    {
        
    }
}
