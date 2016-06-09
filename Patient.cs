using UnityEngine;
using System.Collections;

public class Patient : MonoBehaviour {

    [SerializeField] private GameObject injectionArea;
    private Material mat;
    private Collider col;
    private GameController gameController;
    private GameObject vaccine;
    private bool haveVaccine = false;

    void Start()
    {
        col = GetComponent<Collider>();
        col.enabled = false;
        mat = injectionArea.GetComponent<Renderer>().material;
        mat.DisableKeyword("_EMISSION");
        gameController = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();
        vaccine = transform.FindChild("Vaccine").gameObject;
    }

    public void Interact()
    {
        if (haveVaccine)
        {
            gameController.SendMessage("startPhase1");
            vaccine.SetActive(true);
            StopCoroutine("pulseInjectionArea");
            mat.DisableKeyword("_EMISSION");
            col.enabled = false;
        }
    }

    public void setHaveVaccine()
    {
        haveVaccine = true;
        col.enabled = true;
        StartCoroutine("pulseInjectionArea");
    }

    IEnumerator pulseInjectionArea()
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
}
