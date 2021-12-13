using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//parts of the game in question
public enum RoundState { START, PLAYERTURN, ENEMYTURN, WON, LOST, DRAW }

public class CardManager : MonoBehaviour
{
    //Has the player drawn their cards (offline)
    private bool isDrawn = false;
    // deck where the players gonna draw the cards from
    public PlayerDeck playerDeck;
    // players in question
    public Player player;
    public Player enemy;

    public static string[] statuses;
    public static int chosenstat;

    public Button ResetSelected;
    public Button ConfirmSelected;


    //defines what part of the game we in
    public RoundState state;

     protected void Awake()
    {
       
    }

    //Claims the game as started, shuffles cards, fills the player's hand with cards (should be for the enemy as well next time)
    protected void Start()
    {
        playerDeck.Shuffle();
        player.FillHand(6);
        enemy.FillHand(1);
        state = RoundState.START;
        
        Debug.Log(PlayerDeck.shuffleDeck.Count);
    }

    private void Update()
    {
        if (SelectedItemSlot.hasBeenSelected)
        {
            ResetSelected.gameObject.SetActive(true);
            ConfirmSelected.gameObject.SetActive(true);
            
        } else
        {
            ResetSelected.gameObject.SetActive(false);
            ConfirmSelected.gameObject.SetActive(false);
        }
     
    }


    // Method that *legitiately* starts the game. IEnumerators are kinda separate from void methods, it's a bit weird but it works
    // this one is called when you click "draw".
    // As you can see each state has a respective IEnumerator and a void method.
    // Rn I'm mainly using IEnumerators to change between states and call the respective methods
    //void methods do actual stuff in game.
    //Not consistent, feel free to change.
    IEnumerator SetupBattle()
    {
        Debug.Log("Game started.");
        yield return new WaitForSeconds(1f);
        state = RoundState.PLAYERTURN;
        StartCoroutine(PlayerTurn());
    }


    // Runs when the Draw button is clicked.
    //isDrawn bool disables the player from drawing cards to infinity
    //j is the number of cards in the player's hand,
    //may also represent the index of the cards in the shuffled deck where it's drawing cards from.
    public void OnClickDraw()
    {

        if (!isDrawn)
        {

            for (int j = 0; j < player.handOfCards.Count; j++)
            {

                player.controller.InstantiateCards(j);
                player.controller.thisCard.thisId = player.handOfCards[j].id;
               
            }
        }

        isDrawn = true;
        StartCoroutine(SetupBattle());


    }

  

    public void SelectCardPrep()
    {
       statuses = new string[6];
        statuses[0] = "strength";
        statuses[1] = "dexterity";
        statuses[2] = "constitution";
        statuses[3] = "intelligence";
        statuses[4] = "wisdom";
        statuses[5] = "charisma";
        chosenstat = Random.Range(0, statuses.Length);
        Debug.Log("Status is:" + statuses[chosenstat]);

    }

//it's the player's turn now, pops up a window letting them choose a stat to pick.
IEnumerator PlayerTurn()
    {
      SelectCardPrep();
        Debug.Log("Pick your card bro");
     
      

        //IEnumerators NEED to return something, you can let do this.
        yield return new WaitForSeconds(1f);
    }

    //Happens when a stat is clicked, index represents the button/stat in question (check inspector for the buttons)
    //once done, it hides the window, and stores the following:
    //the stat selected to be compared against the enemy
    //and the index in question to be used for the enemy, to make sure the same stat is chosen
    //index goes 0-5 for the stats repectively.
    //then it switched to the enemy's turn and calls the method for it.

    //now it takes the value from the selected card.
    public void onStatClick()
    {
        if (player.controller.selectedCard != null)
        {
            switch (statuses[chosenstat])
            {
                case "strength": player.controller.selectedVal = player.controller.selectedCard.strength; break;
                case "dexterity": player.controller.selectedVal = player.controller.selectedCard.dexterity; break;
                case "constitution": player.controller.selectedVal = player.controller.selectedCard.constitution; break;
                case "intelligence": player.controller.selectedVal = player.controller.selectedCard.intelligence; break;
                case "wisdom": player.controller.selectedVal = player.controller.selectedCard.wisdom; break;
                case "charisma": player.controller.selectedVal = player.controller.selectedCard.charisma; break;
                default: player.controller.selectedVal = 0; break;
            }
        }
       
        Debug.Log("Card picked was " + player.controller.selectedCard.cardName);
        Debug.Log(player.controller.selectedVal.ToString());
        state = RoundState.ENEMYTURN;
        StartCoroutine(EnemyTurn());

    }

