using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    [SerializeField]
    Card cardModel;

    [SerializeField]
    List<Sprite> listOfCardImages = new List<Sprite>();

    private void Awake()
    {
        instance = this;
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    List<Sprite> cardImages;
    public void initcards(int nbrCol, int nbrRow, Transform cardContainer)
    {
        int nbrOfCards = nbrCol * nbrRow;
        cardImages = new List<Sprite>();
        for(int i=0;i<nbrOfCards/2;i++)
        {
            cardImages.Add(listOfCardImages[i]);
            cardImages.Add(listOfCardImages[i]);
        }
        cardImages.Shuffle();
        
        StartCoroutine(spawnCards(nbrCol, nbrRow, cardContainer));

    }


     IEnumerator spawnCards(int nbrCol,int nbrRow,Transform cardContainer)
    {
        int nbrOfCards = nbrCol * nbrRow;

        for (int i = 0; i < nbrOfCards; i++)
        {
            Card card = Instantiate(cardModel, cardContainer);
            
            card.setcardImage(cardImages[i]);
            StartCoroutine(spawnAnimation(card.GetComponent<Image>()));
            yield return new WaitUntil(() => !beginAnimation);

        }
        cardContainer.GetComponent<GridLayoutGroup>().enabled = false;
    }
    bool beginAnimation = false;
    IEnumerator spawnAnimation(Image cardImage)
    {
        beginAnimation = true;
        float timeInSec = 0, timeToAnimate = 0.1f;
        Color c = cardImage.color;
        while (timeInSec < timeToAnimate)
        {
            cardImage.color = new Color(c.r, c.g, c.b, Mathf.Lerp(0, 1, timeInSec / timeToAnimate));
            timeInSec += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
        cardImage.color = new Color(c.r, c.g, c.b, 1);
        beginAnimation = false;
    }
    Card activeCard, previousCard;
    public void addCard(Card card)
    {
        
        previousCard = activeCard;
        activeCard = card;
        
        if (!previousCard)
            return;
        if( previousCard.getCardImage().sprite== activeCard.getCardImage().sprite)
        {
           
            activeCard.match(true,previousCard);
            previousCard.match(true,activeCard);
            activeCard = previousCard = null;
        }
        else
        {
            activeCard.match(false,null);
            previousCard.match(false,null);
            activeCard = previousCard = null;
            
        }
    }
    
}
