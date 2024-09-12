using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class LayoutChoiceManager : MonoBehaviour
{
    TMP_Dropdown dropdown;
    void Start()
    {
        dropdown = GetComponent<TMP_Dropdown>();
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void Play()
    {        
        PlayerPrefs.SetString("Layout", dropdown.options[dropdown.value].text);
        SceneManager.LoadScene(1);
    }
}
