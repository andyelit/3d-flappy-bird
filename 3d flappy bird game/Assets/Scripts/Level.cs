using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level : MonoBehaviour
{
    private static Level instance;

    public static Level GetInstance()
    {
        return instance;
    }

    [SerializeField] private const float m_PipeMoveSpeed = 30f;
    [SerializeField] private const float m_MinimumPipeHeight = 7f;
    [SerializeField] private const float m_MaximumPipeHeight = 30f;
    [SerializeField] private const float m_PipeDestroyXPosition = -300f;
    [SerializeField] private const float m_PipeSpawnXPosition = 100f;
    [SerializeField] private const float m_BirdXPosition = 0f;

    private List<Pipe> pipeList;
    private int pipesPassedCount;
    private int pipesSpawned;
    private float pipeSpawnTimer;
    private float pipeSpawnTimerMax;
    private float gapSize;
    private State state;

    private enum State
    {
        WaitingToStart,
        Playing,
        BirdDead,
    }

    public enum Difficulty
    {
        Easy,
        Medium,
        Hard,
        Impossible,
    }

    private void Awake()
    {
        instance = this;
        pipeList = new List<Pipe>();
        pipeSpawnTimerMax = 1f;
        SetDifficulty(Difficulty.Easy);
        state = State.WaitingToStart;
    }

    private void Start()
    {
        Bird.GetInstance().OnDied += Bird_OnDied;
        Bird.GetInstance().OnStartedPlaying += Bird_OnStartedPlaying;
    }

    private void Bird_OnStartedPlaying(object sender, System.EventArgs e)
    {
        state = State.Playing;
    }

    private void Bird_OnDied(object sender, System.EventArgs e)
    {
        state = State.BirdDead;
    }

    private void Update()
    {
        if (state == State.Playing)
        {
            HandlePipeMovement();
            HandlePipeSpawning();
        }
    }

    private void HandlePipeMovement()
    {
        for (int i = 0; i < pipeList.Count; i++)
        {
            Pipe pipe = pipeList[i];

            bool isToTheRightOfBird = pipe.GetXPosition() > m_BirdXPosition;
            pipe.Move();

            if (isToTheRightOfBird && pipe.GetXPosition() <= m_BirdXPosition && pipe.IsBottom())
            {
                pipesPassedCount++;

                SoundManager.GetInstance().PlaySound(SoundManager.Sound.Score);
            }

            if (pipe.GetXPosition() < m_PipeDestroyXPosition)
            {
                pipe.DestroySelf();
                pipeList.Remove(pipe);
                i--;
            }
        }
    }

    private void HandlePipeSpawning()
    {
        pipeSpawnTimer -= Time.deltaTime;

        if (pipeSpawnTimer < 0)
        {
            // Time to spawn another Pipe
            pipeSpawnTimer += pipeSpawnTimerMax;

            //float heightEdgeLimit = 10f;
            //float minHeight = gapSize * .5f + heightEdgeLimit;
            //float totalHeight = m_CameraFOV * 2f;
            //float maxHeight = totalHeight - gapSize * .5f - heightEdgeLimit;

            //float height = UnityEngine.Random.Range(minHeight, maxHeight);
            //CreateGapPipes(height, gapSize, m_PipeSpawnXPosition);

            CreateGapPipes(UnityEngine.Random.Range(m_MinimumPipeHeight, m_MaximumPipeHeight), gapSize, m_PipeSpawnXPosition);
        }
    }

    private void CreateGapPipes(float gapY, float gapSize, float xPosition)
    {
        CreatePipe(gapY - gapSize * .5f, xPosition, true);
        CreatePipe(2f - gapY - gapSize * .5f, xPosition, false);
        pipesSpawned++;
        SetDifficulty(GetDifficulty());
    }

    private void CreatePipe(float height, float xPosition, bool createBottom)
    {
        // Set up Pipe Body
        Transform pipeBody = Instantiate(GameAssets.GetInstance().pfPipe.transform);

        float pipeBodyYPosition;

        if (createBottom)
        {
            pipeBodyYPosition = -UnityEngine.Random.Range(m_MinimumPipeHeight, m_MaximumPipeHeight);
        }
        else
        {
            pipeBodyYPosition = +UnityEngine.Random.Range(m_MinimumPipeHeight, m_MaximumPipeHeight);

            pipeBody.localScale = new Vector3(1, -1, 1);
        }

        pipeBody.position = new Vector3(xPosition, pipeBodyYPosition, 0f);

        Pipe pipe = new Pipe(pipeBody, createBottom);

        pipeList.Add(pipe);
    }


    private void SetDifficulty(Difficulty difficulty)
    {
        switch (difficulty)
        {
            case Difficulty.Easy:
                gapSize = 50f;
                pipeSpawnTimerMax = 1.4f;
                break;

            case Difficulty.Medium:
                gapSize = 40f;
                pipeSpawnTimerMax = 1.3f;
                break;

            case Difficulty.Hard:
                gapSize = 33f;
                pipeSpawnTimerMax = 1.1f;
                break;

            case Difficulty.Impossible:
                gapSize = 24f;
                pipeSpawnTimerMax = 1.0f;
                break;
        }
    }

    private Difficulty GetDifficulty()
    {
        if (pipesSpawned >= 24) return Difficulty.Impossible;
        if (pipesSpawned >= 12) return Difficulty.Hard;
        if (pipesSpawned >= 5) return Difficulty.Medium;
        return Difficulty.Easy;
    }

    public int GetPipesPassedCount()
    {
        return pipesPassedCount;
    }

    public class Pipe
    {
        private Transform pipeTransform;
        private bool isBottom;

        public Pipe(Transform pipeTransform, bool isBottom)
        {
            this.pipeTransform = pipeTransform;
            this.isBottom = isBottom;
        }

        public void Move()
        {
            pipeTransform.position += new Vector3(-1, 0, 0) * m_PipeMoveSpeed * Time.deltaTime;
        }

        public float GetXPosition()
        {
            return pipeTransform.position.x;
        }

        public bool IsBottom()
        {
            return isBottom;
        }

        public void DestroySelf()
        {
            Destroy(pipeTransform.gameObject);
        }
    }
}
