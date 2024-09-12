using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Card : MonoBehaviour
{
   
    [SerializeField]
    Sprite frontCardEmpty;
  
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
        yield return new WaitUntil(() => finishedFlipAnim);
        GameManager.instance.addCard(this);

        
      
       
       
    }
    bool finishedFlipAnim = true;
    IEnumerator flip(bool back,Vector2 flipedRotation,Vector2 initRotation)
    {
       
        finishedFlipAnim = false;
       
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
        yield return new WaitForSeconds(0.1f);
        finishedFlipAnim = true;
    }

    public void match(bool isMatch,Card matchedCard)
    {
        if(!isMatch)
        {
            StopAllCoroutines();
            StartCoroutine(flip(false, flipedRotation, initRotation));
        }
        else
        {
            
            StopAllCoroutines();
            StartCoroutine(goToMachingPosition(matchedCard));
        }
    }

    IEnumerator goToMachingPosition(Card matchingCard)
    {
        Vector2 finalPos = (matchingCard.transform.localPosition + transform.localPosition) / 2;
        Vector2 initPos = transform.localPosition;
        float timeInSec = 0, timeToAnimate = 0.5f;
        while(timeInSec<timeToAnimate)
        {
            transform.localPosition = Vector2.Lerp(initPos, finalPos, timeInSec / timeToAnimate);
            timeInSec += Time.deltaTime;
            yield return new WaitForEndOfFrame();

        }
        yield return new WaitForSeconds(0.2f);
        Destroy(gameObject);

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
