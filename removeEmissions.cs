using UnityEngine;
using System.Collections;

public class removeEmissions : MonoBehaviour {

    private Material[] mat;

    void Start()
    {
        mat = GetComponentInChildren<Renderer>().materials;
        foreach (Material item in mat)
            item.DisableKeyword("_EMISSION");
    }
}
