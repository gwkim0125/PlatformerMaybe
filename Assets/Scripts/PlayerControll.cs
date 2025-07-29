using UnityEngine;

public class PlayerControll : MonoBehaviour
{
    
    private Rigidbody rb;
    public float jumpForce = 20f;
    
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
            gameObject.transform.position += new Vector3(0, 0, -0.01f);
        }
        else if (Input.GetKey(KeyCode.D))
        {
            gameObject.transform.position += new Vector3(0, 0, 0.01f);
        }
        
        

    }
}
