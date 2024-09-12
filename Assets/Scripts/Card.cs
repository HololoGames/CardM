using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Card : MonoBehaviour
{
    [SerializeField]
    float resetTime=2;
    [SerializeField]
    Sprite frontCardEmpty;
    Image cardImage;
    void Start()
    {
        cardImage = GetComponentsInChildren<Image>()[1];
        cardImage.gameObject.SetActive(false);
        GetComponent<Button>().onClick.AddListener(delegate { StartCoroutine(_rotateAnimation()); });
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    Sprite backCardImage;
    IEnumerator _rotateAnimation()
    {
        Vector2 initRotation = transform.localEulerAngles;
        Vector2 flipedRotation = new Vector2(0, -180);
        backCardImage = GetComponent<Image>().sprite;

        StartCoroutine(flip(true,flipedRotation,initRotation));
        yield return new WaitForSeconds(resetTime);
        StartCoroutine(flip(false, flipedRotation, initRotation));
      
    }
    IEnumerator flip(bool back,Vector2 flipedRotation,Vector2 initRotation)
    {
        float animationTime = 0.1f, timeInsec = 0;
       
        Image image = GetComponent<Image>();
        Vector2 initRot = back ? initRotation : flipedRotation;
        Vector2 flipedRot = back ? flipedRotation : initRotation;
        
        
        while (timeInsec < animationTime)
        {
            transform.localEulerAngles = Vector2.Lerp(initRot, flipedRot, timeInsec / animationTime);
         
            if (back ? transform.localEulerAngles.y > 90 : transform.localEulerAngles.y <= 90)
            {
                image.sprite = frontCardEmpty;
                cardImage.gameObject.SetActive(true);
            }
            else
            {
                image.sprite = backCardImage;
                cardImage.gameObject.SetActive(false);
            }

            timeInsec += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
        transform.localEulerAngles =flipedRot;
    }
}
