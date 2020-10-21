using UnityEngine;

public class PlayerDetector : MonoBehaviour
{
    [SerializeField] Enemy Enemy1 = null;





    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && Enemy1.Health>0)
        {
            //Debug.Log("Player has entered");
            Enemy1.targetingOn = true;
            Enemy1.PlayerPos = other.transform.position;
            if (Enemy1.rb == null)
                Enemy1.rb = other.GetComponent<Rigidbody>();
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player") && Enemy1.Health > 0)
        {
            //Debug.Log("Player is staying");
            Enemy1.targetingOn = true;
            Enemy1.PlayerPos = other.transform.position;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player") && Enemy1.Health > 0)
        {
            //Debug.Log("Player has left");

            Enemy1.targetingOn = false;

            
            
        }
    }














}
