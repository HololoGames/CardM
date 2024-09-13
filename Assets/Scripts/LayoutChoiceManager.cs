using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using System.IO;
public class LayoutChoiceManager : MonoBehaviour
{
    [SerializeField]
    TMP_InputField rows, columns;
  
    [SerializeField]
    Animator errorMessage;
    void Start()
    {
      
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void Play()
    {
        if (!checkIfLayoutCorrect())
            return;
        PlayerPrefs.SetString("Layout", rows.text+"x"+columns.text);
        PlayerPrefs.SetInt("playMode", 0);
        SceneManager.LoadScene(1);
    }
    public void Load()
    {
        string path = Application.persistentDataPath + "/data.unity";
        if (File.Exists(path))
        {
            PlayerPrefs.SetInt("playMode", 1);
            SceneManager.LoadScene(1);
        }
        else
        {
            errorMessage.Play("loadErrorMessage");
        }
        
    }
    bool checkIfLayoutCorrect()
    {
        if(rows.text.Length==0 || columns.text.Length==0)
        {
            errorMessage.GetComponent<TextMeshProUGUI>().text = "Please choose a layout";
            errorMessage.Play("loadErrorMessage");
            return false;
        }
        

        int row = int.Parse(rows.text);
        int col= int.Parse(columns.text);
        if((row*col)%2==1)
        {
            errorMessage.GetComponent<TextMeshProUGUI>().text = "the number of cards should be even";
            errorMessage.Play("loadErrorMessage");
            return false;
        }
        return true;
    }
    public void quit()
    {
        Application.Quit();
    }
}
