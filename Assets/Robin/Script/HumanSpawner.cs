using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PathCreation;

using PathCreation.Examples;

public class HumanSpawner : MonoBehaviour
{
    public PathCreator Path;
    public List<PathFollower> PathFollowersPrebabs;
    public int NumberHuman { get; set; } = 1;
    public float HumanLuckSpawn;
    public List<PathFollower> PathFollowers { get; set; }
    


    // Start is called before the first frame update
    void Start()
    {
        if(Random.Range(0f,100f) <= HumanLuckSpawn)
        {
            PathFollowers = new List<PathFollower>();
            NumberHuman = Random.Range(1, 5);
            for (int i = 0; i < NumberHuman; i++)
            {
                PathFollowers.Add(Instantiate(PathFollowersPrebabs[Random.Range(0, PathFollowersPrebabs.Count - 1)]));
            }

            PathFollowers.ForEach(f => { f.pathCreator = Path; f.speed = Random.Range(1f, 2f); });
        }

    }

 
}
