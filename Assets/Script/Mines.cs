using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mines : MonoBehaviour
{
    private Rigidbody mineRb;
    private PlayerController player;
    public GameObject explosionParticle;
    public float rangeY = 2f;
    private AudioManager audioManager;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player").GetComponent<PlayerController>();    //accessing player script
        audioManager = GameObject.FindGameObjectWithTag("AudioManager").GetComponent<AudioManager>();//accessing audiomanger script
        mineRb = GetComponent<Rigidbody>();                                     //get rigidbody component
        mineRb.AddForce(player.bombPos.forward, ForceMode.Impulse);             //apply force to place mines
    }

    // Update is called once per frame
    void Update()
    {
        DestroyMine();                                                          //calling destroy mines fuctions
    }
    private void DestroyMine()                                                  //if mine fall from platform it will destroy
    {
        if (transform.position.y < -rangeY)
        {
            Destroy(gameObject);
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))                       //if player collide with mine
        {
            Instantiate(explosionParticle, transform.position, explosionParticle.transform.rotation); //explison particels spwan
            audioManager.playAudio.PlayOneShot(audioManager.blastAudio, 1f);                          //sound will be played
        }
        else if (collision.gameObject.CompareTag("Enemy"))                  //similar case for enemy 
        {
            Instantiate(explosionParticle, transform.position, explosionParticle.transform.rotation);
            audioManager.playAudio.PlayOneShot(audioManager.blastAudio, 1f);
        }
    }

}
