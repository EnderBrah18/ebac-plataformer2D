using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SOUIintUpdate : MonoBehaviour
{
    public SOint soInt;
    public TextMeshProUGUI uiTextValue;

    private void Start()
    {
        uiTextValue.text = soInt.Value.ToString();

    }

    private void Update()
    {
        uiTextValue.text = soInt.Value.ToString();
    }
}
