using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawCards : MonoBehaviour
{
    public GameObject GameCard;
    //public GameObject Card2;
    public GameObject PlayerArea;
   // public GameObject EnemyArea;
   private bool isClicked;

    private List<GameObject> cards = new List<GameObject>();
    void Start()
    {
        cards.Add(GameCard);
       // cards.Add(Card2);
       isClicked = false;


    }

    public void OnClick()
    {
        if(isClicked == false){
        for (var i = 0; i < 4; i++)
        {
            GameObject playerCard = Instantiate(cards[Random.Range(0,cards.Count)], new Vector3(0, 0, 0), Quaternion.identity);
            playerCard.transform.SetParent(PlayerArea.transform, false);
            

           // GameObject enemyCard = Instantiate(cards[Random.Range(0, cards.Count)], new Vector3(0, 0, 0), Quaternion.identity);
           // enemyCard.transform.SetParent(EnemyArea.transform, false);
        }
        }
        isClicked = true;

    }
}