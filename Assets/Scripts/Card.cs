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
    public static int activeCards=0;
     Image cardImage;
    

    void Start()
    {
       
        cardImage.gameObject.SetActive(false);
        GetComponent<Button>().onClick.AddListener(delegate { StartCoroutine(_rotateAnimation()); });
    }

    
    void Update()
    {
        
    }



    Sprite backCardImage;
    Vector2 initRotation, flipedRotation;
    IEnumerator _rotateAnimation()
    {
         initRotation = transform.localEulerAngles;
         flipedRotation = new Vector2(0, -180);
        backCardImage = GetComponent<Image>().sprite;

        StartCoroutine(flip(true,flipedRotation,initRotation));
       
        GameManager.instance.addCard(this);
        yield return new WaitForSeconds(resetTime);
        
        StartCoroutine(flip(false, flipedRotation, initRotation));
       
    }
    IEnumerator flip(bool back,Vector2 flipedRotation,Vector2 initRotation)
    {
        if (back)
            activeCards++;
        else
            activeCards--;
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

    public void match(bool isMatch)
    {
        if(!isMatch)
        {
            StopAllCoroutines();
            StartCoroutine(flip(false, flipedRotation, initRotation));
        }
    }

    public void setcardImage(Sprite img)
    {
        cardImage = GetComponentsInChildren<Image>()[1];
        cardImage.sprite = img;
    }
    public Image getCardImage()
    {
        return cardImage;
    }
}
