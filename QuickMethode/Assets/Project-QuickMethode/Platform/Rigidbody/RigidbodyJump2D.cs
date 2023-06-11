using QuickMethode;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RigidbodyJump2D : MonoBehaviour
{
    [SerializeField] private float jumpForce = 10;
    [SerializeField] private float jumpFrameMax = 18;
    [SerializeField] private float jumpFrameMin = 0;
    [SerializeField] private float jumpFrameStep = 3;

    [SerializeField] private float jumpTime;
    private float jumpTimeCounter;

    private bool isGrounded;
    private bool isJumping;

    private Vector2 PosFoot => QCollider2D.GetBorderPos(m_collider, Direction.Down);

    private Collider2D m_collider;
    private Rigidbody2D rb;

    private void Start()
    {
        m_collider = GetComponent<Collider2D>();
        rb = GetComponent<Rigidbody2D>();

        StartCoroutine(aaa());
    }

    //private void FixedUpdate()
    //{
    //    //X Move
    //}

    //private void Update()
    //{
    //    isGrounded = QCast.GetOverlapCircle2D(PosFoot, 0.1f) != null;

    //    if (isGrounded && Input.GetKeyDown(KeyCode.UpArrow))
    //    {
    //        isJumping = true;
    //        jumpTimeCounter = jumpTime;
    //        rb.velocity = Vector2.up * jumpForce;
    //    }

    //    if (Input.GetKey(KeyCode.UpArrow) && isJumping)
    //    {
    //        if (jumpTimeCounter > 0)
    //        {
    //            rb.velocity = Vector2.up * jumpForce;
    //            jumpTimeCounter -= Time.deltaTime;
    //        }
    //        else
    //        {
    //            isJumping = false;
    //        }
    //    }

    //    if (Input.GetKeyUp(KeyCode.UpArrow))
    //    {
    //        isJumping = false;
    //    }
    //}

    private IEnumerator aaa()
    {
        do
        {
            yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.UpArrow));

            int JumpFrame = 0;

            while(Input.GetKey(KeyCode.UpArrow) && JumpFrame < jumpFrameMax)
            {
                if (JumpFrame < jumpFrameMax)
                    JumpFrame++;

                for (int i = 0; i < jumpFrameStep; i++) yield return null;
            }

            for (int Frame = 0; Frame < JumpFrame; Frame++)
            {
                rb.velocity = Vector2.up * jumpForce;
                yield return new WaitForFixedUpdate();
            }
        }
        while (true);
    }
}