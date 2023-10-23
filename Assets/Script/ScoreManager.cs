using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class ScoreManager : MonoBehaviour
{
    public int mineBomb = 0;
    public int stickyBomb = 0;
    public int playerLife = 1;
    public Text lifeUI;
    public Text stickyUI;
    public Text mineUI;
    public bool canMine = false;
    public bool canStick = false;
    public bool isGameOver = false;
    public GameObject gameOverText;
    public GameObject youWinText;
    private AudioManager audioManager;
    // Start is called before the first frame update
    void Start()
    {
        audioManager = GameObject.FindGameObjectWithTag("AudioManager").GetComponent<AudioManager>();   //accessing score manager cript
    }

    // Update is called once per frame
    void Update()
    {

    }
    public void MineBomb()
    {
        mineBomb = 3;                                   //fixing pick up of mines
        mineUI.text = "Mines: " + mineBomb;             //display in UI
        canMine = true;                                 // can used to check that we can place mines or not
    }
    public void CountMines()                            //mine counter
    {
        mineBomb--;
        mineUI.text = "Mines: " + mineBomb;
        if (mineBomb > 0)                               //if true we can place mine
        {
            canMine = true;
        }
        else
        {
            canMine = false;                           //else no mine left
        }
    }
    public void StickyBomb()                           //fix pick up of sticky bomb
    {
        stickyBomb = 3;
        stickyUI.text = "Sticky Bomb: " + stickyBomb;   //here we can also pick 3 sticky bomb
        canStick = true;
    }
    public void CountStickyBomb()
    {
        stickyBomb--;
        stickyUI.text = "Sticky Bomb: " + stickyBomb;

        if (stickyBomb > 0)
        {
            canStick = true;                    //if bomb avalaibel then we can used it

        }
        else
        {
            canStick = false;                   //otherwise nope
        }
    }
    public void AddLife()
    {
        playerLife++;                           //add life when player collect life prefab
        lifeUI.text = "Life: " + playerLife;
    }
    public void LostLife()
    {
        playerLife--;                           //lost live when hit by bomb or fall from platform
        lifeUI.text = "Life: " + playerLife;
        if (playerLife <= 0)
        {
            isGameOver = true;                  //if life is zero then game will be over
            GameOver();

        }
    }
    public void GameOver()
    {
        gameOverText.SetActive(true);          //for display UI of game over as well as sound
        audioManager.playAudio.PlayOneShot(audioManager.gameOverSound, 1f);
    }
    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name); //reload the current scene
    }


}
