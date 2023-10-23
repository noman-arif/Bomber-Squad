using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StickyBombScript : MonoBehaviour
{
    private Rigidbody stickyBombRb;
    public float throwForce = 10f;
    private PlayerController player;
    public float explosionDelay = 3f;
    private float Timer;
    public bool isTrigger = false;
    public GameObject explosionParticle;
    public float radius;
    public float explosionForce = 1000;
    private ScoreManager score;
    public GameObject spark;
    public float rangeY = 2f;
    private AudioManager audioManager;
    // Start is called before the first frame update
    void Start()
    {
        Timer = explosionDelay;                                                                         //assign value of explosion delay to timer variabel
        stickyBombRb = GetComponent<Rigidbody>();                                                       // getting rigidbody component of sticky bomb
        player = GameObject.Find("Player").GetComponent<PlayerController>();                            //accessing player script
        audioManager = GameObject.FindGameObjectWithTag("AudioManager").GetComponent<AudioManager>();   //accessing audiomanager script
        score = GameObject.FindGameObjectWithTag("ScoreManager").GetComponent<ScoreManager>();          //accessing score manger script
        stickyBombRb.AddForce(player.transform.forward * throwForce, ForceMode.Impulse);                //apply force to throw the bomb;


    }

    // Update is called once per frame
    void Update()
    {
        DestroyStickyBomb();                            //call this function
        TimerForStickyBomb();                           //call timer function
    }
    private void DestroyStickyBomb()
    {
        if (transform.position.y < -rangeY)            //if bomb fall from platform it will destroy
        {
            Destroy(gameObject);
        }
    }
    private void TimerForStickyBomb()
    {
        Timer -= Time.deltaTime;                       //calculate time after that bomb will blast
        if (Timer <= 0 && isTrigger != true)           //condition to check if times up and bomb is not trigger or explode
        {
            TriggerStickyBomb();                       //call trigger fucntion to explode the bomb
            isTrigger = true;                          //mean bomb is triggered
        }
    }
    private void TriggerStickyBomb()
    {
        Instantiate(explosionParticle, transform.position, explosionParticle.transform.rotation); //spawn explosion particels
        Collider[] detectCollision = Physics.OverlapSphere(transform.position, radius);           //getting the reference of all collider placeing in the radius of the bomb
        foreach (Collider objects in detectCollision)                                             //using foreach loop from collider array  
        {
            Rigidbody objectRb = objects.GetComponent<Rigidbody>();                               //we are getting rigidbody of allobjects
            if (objectRb != null)                                                                 //check if any object doest have rigidbody
            {
                objectRb.AddExplosionForce(explosionForce, transform.position, radius);           //apply explosion force 
                audioManager.playAudio.PlayOneShot(audioManager.blastAudio, 1f);                  //play explosion sound effect
                if (objects.CompareTag("Player"))
                {
                    score.LostLife();                                                             //if player present in that radius it will loss their life
                }
                if (objects.CompareTag("Enemy"))
                {
                    Destroy(objects.gameObject);                                                  //similar case for enemy object
                }

            }
        }
        Destroy(gameObject);                                                                      //bomb will destroy
    }
}
