using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    [SerializeField]
    Card cardModel;

    [SerializeField]
    List<Sprite> listOfCardImages = new List<Sprite>();

    [SerializeField]
    TextMeshProUGUI matchesText,turnsText,comboText,comboTextSidBar;
    [SerializeField]
    GameObject nextButton;
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

    List<Card> listOfCards = new List<Card>();
     IEnumerator spawnCards(int nbrCol,int nbrRow,Transform cardContainer)
    {
        int nbrOfCards = nbrCol * nbrRow;

        for (int i = 0; i < nbrOfCards; i++)
        {
            Card card = Instantiate(cardModel, cardContainer);
            listOfCards.Add(card);
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
    int turns, matches,combo=-1;
    
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
            matches++;
            combo++;
            if(combo>=1)
            {
                comboText.text = "combo " + combo;
                comboText.gameObject.SetActive(true);
                comboTextSidBar.text = combo.ToString();
            }

            matchesText.text = matches.ToString();
        }
        else
        {
            activeCard.match(false,null);
            previousCard.match(false,null);
            activeCard = previousCard = null;
            turns++;
            combo = -1;
            comboTextSidBar.text = "0";
            turnsText.text = turns.ToString();
            
        }
     
    }
    public void next()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    public void destroyCard(Card card)
    {
       
        listOfCards.Remove(card);
     
        Destroy(card.gameObject);
        if (listOfCards.Count <= 0)
            nextButton.SetActive(true);
    }
    public void home()
    {
        SceneManager.LoadScene(0);
    }
    
}
