﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BusrtController : MonoBehaviour
{
    public GameObject burstEffectPrefab;
    public float jumpForce = 5f;
    public GameObject tapToStartText;
    public GameObject JumpHintText;
    public AudioClip stickCollisionSound;

    private Rigidbody2D rb;
    private bool gameStarted = false;
    private bool isDead = false;

    private bool canJumpFromStick = false;
    private GameObject currentStick = null;
    private AudioSource audioSource;

    private bool pausedAfterFirstStick = false;
    private bool firstStickHit = false;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.isKinematic = true;

        if (tapToStartText != null)
            tapToStartText.SetActive(true);

        if (JumpHintText != null)
            JumpHintText.SetActive(false);

        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        if (isDead) return;

        if (!gameStarted && IsUserInput())
        {
            StartGame();
            Jump(); // Optional: jump to start
        }
        else if (gameStarted && IsUserInput())
        {
            if (pausedAfterFirstStick)
            {
                ResumeAfterFirstStick();
                Jump(); // Resume with jump
            }
            else if (canJumpFromStick)
            {
                Jump();
                canJumpFromStick = false;
            }
        }

        // Clamp position
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

    void ResumeAfterFirstStick()
    {
        pausedAfterFirstStick = false;
        rb.isKinematic = false;

        if (JumpHintText != null)
            JumpHintText.SetActive(false);
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

                // Play stick sound
                if (audioSource != null && stickCollisionSound != null)
                {
                    audioSource.PlayOneShot(stickCollisionSound);
                }

                // On first stick hit, pause and show hint
                if (!firstStickHit)
                {
                    firstStickHit = true;
                    pausedAfterFirstStick = true;
                    rb.linearVelocity = Vector2.zero;
                    rb.isKinematic = true;

                    if (JumpHintText != null)
                        JumpHintText.SetActive(true);
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