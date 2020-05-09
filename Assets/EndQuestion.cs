using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EndQuestion : MonoBehaviour
{
    public Question_Manager qm;
    public UI_Manager ui;
    public int question;


    private void Start()
    {
        transform.GetChild(0).GetComponent<Button>().onClick.AddListener(OnClickCall);
    }

    public void OnClickCall()
    {
        qm.SetCurrentQuestion(question);
        ui.RefreshQuestion();
        transform.parent.parent.gameObject.SetActive(false);
    }



}
