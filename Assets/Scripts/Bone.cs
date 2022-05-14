using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bone : MonoBehaviour
{

    [SerializeField]
    [Range(-0.5f, 0.5f)]
    float spinSpeed = 0.1f;

    void Update()
    {
        transform.Rotate(new Vector3(0, 0, spinSpeed));
    }
}
