using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    [SerializeField]
    Card cardModel;

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
    public IEnumerator spawnCards(int nbrCol,int nbrRow,Transform cardContainer)
    {
        int nbrOfCards = nbrCol * nbrRow;

        for (int i = 0; i < nbrOfCards; i++)
        {
            Card card = Instantiate(cardModel, cardContainer);
            StartCoroutine(spawnAnimation(card.GetComponent<Image>()));
            yield return new WaitUntil(() => !beginAnimation);

        }
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
}
