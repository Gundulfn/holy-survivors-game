using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class HitItemInfoUI : MonoBehaviour
{
    public TextMeshProUGUI hitItemText;
    public GameObject actionTakeTextObj;
    public GameObject actionConsumeTextObj;

    void Start()
    {
        // hitItemText = transform.GetChild(0).GetComponent<TextMeshProUGUI>();
        // actionTakeTextObj = transform.GetChild(1).gameObject;
        // actionConsumeTextObj = transform.GetChild(2).gameObject;
    }
}
