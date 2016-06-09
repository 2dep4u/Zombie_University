using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class DialogueController : MonoBehaviour {

    public float dialogueTime = 4f;

    private Text dialogueBox;
    private string dialogueText;

    void Start()
    {
        dialogueBox = GetComponent<Text>();
    }

    public void displayDialogue(string text)
    {
        dialogueText = text;
        StopCoroutine("dialogueRoutine");
        StartCoroutine("dialogueRoutine");
    }

    IEnumerator dialogueRoutine()
    {
        dialogueBox.text = dialogueText;
        dialogueBox.enabled = true;
        yield return new WaitForSeconds(dialogueTime);
        dialogueBox.enabled = false;
    }
}
