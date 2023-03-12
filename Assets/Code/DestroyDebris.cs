using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyDebris : MonoBehaviour
{
    private Transform TF;
    void Start()
    {
        TF = GetComponent<Transform>();
    }

    void Update()
    {
        if (TF.transform.position.y < -10)
            Destroy(gameObject);
    }
}
