using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float playerSpeed = 10f;
    private Rigidbody playerRb;
    private Animator playerAnim;
    public GameObject bombPrefab;
    public Transform bombPos;
    public Transform bombPosLeft;
    public Transform bombPosRight;
    public bool isDouble = false;
    public bool lowThrow = false;
    private ScoreManager score;
    public GameObject minePrefab;
    public GameObject stickyBombPrefab;
    private SpawnManager spawnManager;
    public float rangeY = 2;
    public float sprintSpeed = 10f;
    private AudioManager audioManager;
    // Start is called before the first frame update
    void Start()
    {
        playerRb = GetComponent<Rigidbody>();                                                           //getting rigid body component
        score = GameObject.FindGameObjectWithTag("ScoreManager").GetComponent<ScoreManager>();          //acessing score manger
        audioManager = GameObject.FindGameObjectWithTag("AudioManager").GetComponent<AudioManager>();   //accessing audio manager script
        spawnManager = GameObject.FindGameObjectWithTag("SpawnManager").GetComponent<SpawnManager>();   //accessing spawn manager script
        playerAnim = GetComponent<Animator>();                                                          //getting animator component
        playerAnim.SetFloat("Speed_f", 0.1f);                                                           //set walking animation to idle
    }

    // Update is called once per frame
    void Update()
    {
        DestroyPlayer();
        PlayerMovement();
        ThrowBasicBomb();                                   //call the following fucntion :)
        PlacingBasicBomb();
        MineBomb();
        StickyBomb();
        PlayerDeath();
    }
    //Player death Function
    private void PlayerDeath()
    {
        if (score.isGameOver != false)                          //if game over then 
        {
            playerAnim.SetBool("Death_b", true);                //death animation 
            playerAnim.SetInteger("DeathType_int", 1);          //this mean that player will fall backward
        }
    }
    //Destroy Player 
    private void DestroyPlayer()
    {
        if (transform.position.y < -rangeY)
        {
            score.isGameOver = true;                             //if player fall from platform then game will over as well
            score.GameOver();                                    // player object will get desrtroy
            Destroy(gameObject);
        }
    }
    //player movement
    private void PlayerMovement()
    {
        if (score.isGameOver != true && spawnManager.isGameEnd != true) //work until condition is true
        {

            float horiInput = Input.GetAxis("Horizontal");              //getting axes
            float vertInput = Input.GetAxis("Vertical");
            Vector3 movePlayer = new Vector3(horiInput, 0, vertInput);  //storing it in a new vector 3
            if (Input.GetKeyDown(KeyCode.LeftShift))                    //if player press shift ki then walk it move faster 
            {
                playerSpeed += sprintSpeed;
            }
            else if (Input.GetKeyUp(KeyCode.LeftShift))                 //if shift key is not pressed then player will move with it actual speed
            {
                playerSpeed -= sprintSpeed;
            }
            if (movePlayer.magnitude > 0.1f)                            //getting magnitude of that vector 3
            {
                transform.Translate(movePlayer * playerSpeed * Time.deltaTime, Space.World);    //moving player without effective it rotation
                transform.rotation = Quaternion.LookRotation(movePlayer);                       //rotate player in the direction of movement
                playerAnim.SetFloat("Speed_f", 0.6f);                                           //runnig animation will played
                playerAnim.SetBool("Static_b", true);

            }
            else
            {
                playerAnim.SetFloat("Speed_f", 0.1f);                                          //if not running or moving player animation will set back to idle
                playerAnim.SetBool("Static_b", false);
            }
        }
    }
    //Throw Bombs
    private void ThrowBasicBomb()
    {
        if (score.isGameOver != true && spawnManager.isGameEnd != true)             //if condition satisfy 
        {
            if (Input.GetKeyDown(KeyCode.Space) && isDouble != true)                // if player press space and didnt have multiple ball ability
            {
                lowThrow = false;
                Instantiate(bombPrefab, bombPos.position, bombPrefab.transform.rotation);// spawn bomb
                playerAnim.SetInteger("newWeapon", 1);                                   //throwing animation    
                audioManager.playAudio.PlayOneShot(audioManager.throwSound, 1f);         //throwing sound will play
            }
            else if (Input.GetKeyDown(KeyCode.Space) && isDouble != false)               //if player press space key and has special ability of multi ball
            {
                lowThrow = false;
                Instantiate(bombPrefab, bombPosLeft.position, bombPrefab.transform.rotation);//then multi ball while spawn 
                Instantiate(bombPrefab, bombPosRight.position, bombPrefab.transform.rotation);
                audioManager.playAudio.PlayOneShot(audioManager.throwSound, 1f);             //with audio 
                playerAnim.SetInteger("newWeapon", 1);                                       //andthrowing animation
            }
            else
            {
                playerAnim.SetInteger("newWeapon", 0);                                       //if not throwing then animation will not play
            }
        }

    }
    //not throwing but placing bomb
    private void PlacingBasicBomb()
    {
        if (score.isGameOver != true && spawnManager.isGameEnd != true)                    //if true
        {

            if (Input.GetKeyDown(KeyCode.C) && isDouble != true)                          // if C is press and dont have multoball abilty
            {
                lowThrow = true;
                Instantiate(bombPrefab, bombPos.position, bombPrefab.transform.rotation);// single ball place 
                playerAnim.SetInteger("placeWeaponInt", 1);                              //placing animation
                audioManager.playAudio.PlayOneShot(audioManager.throwSound, 1f);         //with sound



            }
            else if (Input.GetKeyDown(KeyCode.C) && isDouble != false)                   // if C is press and have multiball abilty then 
            {
                lowThrow = true;
                Instantiate(bombPrefab, bombPosLeft.position, bombPrefab.transform.rotation);
                Instantiate(bombPrefab, bombPosRight.position, bombPrefab.transform.rotation);//multi ball spawn
                playerAnim.SetInteger("placeWeaponInt", 1);                                   //with animation 
                audioManager.playAudio.PlayOneShot(audioManager.throwSound, 1f);              //amd sound

            }
            else
            {
                playerAnim.SetInteger("placeWeaponInt", 0);                                   //if not placeing the not animation
            }
        }
    }
    //spawn mini bomb
    private void MineBomb()
    {
        if (score.isGameOver != true && spawnManager.isGameEnd != true)
        {

            if (Input.GetKeyDown(KeyCode.V))                                                    // if V press
            {
                if (score.canMine != false)
                {
                    Instantiate(minePrefab, bombPos.position, minePrefab.transform.rotation); // then mine place
                    audioManager.playAudio.PlayOneShot(audioManager.throwSound, 1f);          //with sound
                    score.CountMines();                                                       //and count mines
                }
            }
        }
    }
    private void StickyBomb()
    {
        if (score.isGameOver != true && spawnManager.isGameEnd != true)
        {
            if (Input.GetKeyDown(KeyCode.F))                                                            //if F is press
            {
                if (score.canStick != false)
                {
                    Instantiate(stickyBombPrefab, bombPos.position, bombPrefab.transform.rotation);     //then it spawn 
                    audioManager.playAudio.PlayOneShot(audioManager.throwSound, 1f);                    //with sound and animation
                    playerAnim.SetInteger("newWeaponInt", 1);
                    score.CountStickyBomb();
                }
            }
            else
            {
                playerAnim.SetInteger("newWeaponInt", 0);
            }
        }
    }
    //these trigger are used to collect collectable also play sound while collecting and destroy after it collect
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("DoubleBomb"))
        {
            isDouble = true;
            StartCoroutine(DoubleBomb());
            audioManager.playAudio.PlayOneShot(audioManager.collectSound, 1f);
            Destroy(other.gameObject);
        }
        if (other.gameObject.CompareTag("Mine"))
        {
            score.MineBomb();
            audioManager.playAudio.PlayOneShot(audioManager.collectSound, 1f);
            Destroy(other.gameObject);
        }
        if (other.gameObject.CompareTag("Stick"))
        {
            score.StickyBomb();
            audioManager.playAudio.PlayOneShot(audioManager.collectSound, 1f);
            Destroy(other.gameObject);
        }
        if (other.gameObject.CompareTag("Life"))
        {
            score.AddLife();
            audioManager.playAudio.PlayOneShot(audioManager.collectSound, 1f);
            Destroy(other.gameObject);
        }
    }
    //coroutine used to make delay in multiball case for 10 second
    private IEnumerator DoubleBomb()
    {

        yield return new WaitForSeconds(10f);
        isDouble = false;
    }
    //detect collision with collectable
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("MineBomb"))
        {
            score.LostLife();
            Destroy(collision.gameObject);

        }
        if (collision.gameObject.CompareTag("StickyBomb"))
        {
            collision.gameObject.transform.SetParent(transform);
        }
    }
}
