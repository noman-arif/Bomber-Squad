using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpawnManager : MonoBehaviour
{
    public GameObject[] collectables;
    public GameObject enemyPrefab;
    public float rangeX;
    public float rangeZ;
    public float startDelay = 3;
    public float repeatDelay = 5f;
    public int enemyCount;
    public int waveNo = 1;
    public Text waveUI;
    public bool isGameEnd = false;
    private ScoreManager score;
    private AudioManager audioManager;

    // Start is called before the first frame update
    void Start()
    {
        score = GameObject.FindGameObjectWithTag("ScoreManager").GetComponent<ScoreManager>();       //accessing score manager scripy
        audioManager = GameObject.FindGameObjectWithTag("AudioManager").GetComponent<AudioManager>();//accessing audio manager script
        SpawnEnemy(waveNo);                                                                          //spawn enemy when script load
        SpawnCollectable();                                                                          //as well as collectables
    }

    // Update is called once per frame
    void Update()
    {
        WaveCounter();                                                      //call wavecounter function 
    }
    private void WaveCounter()
    {
        enemyCount = FindObjectsOfType<EnemyScript>().Length;              //getting number of enemy avaliable in the scene and store in enemy counter
        if (enemyCount == 0)                                               //if all present enemy get kill then next wave start 
        {
            if (waveNo < 5)                                               //we have total five waves                                  
            {
                waveNo++;                                                 //if all present enemygot kill then new wave spawn
                waveUI.text = "Wave: " + waveNo;                          //display wave on UI
                SpawnEnemy(waveNo);                                       //the spawn enemy again for next wave
                SpawnCollectable();                                       //as well as collectable
            }
            else if (waveNo == 5 && enemyCount == 0)                      //check if wave is 5 and all enemy are vanished with surf excel
            {
                isGameEnd = true;                                         // then game is officially ended
                score.youWinText.SetActive(true);                         //display you win UI
                audioManager.playAudio.PlayOneShot(audioManager.winSound, 1f);// play that sound
            }

        }
    }
    private void SpawnEnemy(int wave)
    {
        for (int i = 0; i < wave; i++)                  //spawn number of eneny according to the number of waves
        {
            Instantiate(enemyPrefab, SpawnPosition(), enemyPrefab.transform.rotation); //spawn enemy prefab in scene
        }
    }
    private Vector3 SpawnPosition()
    {
        float atX = Random.Range(-rangeX, rangeX);     //getting random X position
        float atZ = Random.Range(-rangeZ, rangeZ);     //getting random Y position
        Vector3 spawnPos = new Vector3(atX, 0, atZ);   //store it in a vector 
        return spawnPos;                               //return it so that it can use to spawn prefabs at random places
    }
    private void SpawnCollectable()
    {
        for (int i = 0; i < collectables.Length; i++)      //spawn all collectable
        {
            Instantiate(collectables[i], SpawnPosition(), collectables[i].transform.rotation);
        }
    }
}