    public void OnResetPressed()
    {
        SelectedItemSlot.hasBeenSelected = false;
        GameObject cardTemp = player.controller.selectGO.GetComponent<RectTransform>().GetChild(0).gameObject;
        cardTemp.GetComponent<CanvasGroup>().blocksRaycasts = true;
        cardTemp.transform.SetParent(player.controller.playerHand.transform, false);
        ResetSelected.gameObject.SetActive(false);
    }

    public void OnConfirmPressed()
    {
      //  HasBeenConfirmed = true;
        if (player.controller.selectGO.GetComponent<RectTransform>().childCount == 0)
        {
            Debug.Log("There is no card to confirm");
        }
        else
        {
            player.controller.selectedCard = player.controller.selectGO.GetComponent<RectTransform>().GetChild(0).GetComponent<ThisCard>();
            Debug.Log(player.controller.selectedCard.cardName);
            onStatClick();
        }
    }

    //instantiates a card in the enemy's area, implies the enemy  has chosen a card.
    //maybe it should be "hidden" at the start, with the back showing.
    //a "compare" button will appear for the player allowing the player to carry out the comparison and conclude the game.
    IEnumerator EnemyTurn()
    {
        yield return new WaitForSeconds(1f);
        enemy.controller.InstantiateCards(1);
        enemy.controller.thisCard.thisId = enemy.handOfCards[0].id;
        enemy.controller.physicalCards[0].transform.Find("BackOfCard").gameObject.SetActive(true);
        //enemy.controller.fetchCard(0);

        player.controller.compareB.gameObject.SetActive(true);
      


    }

    //Called when pressing the compare button
    // the aforementioned stat is now fetched form the enemy's card using the variables stored from the player's move.
    //once that is done, the comparison is carried out
    public void EnemyMove()
    {
        enemy.controller.physicalCards[0].transform.Find("BackOfCard").gameObject.SetActive(false);
        enemy.controller.fetchCard(0);
        switch (statuses[chosenstat])
        {
            case "strength": enemy.controller.selectedVal = enemy.controller.thisCard.strength; break;
            case "dexterity": enemy.controller.selectedVal = enemy.controller.thisCard.dexterity; break;
            case "constitution": enemy.controller.selectedVal = enemy.controller.thisCard.constitution; break;
            case "intelligence": enemy.controller.selectedVal = enemy.controller.thisCard.intelligence; break;
            case "wisdom": enemy.controller.selectedVal = enemy.controller.thisCard.wisdom; break;
            case "charisma": enemy.controller.selectedVal = enemy.controller.thisCard.charisma; break;
            default: enemy.controller.selectedVal = 0; break;
        }


        Debug.Log(enemy.controller.selectedVal);
        player.controller.compareB.gameObject.SetActive(false);
        StartCoroutine(Compare());
    }

    // Carries out the comparison to conclude the game, switched the game state respectively.
    IEnumerator Compare()
    {
        yield return new WaitForSeconds(1f);
       
        if (player.controller.selectedVal > enemy.controller.selectedVal)
        {
            player.score++;
            state = RoundState.WON;
            NextRound();
           
        }
        else if (enemy.controller.selectedVal > player.controller.selectedVal)
        {
            enemy.score++;
            state = RoundState.LOST;
            NextRound();
            
        }
        else if (enemy.controller.selectedVal == player.controller.selectedVal)
        {
            state = RoundState.DRAW;
            NextRound();
            
        }

      //  if (player.score == 5)
      //  {
      //      state = RoundState.WON;
      //  } else if (enemy.score == 5)
      // {
      //      state = RoundState.LOST;
      //  }
    }

    public void NextRound()
    {
        Debug.Log("Going to next round");
        EndRound();
       
    }

    // Happens depending on the current game state. Sorta self explanatory
    public void EndRound()
    {
        int removeid = player.controller.selectGO.GetComponent<RectTransform>().GetChild(0).GetComponent<ThisCard>().id;
        for (int i = 0; i < player.handOfCards.Count; i++)
        {
            if (player.handOfCards[i].id == removeid)
            {
                player.handOfCards.RemoveAt(i);
            }
        }
        player.controller.RemoveUsedCard();

        if (state == RoundState.WON)
        {
            Debug.Log("You won. Commencing victory cry.");
            new WaitForSeconds(3f);
            // Application.OpenURL("https://www.youtube.com/watch?v=sAXZbfLzJUg");
        }
        else if (state == RoundState.LOST)
        {
            Debug.Log("You lost.");
        }
        else if (state == RoundState.DRAW)
        {
            Debug.Log("It's a draw!");
        }

    }

}