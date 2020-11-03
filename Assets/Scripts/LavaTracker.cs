using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class LavaTracker : MonoBehaviour
{
    [SerializeField] GameObject Lava = null;
    [SerializeField] GameObject Player = null;

    [SerializeField] RawImage PlayerImage = null;
    [SerializeField] RawImage LavaImage = null;

    //Thing I want to modify
    RectTransform UI_PlayerRec;
    RectTransform UI_LavaRec;

    //Thing that will update
    Vector3 UI_PlayerPos;
    Vector3 UI_LavaPos;

    //Thing that stores the players position
    Vector3 PlayerPos;
    Vector3 LavaPos;

    //Thing that stores the original Y position
    float UI_Player_StartingLine = 0f;
    float Player_StartingLine = 0f;
    float UI_Lava_StartingLine = 0f;
    float Lava_StartingLine = 0f;

    private void Awake()
    {
        //gets the component
        UI_PlayerRec = PlayerImage.GetComponent<RectTransform>();
        //Gets the og number to use for the UI
        UI_Player_StartingLine = UI_PlayerRec.anchoredPosition.y;
        //Gets the og number to use for the Player
        Player_StartingLine = Player.transform.position.y;
        //Makes the updatable  = to the PlayerPos
        PlayerPos = Player.transform.position;
        //Makes the Updatable = to the PlayerRec
        UI_PlayerPos = UI_PlayerRec.anchoredPosition;

        //gets the component
        UI_LavaRec = LavaImage.GetComponent<RectTransform>();
        //Gets the og number to use for the UI
        UI_Lava_StartingLine = UI_LavaRec.anchoredPosition.y;
        //Gets the og number to use for the Player
        Lava_StartingLine = Lava.transform.position.y;
        //Makes the updatable  = to the PlayerPos
        LavaPos = Lava.transform.position;
        //Makes the Updatable = to the PlayerRec
        UI_LavaPos = UI_LavaRec.anchoredPosition;



        //I'm dying help



    }

    // Update is called once per frame
    void Update()
    {
        PlayerPos = Player.transform.position;
        LavaPos = Lava.transform.position;
        //UI_PlayerPos.y += .1f; 
       
        //UI_PlayerPos.y = (PlayerPos.y*3.7f - Player_StartingLine) + (UI_PlayerRec.position.y + UI_Player_StartingLine);
        //UI_LavaPos.y = (LavaPos.y*3.7f - Lava_StartingLine) + (UI_LavaRec.position.y+59f + UI_Lava_StartingLine);

        UI_PlayerPos.y = (PlayerPos.y - Player_StartingLine)*4.5f;
        UI_LavaPos.y =  (LavaPos.y)*4.5f;
        Debug.Log("PlayerPos: " + PlayerPos.y + "+ PlayerStartingLine: " + Player_StartingLine + "=" + (PlayerPos.y - Player_StartingLine));
        Debug.Log("LavaPos: " + LavaPos.y + "+ LavaStartingLine: " + Lava_StartingLine + "=" + (PlayerPos.y - Player_StartingLine));


        UI_PlayerRec.localPosition =UI_PlayerPos;
        UI_LavaRec.localPosition = UI_LavaPos;

    }
}
