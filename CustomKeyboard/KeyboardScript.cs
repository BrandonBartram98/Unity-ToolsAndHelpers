using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class KeyboardScript : MonoBehaviour
{
    [SerializeField] TMP_InputField TextField;
    [SerializeField] GameObject lowercaseKb, uppercaseKb, symbolKb;

    public void AlphabetFunction(string alphabet)
    {
        if (TextField.text.Length < TextField.characterLimit)
        {
            TextField.text += alphabet;
        }
    }

    public void ClearAll()
    {
        TextField.text = "";
    }

    public void BackSpace()
    {
        if (TextField.text.Length > 0)
        {
            TextField.text = TextField.text.Remove(TextField.text.Length - 1);
        }
    }

    public void CloseAllLayouts()
    {
        lowercaseKb.SetActive(false);
        uppercaseKb.SetActive(false);
        symbolKb.SetActive(false);
    }

    public void ShowLayout(GameObject SetLayout)
    {
        CloseAllLayouts();
        SetLayout.SetActive(true);
    }

}
