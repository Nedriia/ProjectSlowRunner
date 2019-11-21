using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkyManager : MonoBehaviour
{
    public Material MaterialSky;
    public Material MaterialGlass;
    public Light Light;
    public float Speed;
    // Start is called before the first frame update
    void Start()
    {
        //  MaterialSky.SetTextureOffset("_MainTex").x = 0.1;
        MaterialSky.SetTextureOffset("_MainTex", new Vector2(0.1f, 0));
        MaterialGlass.SetColor("_EmissionColor", Color.black);
        Light.color = Color.white;
    }

    // Update is called once per frame
    void Update()
    {
     /*   Vector2 newOffset = new Vector2(0,0);
        newOffset.x = ;// MaterialSky.GetTextureOffset("_MainTex").x + Speed * Time.deltaTime;*/
        Light.color = Color.Lerp(Light.color, Color.black, 2*Speed * Time.deltaTime);
        MaterialSky.SetTextureOffset("_MainTex",new Vector2( Mathf.Lerp(0, 1, Speed * Time.time),0));
        MaterialGlass.SetColor("_EmissionColor", Color.Lerp(MaterialGlass.GetColor("_EmissionColor"), Color.yellow, 2*Speed*Time.deltaTime));
        /*   if(MaterialSky.GetTextureOffset("_MainTex").x >= 0.5f)
           {

           }
           */

    }
}
