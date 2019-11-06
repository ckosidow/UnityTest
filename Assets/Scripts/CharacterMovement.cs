using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMovement : MonoBehaviour
{
    public CharacterController2D controller;
    public Animator animator;
    public float runSpeed = 160f;
    float horizontalMove = 0f;
    bool jump = false;
    bool crouch = false;
    int jumpsUsed = 0;

    // Update is called once per frame
    void Update()
    {
        horizontalMove = Input.GetAxisRaw("Horizontal") * runSpeed;

        animator.SetFloat("Speed", Mathf.Abs(controller.m_Rigidbody2D.velocity.x));

        if (Input.GetButtonDown("Jump"))
        {
            if (controller.m_Grounded)
            {
                animator.SetBool("IsRising", true);
                jump = true;
            } else
            {
                if (jumpsUsed < controller.extraJumpsAllowed)
                {
                    animator.SetBool("IsRising", true);
                    jump = true;
                    jumpsUsed++;
                }
            }
        }

        if (Input.GetButtonDown("Crouch"))
        {
            crouch = true;
            animator.SetBool("IsCrouching", true);
        } else if (Input.GetButtonUp("Crouch"))
        {
            crouch = false;
            animator.SetBool("IsCrouching", false);
        }
    }

    public void OnLanding()
    {
        animator.SetBool("IsRising", false);
        animator.SetBool("IsFalling", false);

        jumpsUsed = 0;
    }

    void FixedUpdate()
    {
        controller.Move(horizontalMove * Time.fixedDeltaTime, crouch, jump);
        jump = false;

        if (controller.m_Rigidbody2D.velocity.y < 0 && !controller.m_Grounded)
        {
            animator.SetBool("IsFalling", true);
            animator.SetBool("IsRising", false);
        } else if (controller.m_Rigidbody2D.velocity.y > 0 && !controller.m_Grounded)
        {
            animator.SetBool("IsRising", true);
        } else
        {
            animator.SetBool("IsFalling", false);
            animator.SetBool("IsRising", false);
        }
    }
}
