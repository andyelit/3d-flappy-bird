using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bird : MonoBehaviour
{
    private static Bird instance;

    public static Bird GetInstance()
    {
        return instance;
    }

    public event EventHandler OnDied;
    public event EventHandler OnStartedPlaying;

    [SerializeField] private float m_JumpAmount = 60f;
    [SerializeField] private Rigidbody m_Rigidbody;

    private State state;

    private enum State
    {
        WaitingToStart,
        Playing,
        Dead
    }

    private void Awake()
    {
        instance = this;
        m_Rigidbody = GetComponent<Rigidbody>();
        m_Rigidbody.isKinematic = true;
        state = State.WaitingToStart;
    }

    private void Update()
    {
        switch (state)
        {
            case State.WaitingToStart:

                if (GetInput())
                {
                    // Start playing
                    state = State.Playing;

                    m_Rigidbody.isKinematic = false;

                    Jump();

                    if (OnStartedPlaying != null)
                        OnStartedPlaying(this, EventArgs.Empty);
                }

                break;

            case State.Playing:
                if (GetInput())
                {
                    m_Rigidbody.isKinematic = false;
                    Jump();
                }
                break;

            case State.Dead:
                break;

            default:
                break;
        }
    }

    private bool GetInput()
    {
        return Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0) || Input.touchCount > 0;
    }

    private void Jump()
    {
        m_Rigidbody.velocity = Vector2.up * m_JumpAmount;

        SoundManager.GetInstance().PlaySound(SoundManager.Sound.BirdJump);
    }

    private void OnTriggerEnter(Collider collider)
    {
        m_Rigidbody.isKinematic = true;

        SoundManager.GetInstance().PlaySound(SoundManager.Sound.Lose);

        if (OnDied != null)
            OnDied(this, EventArgs.Empty);
    }
}
