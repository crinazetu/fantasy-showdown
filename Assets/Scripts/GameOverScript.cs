using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using UnityEngine.SceneManagement;

public class GameOverScript : MonoBehaviour
{

    [SerializeField]
    private Text winConclusion;
    [SerializeField]
    private Text winsnum;
    [SerializeField]
    private Text lossnum;
    [SerializeField]
    private Player thisplayer;

    // Start is called before the first frame update
    void Start()
    {


    }

    // Update is called once per frame
    void Update()
    {
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");

        foreach (GameObject p in players)
        {
            if (p.GetComponent<PhotonView>().IsMine)
            {
                thisplayer = p.GetComponent<Player>();

            }
        }


        if (thisplayer.isWinner)
        {
            winConclusion.text = "You won this game! Congrats!";

            if(FirebaseManager.Singleton.User != null)
            {
                StartCoroutine(FirebaseManager.Singleton.GetWins());
                StartCoroutine(FirebaseManager.Singleton.UpdateWins(FirebaseManager.Singleton.wins++));
            }
            
        }
        else
        {
            winConclusion.text = "You lost this game. Better luck next time.";

            if(FirebaseManager.Singleton.User != null)
            {
                StartCoroutine(FirebaseManager.Singleton.GetLosses());
                StartCoroutine(FirebaseManager.Singleton.UpdateLosses(FirebaseManager.Singleton.losses++));
            }
        }
        winsnum.text = thisplayer.score.ToString();
        lossnum.text = (5 - thisplayer.score).ToString();
        

    }

    public void BackToLobby()
    {
        PhotonNetwork.Disconnect();
        SceneManager.LoadScene("Loading");
    }

    public void BackToStart()
    {
        PhotonNetwork.Disconnect();
        PhotonNetwork.LoadLevel("Menu");
    }
}
