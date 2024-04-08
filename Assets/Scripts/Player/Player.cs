using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Player : MonoBehaviour
{
    public Rigidbody2D myrigibody;
    public HealthBase healthBase;
    
    [Header("Speed setup")]
    public Vector2 friction = new Vector2(.1f, 0);
    
    public float speed;
    public float speedRun;
    public float forceJump = 2;

    [Header("Animation setup")]
    public float jumpScaleY = 1.5f;
    public float jumpScaleX = .7f;
    public float animationDuration = .3f;
    public Ease ease = Ease.OutBack;


    [Header("Animation player")]
    public string boolRun = "Run";
    public string triggerDeath = "Death";
    public Animator animator;
    public float playerSwipeDuration = .1f;


    private float _currentSpeed;
    public bool isJumping;


    private void Awake()
    {
        if (healthBase != null)
        {
            healthBase.OnKill += OnPlayerDeath;
        }
    }
    private void OnPlayerDeath()
    {
        healthBase.OnKill -= OnPlayerDeath;

        animator.SetTrigger(triggerDeath);
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
            _currentSpeed = speedRun;
            animator.speed = 2;
        }

        else
        {
            _currentSpeed = speed;
            animator.speed = 1;

        }
        

        if (Input.GetKey(KeyCode.LeftArrow))
        {
            //myrigibody.MovePosition(myrigibody.position - velocity * Time.deltaTime);
            myrigibody.velocity = new Vector2(-_currentSpeed, myrigibody.velocity.y);
            if (myrigibody.transform.localScale.x != -1)
            {
                myrigibody.transform.DOScaleX(-1, playerSwipeDuration);
            }
            animator.SetBool(boolRun, true);
        }
        else if (Input.GetKey(KeyCode.RightArrow))
        {
            //myrigibody.MovePosition(myrigibody.position + velocity * Time.deltaTime);
            myrigibody.velocity = new Vector2(_currentSpeed, myrigibody.velocity.y);
            if (myrigibody.transform.localScale.x != 1)
            {
                myrigibody.transform.DOScaleX(1, playerSwipeDuration);
            }
            animator.SetBool(boolRun, true);
        }
        else
        {
            animator.SetBool(boolRun, false);
        }


        if (myrigibody.velocity.x > 0)
        {
            myrigibody.velocity += friction;
        }
        else if (myrigibody.velocity.x < 0)
        {
            myrigibody.velocity -= friction;
        }
    }

    private void HandleJump()
    {
        if (Input.GetKeyDown(KeyCode.Space) && !isJumping)
        {
            myrigibody.velocity = Vector2.up * forceJump;
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
        myrigibody.transform.DOScaleY(jumpScaleY, animationDuration).SetLoops(2, LoopType.Yoyo).SetEase(ease);
        

        if (myrigibody.transform.localScale.x != -1)
        {
            myrigibody.transform.DOScaleX(.7f, animationDuration).SetLoops(2, LoopType.Yoyo).SetEase(ease);
        }
        else myrigibody.transform.DOScaleX(-.7f, animationDuration).SetLoops(2, LoopType.Yoyo).SetEase(ease);
       


    }

    public void DestroyMe()
    {
        Destroy(gameObject);
    }
}
