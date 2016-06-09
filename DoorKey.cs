using UnityEngine;
using System.Collections;

public class DoorKey : MonoBehaviour {

    [SerializeField] private int keyID = 0;
    [SerializeField] private GameObject meshObject;

    private Material mat;
    private bool emitON;

    void Awake()
    {
        mat = GetComponentInChildren<Renderer>().material;
        mat.DisableKeyword("_EMISSION");
        emitON = false; 
    }
    
    public void getKeyID(Transform player)
    {
        player.SendMessage("addKey", keyID);
    }

    public void destroySelf()
    {
        Destroy(gameObject);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player" && emitON == false)
        {
            emitON = true;
            mat.EnableKeyword("_EMISSION");
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            emitON = false;
            mat.DisableKeyword("_EMISSION");
        }
    }
}
