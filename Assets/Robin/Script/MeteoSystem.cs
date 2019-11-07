using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeteoSystem : MonoBehaviour
{

    public Mesh Mesh { get; set; }
    public float ProbaEffect;
    public float ProbaRain;
    public float ProbaThunder;
    public float ProbaSnow;
    public float ProbaSunny;
    public GameObject Rain;
    public GameObject Thunder;
    public GameObject Snow;
    public GameObject Sunny;

    private void Start()
    {
        Mesh = GetComponent<MeshFilter>().mesh;
        foreach (Vector3 positionVertices in Mesh.vertices)
        {
            if(Random.Range(0f, 100f) <= ProbaEffect)
            {
                float choseEffectProba = Random.Range(0f, 100f);
                
                if(choseEffectProba <= ProbaRain)
                {
                    GameObject gameObject_ = Instantiate(Rain, positionVertices, Quaternion.identity,transform);
                    gameObject_.transform.localScale = new Vector3(0.003f, 0.003f, 0.003f);
                    gameObject_.transform.localPosition = new Vector3(0, 0, 0);
                    gameObject_.transform.up = positionVertices.normalized;
                }
                else if (choseEffectProba <= ProbaThunder)
                {
                    GameObject gameObject_ = Instantiate(Thunder, positionVertices, Quaternion.identity, transform);
                    gameObject_.transform.localScale = new Vector3(0.003f, 0.003f, 0.003f);
                    gameObject_.transform.localPosition = new Vector3(0, 0, 0);
                    gameObject_.transform.up = positionVertices.normalized;
                }
                else if (choseEffectProba <= ProbaSnow)
                {
                    GameObject gameObject_ =  Instantiate(Snow, positionVertices, Quaternion.identity, transform);
                    gameObject_.transform.localScale = new Vector3(0.003f, 0.003f, 0.003f);
                    gameObject_.transform.localPosition = new Vector3(0, 0, 0);
                    gameObject_.transform.up = positionVertices.normalized;
                }
              
                
            }
        }
    }




}
