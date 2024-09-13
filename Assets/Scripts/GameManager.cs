using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
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
    GameObject nextButton,muteSign,savingProgressBar;
    [SerializeField]
    AudioClip matchCardClip,comboClip, cardPlaceClip, audioOnClip;
    AudioSource audioSource;

    private void Awake()
    {
        instance = this;
    }
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    List<Sprite> cardImages;
    public void initcards(int nbrCol, int nbrRow, Transform cardContainer)
    {
        if(PlayerPrefs.GetInt("playMode")==1)
        {
          
            load(cardContainer);
        }else
        {
            
            int nbrOfCards = nbrCol * nbrRow;
            cardImages = new List<Sprite>();
            for (int i = 0; i < nbrOfCards / 2; i++)
            {
                cardImages.Add(listOfCardImages[i]);
                cardImages.Add(listOfCardImages[i]);
            }
            cardImages.Shuffle();

            StartCoroutine(spawnCards(nbrCol, nbrRow, cardContainer));
        }
        

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
        audioSource.PlayOneShot(cardPlaceClip);
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
                audioSource.PlayOneShot(comboClip);
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
    public void playClip(string clip)
    {
        switch(clip)
        {
            case "match": audioSource.PlayOneShot(matchCardClip);break;
        }
    }
    public void home()
    {
        SceneManager.LoadScene(0);
    }
    int nbrOfClicks = 0;
    
    public void mute()
    {
        int r = nbrOfClicks % 2;
        audioSource.mute=r== 0;
        muteSign.SetActive(r == 0);
        Card.mute = r == 0;
        if (r == 1)
            audioSource.PlayOneShot(audioOnClip);
        nbrOfClicks++;
    }

    IEnumerator loadCards( Transform cardContainer)
    {
      

        for (int i = 0; i < listOfImageIndexes.Count; i++)
        {
            Card card = Instantiate(cardModel, cardContainer);
            listOfCards.Add(card);
            card.setcardImage(listOfCardImages[listOfImageIndexes[i]]);
            StartCoroutine(spawnAnimation(card.GetComponent<Image>()));
            yield return new WaitUntil(() => !beginAnimation);

        }
        cardContainer.GetComponent<GridLayoutGroup>().enabled = false;
    }

    //save and load 


    List<int> listOfImageIndexes = new List<int>();
    public void save()
    {
        Card.canPlay = false;
        savingProgressBar.SetActive(true);
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + "/data.unity";
        FileStream stream = new FileStream(path, FileMode.Create);

        listOfImageIndexes.Clear();
        foreach (Card card in listOfCards)
        {
        listOfImageIndexes.Add(listOfCardImages.IndexOf(card.getCardImage().sprite));

        }
        
        Data data=new Data(listOfImageIndexes.ToArray(),matches,turns,combo);
        formatter.Serialize(stream,data);
        stream.Close();
       
    }
     void load(Transform cardContainer)
    {
        string path = Application.persistentDataPath + "/data.unity";
        if(File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);
            Data data = formatter.Deserialize(stream) as Data;
           
            listOfImageIndexes.AddRange(data.cardImageIndexes);
            matches = data.nbrOfMatches;
            turns = data.nbrOfTurns;
            combo = data.nbrCombo;
            turnsText.text = turns.ToString();
            comboTextSidBar.text =combo<0?"0":combo.ToString();
            matchesText.text = matches.ToString();
            stream.Close();
            StartCoroutine(loadCards(cardContainer));

        }
        PlayerPrefs.SetInt("playMode", 0);
    }
}
