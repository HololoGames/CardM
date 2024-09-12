using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CardScaler : MonoBehaviour
{
    RectTransform panelTransform;  
    GridLayoutGroup gridLayoutGroup;
   
    [SerializeField]
    Card cardModel;
   


    private void Start()
    {
        panelTransform = GetComponent<RectTransform>();
        gridLayoutGroup = GetComponent<GridLayoutGroup>();
        adjustLayout();
    }
    private void Update()
    {
       
    }
    int nbrCol, nbrRow;
    void adjustLayout()
    {
        
        string layoutStr=PlayerPrefs.GetString("Layout");
        nbrCol = int.Parse(layoutStr.Split("x")[1]);
        nbrRow = int.Parse(layoutStr.Split("x")[0]);
        gridLayoutGroup.constraintCount = nbrCol;
        float x = ((panelTransform.rect.width - gridLayoutGroup.padding.left + gridLayoutGroup.padding.right) - nbrCol * gridLayoutGroup.spacing.x) / nbrCol;
        float y = (panelTransform.rect.height - gridLayoutGroup.padding.top + gridLayoutGroup.padding.bottom - nbrCol * gridLayoutGroup.spacing.y) / nbrRow;
        gridLayoutGroup.cellSize = new Vector2(x, y);
        StartCoroutine(spawnCards());
    }
    IEnumerator spawnCards()
    {
        int nbrOfCards = nbrCol * nbrRow;
        
        for(int i=0;i<nbrOfCards;i++)
        {
            Card card= Instantiate(cardModel, transform);
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
            cardImage.color = new Color(c.r, c.g, c.b, Mathf.Lerp(0, 1,timeInSec / timeToAnimate));
            timeInSec += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
        cardImage.color = new Color(c.r,c.g,c.b,1);
        beginAnimation = false;
    }
}
