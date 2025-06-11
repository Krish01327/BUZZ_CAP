using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BusrtController : MonoBehaviour
{
    public GameObject burstEffectPrefab;
    public float jumpForce = 5f;
    public GameObject tapToStartText;
    public AudioClip stickCollisionSound; // Add this

    private Rigidbody2D rb;
    private bool gameStarted = false;
    private bool isDead = false;

    private bool canJumpFromStick = false;
    private GameObject currentStick = null;

    private AudioSource audioSource;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.isKinematic = true;

        if (tapToStartText != null)
            tapToStartText.SetActive(true);

        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        if (isDead) return;

        if (!gameStarted && IsUserInput())
        {
            StartGame();
            Jump();
        }
        else if (gameStarted && IsUserInput())
        {
            if (canJumpFromStick)
            {
                Jump();
                canJumpFromStick = false;
            }
        }

        if (gameStarted && transform.position.y > 6f)
        {
            Vector3 clampedPos = transform.position;
            clampedPos.y = 6f;
            transform.position = clampedPos;
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, 0);
        }
    }

    bool IsUserInput()
    {
        return Input.GetMouseButtonDown(0) ||
               (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began);
    }

    void StartGame()
    {
        gameStarted = true;
        rb.isKinematic = false;

        if (tapToStartText != null)
            tapToStartText.SetActive(false);
    }

    void Jump()
    {
        if (rb != null)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, 0);
            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Stick"))
        {
            if (collision.gameObject != currentStick)
            {
                currentStick = collision.gameObject;
                canJumpFromStick = true;

                // Play stick collision sound
                if (audioSource != null && stickCollisionSound != null)
                {
                    audioSource.PlayOneShot(stickCollisionSound);
                }
            }
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("DeadZone") && !isDead)
        {
            isDead = true;

            if (burstEffectPrefab)
            {
                Instantiate(burstEffectPrefab, transform.position, Quaternion.identity);
            }

            gameObject.SetActive(false);
            Invoke("RestartGame", 1f);
        }
    }

    void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
