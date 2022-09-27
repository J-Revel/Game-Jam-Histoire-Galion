using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DateDisplay : MonoBehaviour
{
    public GameController gameController;
    private TMPro.TextMeshProUGUI text;
    void Start()
    {
        text = GetComponent<TMPro.TextMeshProUGUI>();
    }

    void Update()
    {
        text.text = gameController.currentDate;
    }
}
