using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceFreeze : MonoBehaviour
{
    [SerializeField] GameObject visualsToDeactivate = null;

    Collider colliderToDeactivate = null;
    float timerDecay;

    private void Awake()
    {
        colliderToDeactivate = GetComponent<Collider>();

    }

    private void OnTriggerEnter(Collider other)
    {
        Enemy Enemy1
            = other.gameObject.GetComponent<Enemy>();
        
            if (Enemy1 != null && other.transform.tag=="Enemy")
        {
            //Debug.Log("Hitting " + other.transform.name);
            Enemy1.FreezeAdd();
            Destroy(this.gameObject);
            
        }



    }

    private void Update()
    {
     /*   timerDecay += Time.deltaTime;
        if (timerDecay > .7f)
            Destroy(this.gameObject);*/
    }

    public void DisableObject()
    {
        colliderToDeactivate.enabled = false;
        visualsToDeactivate.SetActive(false);
    }

}
