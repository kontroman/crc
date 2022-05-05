using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FieldClearer : MonoBehaviour
{
    private void FixedUpdate()
    {
        if (transform.childCount >= 2)
            Destroy(transform.GetChild(0).gameObject);
    }
}
