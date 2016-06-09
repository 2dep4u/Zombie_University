using UnityEngine;
using System.Collections;

public class GasContainer : MonoBehaviour {

    [SerializeField] private GameObject dialogueBox;
    [Multiline]
    [SerializeField] private string dialogueText = "displays if canEquip is true and messages Game Controller";
    [SerializeField] private GameObject generator;

    private Collider[] col;
    private Material mat;
    private GameController gameController;
    private bool emitON;

    void Start()
    {
        col = GetComponents<Collider>();
        col[0].enabled = false;
        col[1].enabled = false;
        emitON = false;
        mat = GetComponentInChildren<Renderer>().material;
        mat.DisableKeyword("_EMISSION");
        gameController = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();
    }

    public void Interact()
    {
            dialogueBox.SendMessage("displayDialogue", dialogueText);
            gameController.SendMessage("startPhase2");
            generator.SendMessage("setHaveGas");
            Destroy(gameObject);
    }

    public void enableInteraction()
    {
        col[0].enabled = true;
        col[1].enabled = true;
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


