using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using System.IO;
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
        PlayerPrefs.SetInt("playMode", 0);
        SceneManager.LoadScene(1);
    }
    public void Load()
    {
        string path = Application.persistentDataPath + "/data.unity";
        if (File.Exists(path))
        {
            PlayerPrefs.SetInt("playMode", 1);
        }else
        {
            PlayerPrefs.SetInt("playMode", 0);
        }
        SceneManager.LoadScene(1);
    }
}
