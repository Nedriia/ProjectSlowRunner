using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Glitch : MonoBehaviour
{
    public float duration = 1.5f;
    public float force = .4f;

    private void OnEnable()
    {
        Camera.main.transform.DOShakePosition(duration, force);
    }
}
