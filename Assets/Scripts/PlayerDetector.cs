using UnityEngine;

public class PlayerDetector : MonoBehaviour
{
    [SerializeField] Enemy Enemy1 = null;
    [SerializeField] GameObject SpottedSymbol = null;


    private void Awake()
    {
        SpottedSymbol.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && Enemy1.Health>0)
        {
            //Debug.Log("Player has entered");
            Enemy1.targetingOn = true;
            Enemy1.PlayerPos = other.transform.position;
            if (Enemy1.rb == null)
                Enemy1.rb = other.GetComponent<Rigidbody>();
            SpottedSymbol.SetActive(true);
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
            SpottedSymbol.SetActive(false);


        }
    }














}
