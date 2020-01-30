using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkyscraperHeightManager : MonoBehaviour
{
    public List<Transform> skyscrapersList = new List<Transform>();
    private GameManager playerManager;
    // Start is called before the first frame update

    private void Start()
    {
        playerManager = GetComponent<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < skyscrapersList.Count; ++i)
        {
            float maxDistance = 15.0f;  // the range at which you want to start slowing
            float percentageOfMax = Vector3.Distance(playerManager.player.car.position, skyscrapersList[i].position) / maxDistance;
            percentageOfMax = Mathf.Clamp(percentageOfMax, 0, 0.7f);
            skyscrapersList[i].transform.localScale = new Vector3(skyscrapersList[i].transform.localScale.x, percentageOfMax, skyscrapersList[i].transform.localScale.z);
        }
    }
}
