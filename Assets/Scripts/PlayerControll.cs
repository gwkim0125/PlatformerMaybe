using System;
using System.Collections;
using UnityEngine;

public class PlayerControll : MonoBehaviour
{

    private Rigidbody rb;
    public float jumpForce = 20f;

    [Header("대시 설정")]
    public float dashDistance = 0.3f; // 대시 거리
    public float dashDuration = 0.2f; // 대시 지속 시간 (빠르게 이동하는 시간)
    public float dashCooldown = 1f; // 대시 쿨타임
    

    private bool isDashing = false; // 현재 대시 중인지 여부
    
    private float nextDashTime = 1f; // 다음 대시 가능한 시간

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Debug.Log("Game Start");
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            rb.linearVelocity = new Vector3(rb.linearVelocity.x, 0, rb.linearVelocity.z);
            Debug.Log(jumpForce);
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse); // Impulse 모드는 즉각적인 힘을 가함
            Debug.Log("점프!");
        }
        else if (Input.GetKeyDown(KeyCode.W))
        {
            transform.position += new Vector3(-3.5f, 0f, 0f);
        }
        else if (Input.GetKeyDown(KeyCode.S))
        {
            transform.position += new Vector3(3.5f, 0, 0);
        }
        else if (Input.GetKey(KeyCode.A))
        {
            gameObject.transform.position += new Vector3(0, 0, -0.02f);
        }
        else if (Input.GetKey(KeyCode.D))
        {
            gameObject.transform.position += new Vector3(0, 0, 0.02f);
        }
        if (Time.time >= nextDashTime && !isDashing) // 쿨타임 중이 아니고 대시 중이 아닐 때만 감지
        {
            if (Input.GetKey(KeyCode.A) && Input.GetKey(KeyCode.LeftShift))
            {
                StartDash(-1);
            }
            else if (Input.GetKey(KeyCode.D) && Input.GetKey(KeyCode.LeftShift))
            {
                StartDash(1);
            }


            
        }


    }

    void StartDash(int direction)
    {
        isDashing = true;
        nextDashTime = Time.time + dashCooldown;
        StartCoroutine(DashCoroutine(direction));
    }
    
     IEnumerator DashCoroutine(int direction)
    {
        // === 이 부분이 변경되었습니다! ===
        // Z축 (앞뒤) 방향으로 대시하도록 transform.forward 사용
        Vector3 dashDirection = transform.forward * direction; 
        // ============================

        float startYVelocity = rb.linearVelocity.y;
        float dashSpeed = dashDistance / dashDuration;

        float timer = 0f;
        while (timer < dashDuration)
        {
            rb.linearVelocity = new Vector3(dashDirection.x * dashSpeed, startYVelocity, dashDirection.z * dashSpeed);
            timer += Time.fixedDeltaTime;
            yield return new WaitForFixedUpdate();
        }

        rb.linearVelocity = new Vector3(0, startYVelocity, 0); 
        isDashing = false;
    }
}
