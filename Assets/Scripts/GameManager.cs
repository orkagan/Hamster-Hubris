using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class MyGOEvent : UnityEvent<GameObject>
{
}
public class MyBoolEvent : UnityEvent<bool>
{
}

public class GameManager : MonoBehaviour
{

    #region References

    public int pickUpCount;
    public MyGOEvent pickUpEvent;
    public UnityEvent placeDownEvent;
    public MyBoolEvent death;
    
    public Camera mainCamera;
    public Shader placedShader, ghostShader;

    public GameObject hampterGO;
    public GameObject explodeParticleEffect;
    public GameObject player;
    public GameObject bloodEffect;

    public Transform startPosition;

    public List<GameObject> bloodEffectsPlaced;

    public List<GameObject> currentItemsPickedUp;
    public List<GameObject> pickUpSlots;
    public List<GameObject> itemsOnBack;
    
    #endregion
    

    private void Start()
    {
        hampterGO.SetActive(false);
        
        pickUpCount = 0;
        mainCamera = GameObject.FindWithTag("MainCamera").GetComponent<Camera>();
        pickUpEvent = new MyGOEvent();
        pickUpEvent.AddListener(OnPickingUp);

        placeDownEvent = new UnityEvent();
        placeDownEvent.AddListener(Placed);

        death = new MyBoolEvent();
        death.AddListener(OnDeath);
    }

    private void OnPickingUp(GameObject arg0)
    {
        pickUpCount++;
        //var of what the enum is equal to
        var pickUp = arg0.GetComponent<PickUp>();
        
        //var i = Instantiate(particleEffects, pickUp.shrineEquivalent.transform);


        //add the object to the list of items being held
        currentItemsPickedUp.Add(arg0);
        var copy = Instantiate(arg0, pickUpSlots[pickUpCount - 1].transform);
        copy.transform.localPosition = new Vector3(0,0,0);
        copy.transform.localScale = new Vector3(0.05f, 0.05f, 0.05f);
        itemsOnBack.Add(copy);
        //Debug
        Debug.Log("picked Up " + pickUp);
        //Set the Shrine object active without the glow just to show that the item CAN be placed
    }

    private void Placed()
    {
        if (currentItemsPickedUp.Count == 0) return;
        foreach (var item in currentItemsPickedUp)
        {
            item.GetComponent<PickUp>().shrineEquivalent.GetComponent<MeshRenderer>().materials[0].shader = placedShader;
            item.GetComponent<PickUp>().shrinePreview.GetComponent<MeshRenderer>().materials[0].shader = placedShader;
            
            //item.GetComponent<PickUp>().shrineEquivalent.GetComponentInChildren<ParticleSystem>().gameObject.SetActive(false);

            Debug.Log("Placed " +  item);
            //currentItemsPickedUp.Remove(item);
        }

        currentItemsPickedUp.Clear();
        
        foreach (var item in itemsOnBack)
        {
            Destroy(item);
        }
        
        itemsOnBack.Clear();

        if (pickUpCount == 4)
        {
            EndGame(false);
        }
    }

    public void EndGame(bool sacrafice)
    {
        if (!sacrafice)
        {
            hampterGO.GetComponent<MeshRenderer>().materials[0].shader = ghostShader;
            hampterGO.SetActive(true);
            hampterGO.GetComponentInChildren<ParticleSystem>().gameObject.SetActive(true);
            pickUpCount++;
        }
        else
        {
            OnDeath(true);
            Debug.Log("End Game??");
        }
    }

    private void OnDeath(bool finalDeath)
    {
        //Explode Particles
        var exp = Instantiate(explodeParticleEffect);
        exp.transform.position = player.transform.position;

        RaycastHit hit;
        if (Physics.Raycast(player.transform.position, Vector3.down, out hit, 10))
        {
            var blood = Instantiate(bloodEffect);
            blood.transform.position = player.transform.position;
            //blood.transform.rotation = player.transform.rotation;

            bloodEffectsPlaced.Add(blood);
        }
        
        
        player.SetActive(false);
        
        //WaitSeconds
        StartCoroutine(Death(2f, exp));
        if (finalDeath)
        {
            //ENDGAME
        }
        else
        {
            //Respawn

        }
    }

    public IEnumerator Death(float seconds, GameObject particleEffect)
    {
        var pos = player.transform.position;
        player.SetActive(false);
        player.transform.position = pos;
        yield return new WaitForSeconds(seconds);
        Destroy(particleEffect);
        player.SetActive(true);
        player.transform.position = startPosition.position;
    }
}
