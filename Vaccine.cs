using UnityEngine;
using System.Collections;

public class Vaccine : MonoBehaviour {

    [SerializeField] public GameObject dialogueBox;
    [SerializeField] [Multiline] public string dialogue = "default text";
    [SerializeField] public GameObject patient;

    private Material[] mat;
    private bool emitON;

    void Start()
    {
        emitON = false;
        mat = GetComponentInChildren<Renderer>().materials;
        foreach (Material item in mat)
            item.DisableKeyword("_EMISSION");
    }

    public void Interact()
    {
        patient.SendMessage("setHaveVaccine");
        dialogueBox.SendMessage("displayDialogue", dialogue);
        Destroy(gameObject);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player" && emitON == false)
        {
            emitON = true;
            foreach (Material item in mat)
                item.EnableKeyword("_EMISSION");
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            emitON = false;
            foreach (Material item in mat)
                item.DisableKeyword("_EMISSION");
        }
    }
}
