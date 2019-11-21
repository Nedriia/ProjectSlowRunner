using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scale_FloatingText : MonoBehaviour
{
    public float augmentation;
    public float augmentationPos;
    void Update()
    {
        transform.localScale += new Vector3(augmentation, augmentation, 1);
        transform.position -= new Vector3(0, augmentationPos, 0);
    }
}
