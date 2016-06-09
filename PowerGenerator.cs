using UnityEngine;
using System.Collections;


public class PowerGenerator : MonoBehaviour {

    [SerializeField] private GameObject dialogueBox;
    [Multiline]
    [SerializeField] private string dialogue1 = "default text";
    [Multiline]
    [SerializeField] private string dialogue2 = "default text";
    [Multiline]
    [SerializeField] private string dialogue3 = "default text";
    [SerializeField] private GameObject generatorPoint;
    [SerializeField] private GameObject electricSwitch;

    private GameController lastPlayerSighting;
    private AudioSource audioSource;
    private bool haveGas;
    private Collider col;
    private Material mat;

    void Start()
    {
        haveGas = false;
        col = GetComponent<Collider>();
        col.enabled = false;
        mat = GetComponentInChildren<Renderer>().material;
        mat.DisableKeyword("_EMISSION");
        audioSource = GetComponent<AudioSource>();
        lastPlayerSighting = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();
    }

    public void Interact()
    {
        if (haveGas)
        {
            StopCoroutine("pulseGenerator");
            mat.DisableKeyword("_EMISSION");
            col.enabled = false;  
            StartCoroutine("startGenerator");
            electricSwitch.SendMessage("enableInteraction");
            lastPlayerSighting.position = generatorPoint.transform.position;
        }
        else
            dialogueBox.SendMessage("displayDialogue", dialogue1);

    }

    public void enableInteraction()
    {
        col.enabled = true;
        StartCoroutine("pulseGenerator");
    }

    public void setHaveGas()
    {
        haveGas = true;
    }

    IEnumerator pulseGenerator()
    {
        bool flag = false;
        while (true)
        {
            flag = !flag;
            if(flag)
                mat.EnableKeyword("_EMISSION");
            else
                mat.DisableKeyword("_EMISSION");
            yield return new WaitForSeconds(1f);
            yield return null;
        }
    }

    IEnumerator startGenerator()
    {
        audioSource.Play();
        dialogueBox.SendMessage("displayDialogue", dialogue2);
        yield return new WaitForSeconds(audioSource.clip.length);
        electricSwitch.SendMessage("setGenOn");
        dialogueBox.SendMessage("displayDialogue", dialogue3);

    }
}
