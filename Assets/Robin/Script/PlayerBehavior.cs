using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBehavior : MonoBehaviour
{
    public Customer ActualCustomer { get; set; }
    public List<Customer> TinderCustomer { get; set; }
    public GameObject Map;

    public float Money;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
      // (ActualCustomer.DetectSpot(Map.GetTile().Spot))
       

        

    }
}
