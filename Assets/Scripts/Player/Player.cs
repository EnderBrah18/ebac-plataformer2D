using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Player : MonoBehaviour
{
    public Rigidbody2D myrigibody;
    public HealthBase healthBase;
    [Header("Setup")]
    public SOPlayer soPlayerSetup;

    //public Animator animator;


    private float _currentSpeed;
    public bool isJumping;

    private Animator _currentPlayer;


    private void Awake()
    {
        if (healthBase != null)
        {
            healthBase.OnKill += OnPlayerDeath;
        }

        _currentPlayer = Instantiate(soPlayerSetup.player, transform);
        var gunBase = _currentPlayer.GetComponentInChildren<GunBase>();
        gunBase.playerSideReference = transform;
    }
    private void OnPlayerDeath()
    {
        healthBase.OnKill -= OnPlayerDeath;

        _currentPlayer.SetTrigger(soPlayerSetup.triggerDeath);
    }

    public void Update()
    {
        HandleJump();
        HandleMovement();


    }

    private void HandleMovement()
    {
        if (Input.GetKey(KeyCode.LeftControl))
        {
            _currentSpeed = soPlayerSetup.speedRun;
            _currentPlayer.speed = 2;
        }

        else
        {
            _currentSpeed = soPlayerSetup.speed;
            _currentPlayer.speed = 1;

        }
        

        if (Input.GetKey(KeyCode.LeftArrow))
        {
            //myrigibody.MovePosition(myrigibody.position - velocity * Time.deltaTime);
            myrigibody.velocity = new Vector2(-_currentSpeed, myrigibody.velocity.y);
            if (myrigibody.transform.localScale.x != -1)
            {
                myrigibody.transform.DOScaleX(-1, soPlayerSetup.playerSwipeDuration);
            }
            _currentPlayer.SetBool(soPlayerSetup.boolRun, true);
        }
        else if (Input.GetKey(KeyCode.RightArrow))
        {
            //myrigibody.MovePosition(myrigibody.position + velocity * Time.deltaTime);
            myrigibody.velocity = new Vector2(_currentSpeed, myrigibody.velocity.y);
            if (myrigibody.transform.localScale.x != 1)
            {
                myrigibody.transform.DOScaleX(1, soPlayerSetup.playerSwipeDuration);
            }
            _currentPlayer.SetBool(soPlayerSetup.boolRun, true);
        }
        else
        {
            _currentPlayer.SetBool(soPlayerSetup.boolRun, false);
        }


        if (myrigibody.velocity.x > 0)
        {
            myrigibody.velocity += soPlayerSetup.friction;
        }
        else if (myrigibody.velocity.x < 0)
        {
            myrigibody.velocity -= soPlayerSetup.friction;
        }
    }

    private void HandleJump()
    {
        if (Input.GetKeyDown(KeyCode.Space) && !isJumping)
        {
            myrigibody.velocity = Vector2.up * soPlayerSetup.forceJump;
            isJumping = true;
            

            HandleScaleJump();

            DOTween.Kill(myrigibody.transform);
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Ground"))
        {
            isJumping = false;
        }
    }

    private void HandleScaleJump()
    {
        myrigibody.transform.DOScaleY(soPlayerSetup.jumpScaleY, soPlayerSetup.animationDuration).SetLoops(2, LoopType.Yoyo).SetEase(soPlayerSetup.ease);
        

        if (myrigibody.transform.localScale.x != -1)
        {
            myrigibody.transform.DOScaleX(soPlayerSetup.jumpScaleX, soPlayerSetup.animationDuration).SetLoops(2, LoopType.Yoyo).SetEase(soPlayerSetup.ease);
        }
        else myrigibody.transform.DOScaleX(-soPlayerSetup.jumpScaleX, soPlayerSetup.animationDuration).SetLoops(2, LoopType.Yoyo).SetEase(soPlayerSetup.ease);
       


    }

    public void DestroyMe()
    {
        Destroy(gameObject);
    }
}
