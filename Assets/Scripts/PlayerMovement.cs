using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerMovement : MonoBehaviour
{
    private Rigidbody2D rb;
    private Animator animator;
    private BoxCollider2D coll;

    [SerializeField] private LayerMask ableToJumpOnGround;

    //private bool isGrounded;
    private bool isFacingRight;
    [SerializeField] private float mSpeed = 7f;
    [SerializeField] private float jForce = 14f;
    private float xDirection;
    private enum stateOfAnimation {idle, running, jumping, falling};

    // Start is called before the first frame update
    private void Start()
    {
        isFacingRight = true;
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        coll = GetComponent<BoxCollider2D>();
    }
    // Check if Player has Collided With The Ground
    /*void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Ground")
        {
            isGrounded = true;
        }
    }*/
    private void Flip()
    {
        isFacingRight = !isFacingRight;
        Vector3 localScale = transform.localScale;
        localScale.x *= -1f;
        transform.localScale = localScale;
    }
    private bool isGrounded()
    {
        return Physics2D.BoxCast(coll.bounds.center, coll.bounds.size, 0f, Vector2.down, .1f, ableToJumpOnGround);
    }


    // Update is called once per frame
    private void Update()
    {
        // Get the value of the x position
        xDirection = Input.GetAxisRaw("Horizontal");
        // trigger running (this will keep the charachter idle if he didnt move )
        rb.velocity = new Vector2(xDirection * mSpeed, rb.velocity.y);
        // Check if the Player Jumped
        if (isGrounded() && Input.GetButtonDown("Jump"))
        {
            // make the player jump
            rb.velocity = new Vector2(rb.velocity.x, jForce);
        }
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            SceneManager.LoadScene("Menu");
        }
        UpdatePlayerAnimation();
    }
    // The Update method got messy so i figured i will make a seperate function for the animation
    private void UpdatePlayerAnimation()
    {
        /* i made alot of changes to this method and to the whole script, 
        because it was messy and i figured out enums will make the code much more clean!
        Just thought i'll leave a note here lol */

        stateOfAnimation stateofanim;
        // Check if the Player has moved to trigger the running animation
        if (xDirection != 0)
        {
            stateofanim = stateOfAnimation.running;
        }
        else
        {
            stateofanim = stateOfAnimation.idle;
        }
        // flip the player sprite basically
        if (!isFacingRight && xDirection > 0 || isFacingRight && xDirection < 0)
        {
            Flip();
        }
        if (rb.velocity.y > .1f)
        {
            stateofanim = stateOfAnimation.jumping;
        }
        else if (rb.velocity.y < -.1f)
        {
            stateofanim = stateOfAnimation.falling;
        }
        animator.SetInteger("StateOfAnimation", (int)stateofanim);

    }
}
