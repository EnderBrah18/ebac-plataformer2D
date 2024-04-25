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

    private Animator _currentPlayer;

    [Header("Jump Collision Check")]
    public Collider2D collide2D;
    public float distToGround;
    public float spaceToGround = .1f;
    public ParticleSystem jumpVFX;


    private void Awake()
    {
        if (healthBase != null)
        {
            healthBase.OnKill += OnPlayerDeath;
        }

        _currentPlayer = Instantiate(soPlayerSetup.player, transform);
        var gunBase = _currentPlayer.GetComponentInChildren<GunBase>();
        gunBase.playerSideReference = transform;

        if (collide2D != null)
        {
            distToGround = collide2D.bounds.extents.y;
        }
    }

    private bool IsGrounded()
    {
        
        return Physics2D.Raycast(transform.position, -Vector2.up, distToGround + spaceToGround);
    }
    


    private void OnPlayerDeath()
    {
        healthBase.OnKill -= OnPlayerDeath;

        _currentPlayer.SetTrigger(soPlayerSetup.triggerDeath);
    }

    public void Update()
    {
        IsGrounded();
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
        if (Input.GetKeyDown(KeyCode.Space) && IsGrounded())
        {
            myrigibody.velocity = Vector2.up * soPlayerSetup.forceJump;
            

            HandleScaleJump();
            PlayJumpVFX();

            DOTween.Kill(myrigibody.transform);
        }
    }

    private void PlayJumpVFX()
    {
        VFXManager.Instance.PlayVFXByType(VFXManager.VFXType.JUMP, transform.position);
        //if(jumpVFX != null) jumpVFX.Play();
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
