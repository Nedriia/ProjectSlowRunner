using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBehavior : MonoBehaviour
{
    public Customer ActualCustomer { get; set; }
    public List<Customer> TinderCustomer { get; set; }


   // public GameObject Map;
 
    public float Money;
    //private PlayerController _playerController { get; set; }


    // Start is called before the first frame update
    void Start()
    {
        //_playerController = GetComponent<PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {
        //  ActualCustomer.DetectSpot(_playerController.currentCube.GetComponent<InspectElement>().Event);
        // (ActualCustomer.DetectSpot(Map.GetTile().Spot))




    }


}
