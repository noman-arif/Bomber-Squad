using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombScript : MonoBehaviour
{
    private Rigidbody bombRb;
    public float throwForce=10f;
    private PlayerController player;
    public float explosionDelay = 3f;
    private float timer;
    public GameObject explosionParticle;
    public bool isTrigger = false;
    public float radius = 5f;
    public float explosionForce=700f;
    private ScoreManager score;
    public GameObject spark;
    public float rangeY = 2;
    private EnemyScript enemyScript;
    private AudioManager audioManager;
    // Start is called before the first frame update
    void Start()
    {
        bombRb = GetComponent<Rigidbody>();                                                             //getting bomb rigidbody 
        score = GameObject.FindGameObjectWithTag("ScoreManager").GetComponent<ScoreManager>();          //accessing score script
        audioManager = GameObject.FindGameObjectWithTag("AudioManager").GetComponent<AudioManager>();   //accessing audiomanager script
        enemyScript = GameObject.FindObjectOfType<EnemyScript>();                                       //accessing enemy script
        player = GameObject.Find("Player").GetComponent<PlayerController>();                            //accessing player script
        SelectThrow();                                                                                  //call throw function
        timer = explosionDelay;                                                                         //assign value to timer variabel
    }

    // Update is called once per frame
    void Update()
    {
        DestroyBomb();                      //calling fucntion
        Timer();                            //calling timer function
    }
    private void SelectThrow()
    {
        if (player.lowThrow != true)        // if player want to throw bomb then this condition will excuted
        {
            bombRb.AddForce(player.transform.forward * throwForce, ForceMode.Impulse);
        }
        else if (player.lowThrow != false)  // if player wan to place the bomb then this will executed
        {
            bombRb.AddForce(player.transform.forward, ForceMode.Impulse);
        }
    }
    private void DestroyBomb()
    {
        if (transform.position.y < -rangeY)         //if bomb get out of the platform then it will destroy
        {
            Destroy(gameObject);
        }
    }
    private void Timer()
    {
        timer -= Time.deltaTime;                    //it use to calculate the timer to trigger the bomb
        if (timer <= 0 && isTrigger != true)        //if time is up and bomb is not explode 
        {
            TriggerBomb();                          //then trigger function is called
            isTrigger = true;                       //mean bomb is triggered
        }
    }
    private void TriggerBomb()
    {
        Instantiate(explosionParticle, transform.position, explosionParticle.transform.rotation); //spawn explosion paticles
        Collider[] detectCollision = Physics.OverlapSphere(transform.position, radius);           //detect near by collide and storre it reference in the collider array
        foreach(Collider objects in detectCollision)                                              //using foreach loop to check 
        {
            Rigidbody objectRb = objects.GetComponent<Rigidbody>();                               //rigid bodies component from the collider array
            if (objectRb != null)                                                                 //if true that rigidbodies are avaliavle
            {
                objectRb.AddExplosionForce(explosionForce,transform.position,radius);             //bomb explode
                audioManager.playAudio.PlayOneShot(audioManager.blastAudio, 1f);                  //explosion audion will play
                if (objects.CompareTag("Player"))
                {
                    score.LostLife();                                                             //if player present in that radis it will loss life 
                }
                if (objects.CompareTag("Enemy"))
                {
                    enemyScript.enemyHealth -= 10;                                               //and enemy will get damage according to the health and then destroy
                    if (enemyScript.enemyHealth <= 0)
                    {
                        Destroy(objects.gameObject);
                    }
                }
           
            }
        }
        Destroy(gameObject);

    }
    //sticky bombs agr player yh enemy collide kry sticky bomb sy tu usko ma usko object ka child bana do.
}
