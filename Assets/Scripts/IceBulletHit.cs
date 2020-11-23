using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceBulletHit : MonoBehaviour
{

    [SerializeField] GameObject visualsToDeactivate = null;
    [SerializeField] float DisableAtTime = 0f;
    [SerializeField] bool BeDisabled = true;
    [SerializeField] int Damage = 1;
    private float Timer1 = 0f;
    Collider colliderToDeactivate = null;


    private void Awake()
    {
        colliderToDeactivate = GetComponent<Collider>();

    }

    private void OnTriggerEnter(Collider other)
    {
        Enemy Enemy1
            = other.gameObject.GetComponent<Enemy>();

        if (Enemy1 != null)
        {
            Debug.Log("enemy has been hit");
            Enemy1.TakeDamage(Damage);
            if (BeDisabled == true)
                DisableObject();

        }
       

        if (BeDisabled == true)
        {
            Timer1 += Time.deltaTime;
            if (Timer1 > DisableAtTime)
            {
                DisableObject();
            }
        }


    }

    public void DisableObject()
    {
        colliderToDeactivate.enabled = false;
        visualsToDeactivate.SetActive(false);
    }
}
