using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyScript : MonoBehaviour
{
    public float speed = 10f;
    private PlayerController player;
    private Rigidbody enemyRb;
    private Animator enemyAnim;
    public GameObject[] enemyBombs;
    public Transform spawnArea;
    public float startdelay = 2;
    public float repeatTime = 3;
    private SpawnManager spMgr;
    private ScoreManager score;
    public int enemyHealth = 10;
    private AudioManager audioManager;
    public float rangeY = 3f;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player").GetComponent<PlayerController>();                         //accessing player script
        spMgr = GameObject.FindGameObjectWithTag("SpawnManager").GetComponent<SpawnManager>();       //accessing spawnmanger script
        score = GameObject.FindGameObjectWithTag("ScoreManager").GetComponent<ScoreManager>();       //accessing scoremanager script
        audioManager = GameObject.FindGameObjectWithTag("AudioManager").GetComponent<AudioManager>();//accessing audio managerscript
        enemyRb = GetComponent<Rigidbody>();                        //getting enemy rigidbody component
        enemyAnim = GetComponent<Animator>();                       //getting player animator component
        enemyAnim.SetInteger("placeWeaponInt", 0);                  //playing throwing animation
        InvokeRepeating("ThrowBomb", startdelay, repeatTime);       //throw bomb after certain amunt of time
    }

    // Update is called once per frame
    void Update()
    {
        EnemyMovement();                                           //call for enemy movememnt 
        HealthIncreasebyWave();                                    //increase health of enemy after every wave
    }
    private void EnemyMovement()
    {
        if (score.isGameOver != true && spMgr.isGameEnd != true)    //movement work until these condition become true
        {

            Vector3 moveDirection = (player.transform.position - transform.position).normalized;    //getting distance betwwen player and enemy
            if (moveDirection.magnitude > 0.1f)                                                     //check magnitude of the measurement distance
            {
                transform.Translate(moveDirection * speed * Time.deltaTime, Space.World);           //movement enemy in SpaceWorld with bounding it rotation with axis
                transform.rotation = Quaternion.LookRotation(moveDirection);                        //Rotate it in the direction of movement
                enemyAnim.SetBool("Static_b", true);                                                //running animation when moving
                enemyAnim.SetFloat("Speed_f", 0.6f);

            }
            else
            {
                enemyAnim.SetBool("Static_b", false);                                              //stop running animation
                enemyAnim.SetFloat("Speed_f", 0.1f);
            }
        }
        else
        {
            enemyAnim.SetBool("Static_b", false);
            enemyAnim.SetFloat("Speed_f", 0.1f);
        }

    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("MineBomb"))                //it dectect collision with minebomb
        {
            Destroy(gameObject);                                       //if that happend both object will get destroy
            Destroy(collision.gameObject);
        }
        else if (collision.gameObject.CompareTag("StickyBomb"))        //it detect collision with sticky bomb
        {
            collision.gameObject.transform.SetParent(transform);       //if this happend then that bomb become its child 
        }
        else if (collision.gameObject.CompareTag("Collider"))         //if enemy collide with it he will destroy
        {
            Destroy(gameObject);
        }
    }
    private void ThrowBomb()
    {
        if (score.isGameOver != true && spMgr.isGameEnd != true)
        {

            int index = Random.Range(0, enemyBombs.Length);                                                     //getting random index for bomb to throw
            audioManager.playAudio.PlayOneShot(audioManager.throwSound, 1f);                                    //play sound of throwing
            Instantiate(enemyBombs[index], spawnArea.transform.position, enemyBombs[index].transform.rotation); //spawn it in the scene
        }
    }
    public void EnemyHealth(int num)                        //decrease health of enemy if its in range of bkack bomb  
    {
        if (enemyHealth > 0)
        {
            enemyHealth -= num;                             //decrease health
            Debug.Log("Health kam hurhi");
        }
        if (enemyHealth <= 0)                              //if health reach 0 or less then enemy get kill
        {
            Debug.Log("Han hu rhi or ma maaar gya");
            Destroy(gameObject);
        }
    }
    public void HealthIncreasebyWave()                //different health assign to enemy on different waves
    {
        if (spMgr.waveNo == 2)
        {
            enemyHealth = 12;
        }
        else if (spMgr.waveNo == 3)
        {
            enemyHealth = 14;
        }
        else if (spMgr.waveNo == 4)
        {
            enemyHealth = 16;
        }
        else if (spMgr.waveNo == 5)
        {
            enemyHealth = 18;
        }
    }
}
