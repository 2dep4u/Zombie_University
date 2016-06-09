using UnityEngine;
using System.Collections;

public class ElectricSwitch : MonoBehaviour {

    [SerializeField] private GameObject dialogueBox;
    [Multiline]
    [SerializeField] private string dialogue1 = "default text";
    [Multiline]
    [SerializeField] private string dialogue2 = "default text";

    [SerializeField] private GameObject lights;
    [SerializeField] private GameObject elevator;

    private Material mat;
    private AudioSource audioSource;
    private bool gensOn = false;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        mat = GetComponent<Renderer>().material;
        mat.DisableKeyword("_EMISSION");
        tag = "Untagged";
    }

    public void Interact()
    {
        if (gensOn)
        {
            lights.SetActive(true);
            audioSource.Play();
            elevator.SendMessage("setPower");
            dialogueBox.SendMessage("displayDialogue", dialogue2);
            StopCoroutine("pulseSwitch");
            mat.DisableKeyword("_EMISSION");
            tag = "Untagged";
        }
        else
            dialogueBox.SendMessage("displayDialogue", dialogue1);
    }

    public void setGenOn()
    {
        gensOn = true;
    }

    public void enableInteraction()
    {
        tag = "Switch";
        StartCoroutine("pulseSwitch");
    }

    public void playSwitchSound()
    {
        audioSource.Play();
    }

    IEnumerator pulseSwitch()
    {
        bool flag = false;
        while (true)
        {
            flag = !flag;
            if (flag)
                mat.EnableKeyword("_EMISSION");
            else
                mat.DisableKeyword("_EMISSION");
            yield return new WaitForSeconds(1f);
            yield return null;
        }
    }
}
