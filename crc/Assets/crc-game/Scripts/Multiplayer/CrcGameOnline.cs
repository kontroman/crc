using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrcGameOnline : MonoBehaviour
{
    public static CrcGameOnline Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null) return;
        else Instance = this;
    }
}
