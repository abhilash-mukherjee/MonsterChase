using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Player : MonoBehaviour
{
    public delegate void PlayerDestroyHandler();
    public static event PlayerDestroyHandler OnPlayerDestroyed;
    public delegate void DamageHandler(int health);
    public static event DamageHandler OnDamageDone;
    public delegate void BulletFireHandler(int remainingBullets);
    public static event BulletFireHandler OnBulletFired;

    
    [SerializeField]
    private float speed = 10f;
    [SerializeField]
    private float jumpForce = 5f;
    [SerializeField]
    private float pauseBeforeJumpAudioPlay;
    [SerializeField]
    private float fireForce = 20f;

    [SerializeField]
    private GameObject bulletPrefab;
    private GameObject firePoint;

    private float movementDirection;
    private bool isGrounded = true;
    private bool shouldJump = false;
    private bool shouldFireAnimate = false, shouldFire = false;

    private Animator anim;
    private Rigidbody2D rigidBody2d;
    private Collider2D playerBCollider;
    private SpriteRenderer sr;
    private string WALK_ANIMATION = "Walk", JUMP_ANIMATION = "Jump", FIRE_ANIMATION = "Shoot";
    [SerializeField]
    private Vector3 positionOffsetOnPlayerSpawn;
    private bool flipX = false;

    public static int MaxHealth;
    private int _health;
    public int Health
    {
        get { return _health; }
    }

    private int _bulletCount;
    public static int MaxBulletCount;

    private void OnEnable()
    {
        isGrounded = false;
    }
    private Vector3 cachedPlayerPosition;
    void Awake()
    {
        MaxBulletCount = GameManager.Instance.MaxBulletCountForLevel;
        MaxHealth = GameManager.Instance.MaxHealthForLevel;
        _bulletCount = MaxBulletCount;
        _health = MaxHealth;
        anim = gameObject.GetComponent<Animator>();
        rigidBody2d = gameObject.GetComponent<Rigidbody2D>();
        sr = gameObject.GetComponent<SpriteRenderer>();
        playerBCollider = gameObject.GetComponent<Collider2D>(); 
    }
   

    // Update is called once per frame
    void Update()
    {
        ReadInput();
        PlayerMovement();
        PlayerWalkAnimation();
    }
    private void FixedUpdate()
    {
        PlayerJump();
        PlayerJumpAnimation();
        PlayerFire();
        PlayerFireAnimation();
    }



    private void ReadInput()
    {
        if (anim.GetCurrentAnimatorStateInfo(0).IsName(FIRE_ANIMATION))
        {
            movementDirection = 0;
        }
        else
        {
            movementDirection = Input.GetAxisRaw("Horizontal");
        }

        if (Input.GetButtonDown("Jump") && isGrounded)
            shouldJump = true;

        if(Input.GetButtonDown("Fire1") && !(anim.GetCurrentAnimatorStateInfo(0).IsName(FIRE_ANIMATION)))
        {
            if (EventSystem.current.IsPointerOverGameObject() == false && Time.timeScale != 0)
            {
                shouldFireAnimate = true;
                shouldFire = true;
            }
                    
        }
        if(anim.GetCurrentAnimatorStateInfo(0).IsName(FIRE_ANIMATION))
        {
            shouldFireAnimate = false;
            shouldFire = false;
        }
    }

    private void PlayerJump()
    {
        if(shouldJump == true && isGrounded == true)
        {
            shouldJump = false;
            isGrounded = false;
            FindObjectOfType<AudioManager>().PlaySound("Jump_Start");
            StartCoroutine(PlayJumpAudioAfterPause(pauseBeforeJumpAudioPlay));
            rigidBody2d.AddForce(new Vector2(0f,jumpForce) , ForceMode2D.Impulse);

        }
    }

    private void PlayerJumpAnimation()
    {
        if(isGrounded == false)
        {
            anim.SetBool(WALK_ANIMATION, false);
            anim.SetBool(JUMP_ANIMATION, true);
            if (movementDirection != 0)
            {  
                anim.SetFloat("Speed", 1f);
            }
            else
            {
                anim.SetFloat("Speed", 0f);
            }
        }
        else
        {
            anim.SetBool(JUMP_ANIMATION, false);
        }
    }

    private void PlayerFireAnimation()
    {
        if (shouldFireAnimate == true)
        {
            anim.SetBool(JUMP_ANIMATION, false);
            anim.SetBool(FIRE_ANIMATION, true);
            
        }
        else
            anim.SetBool(FIRE_ANIMATION, false);
    }

    private void PlayerFire()
    {
        if (shouldFire == true)
        {
            if(_bulletCount > 0)
            {
                Debug.Log("Fire");
                firePoint = transform.GetChild(0).gameObject;
                GameObject bullet = Instantiate(bulletPrefab, firePoint.transform.position, Quaternion.identity);
                Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
                if (flipX)
                {
                    rb.velocity = new Vector2(-fireForce, 0f);
                }
                else
                {
                    rb.velocity = new Vector2(fireForce, 0f);
                }
                FindObjectOfType<AudioManager>().PlaySound("Fire");
                _bulletCount--;
                OnBulletFired?.Invoke(_bulletCount);
            }
           
            else if(_bulletCount == 0)
            {
                FindObjectOfType<AudioManager>().PlaySound("EmptyFire");
            }
            shouldFire = false;
            
        }
    }

    private object FindObjetOfType<T>()
    {
        throw new NotImplementedException();
    }

    private void PlayerMovement()
    {
        if (movementDirection>0)
        {
            transform.position += speed * Time.deltaTime * Vector3.right;

        }
        if (movementDirection < 0f)
        {
            transform.position += speed * Time.deltaTime * Vector3.left;
        }
    }

    private void PlayerWalkAnimation()
    {
        if(movementDirection>0)
        {
            anim.SetBool(WALK_ANIMATION, true);
            RotatePlayer(false);
        }
        else if (movementDirection < 0)
        {
            anim.SetBool(WALK_ANIMATION, true);
            RotatePlayer(true);
        }
        else
        {
            anim.SetBool(WALK_ANIMATION, false);
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
        }

        if (collision.collider.gameObject.CompareTag("Enemy"))
        {
            if (_health == 0)
            {
                FindObjectOfType<AudioManager>().PlaySound("PlayerDied");
                Destroy(gameObject);
                OnPlayerDestroyed?.Invoke();
            }

            else
            {           
                Damage(1);
            }

        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Enemy"))
        {
            if (_health == 0)
            {
                FindObjectOfType<AudioManager>().PlaySound("PlayerDied");
                Destroy(gameObject);
                OnPlayerDestroyed?.Invoke();
            }
            else
            {
                Damage(1);
            }
            
        }
    }

    private void Damage(int dmg)
    {
        _health -= dmg;
        FindObjectOfType<AudioManager>().PlaySound("PlayerDied");
        sr.enabled = false;
        cachedPlayerPosition = transform.position;
        GameManager.Instance.PauseGame();
        StartCoroutine(ResumeGameAfterPlayerIsHit());

    }

    IEnumerator ResumeGameAfterPlayerIsHit()
    {
        yield return new WaitForSecondsRealtime(0.5f);
        OnDamageDone?.Invoke(_health);
        Debug.Log($"Health = {_health}");
        Vector3 newPosition = cachedPlayerPosition + positionOffsetOnPlayerSpawn;
        transform.position = newPosition;
        GetComponent<Rigidbody2D>().velocity = new Vector2(0, 0);
        sr.enabled = true;
        GameManager.Instance.ResumeGame();
    }
    private void RotatePlayer(bool shouldRotate)
    {
        if(shouldRotate)
        {
            if(!flipX)
            {
                transform.Rotate(0f, 180f, 0f);
                flipX = true;
            }
            
        }
        else
        {
            if (flipX)
            {
                transform.Rotate(0f, 180f, 0f);
                flipX = false;
            }
        }
    }
    
    IEnumerator PlayJumpAudioAfterPause(float pause)
    {
        yield return new WaitForSeconds(pause);
        FindObjectOfType<AudioManager>().PlaySound("Jump_End");
    }
    
}
