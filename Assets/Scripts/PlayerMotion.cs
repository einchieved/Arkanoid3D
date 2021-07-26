using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMotion : MonoBehaviour
{
    public float speed;
    public Transform playArea;
    public GameObject ball;
    public GameObject spawner;
    public UnityEngine.UI.Text scoreCount;
    public UnityEngine.UI.Text topCount;
    public UnityEngine.UI.Text liveCount;
    public UnityEngine.UI.Text roundCount;
    public GameObject startButton;
    public GameObject gameOverText;

    public int remainingBalls;
    private int startBalls;
    private int totalPoints = 0;
    private int activeBalls = 0;
    private bool isNextBallBonus = false;

    private int numberOfBlocks;
    private int multiplier = 1;

    private bool isPaddleBig = false;
    private float paddleBigRemainingTime = 0;
    private Vector3 paddleInitialSize;

    private List<GameObject> balls;

    // Start is called before the first frame update
    void Start()
    {
        startBalls = remainingBalls;
        paddleInitialSize = transform.localScale;
    }

    // Update is called once per frame
    void Update()
    {
        if (isPaddleBig)
        {
            paddleBigRemainingTime -= Time.deltaTime;
            if (paddleBigRemainingTime <= 0)
            {
                isPaddleBig = false;
                transform.localScale -= new Vector3(2, 0, 0);
                paddleBigRemainingTime = 0;
            }
        }

        float playAreaSize = playArea.localScale.x * 10;
        float paddleSize = transform.localScale.x * 1;
        float maxX = 0.5f * playAreaSize - 0.5f * paddleSize;

        float dir = Input.GetAxis("Horizontal");
        float newX = transform.position.x + Time.deltaTime * speed * dir;
        float clampedX = Mathf.Clamp(newX, -maxX, maxX);

        transform.position = new Vector3(clampedX, transform.position.y, transform.position.z);
    }

    void OnTriggerEnter(Collider other)
    {
        switch (other.name)
        {
            case "ExtendedPaddle(Clone)":
                if (!isPaddleBig)
                {
                    transform.localScale += new Vector3(2, 0, 0);
                    isPaddleBig = true;
                }
                paddleBigRemainingTime += 10;
                Destroy(other.gameObject);
                break;
            case "MultiBall(Clone)":
                SpawnBall(true, false);
                Destroy(other.gameObject);
                break;
        }
    }

    public void StartGame()
    {
        ResetPaddle();
        balls = new List<GameObject>();
        startButton.SetActive(false);
        gameOverText.SetActive(false);
        numberOfBlocks = spawner.GetComponent<SpawnBlocks>().FirstRound();
        multiplier = 1;
        UpdateRoundCount();
        UpdateRemainingBalls();
        SpawnBall(false, true);
    }

    private void SpawnBall(bool bonus, bool delay)
    {
        isNextBallBonus = bonus;
        if (delay)
        {
            Invoke(nameof(CreateBall), 3);
        }
        else
        {
            CreateBall();
        }
    }

    private void CreateBall()
    {
        activeBalls++;
        if (!isNextBallBonus)
        {
            remainingBalls--;
        }
        GameObject newBall = Instantiate(ball, new Vector3(transform.position.x /*+ 1*/, transform.position.y, transform.position.z + 1.5f), Quaternion.identity);
        newBall.GetComponent<BallScript>().PlayerScript = GetComponent<PlayerMotion>();
        balls.Add(newBall);
        UpdateRemainingBalls();
    }

    public void AddPoints(int points)
    {
        UpdateTotalPoints(totalPoints + points * multiplier);
        if (--numberOfBlocks <= 0)
        {
            balls.ForEach(item =>
            {
                if (item != null)
                {
                    Destroy(item);
                }
            });
            numberOfBlocks = spawner.GetComponent<SpawnBlocks>().NextRound();
            remainingBalls += activeBalls; // add a ball for each ball kept in the round. One ball is the ball for winning this round, the rest are the ones from power ups
            activeBalls = 0;
            multiplier++;
            UpdateRoundCount();
            UpdateRemainingBalls();
            ResetPaddle();
            SpawnBall(true, true); // give the player the destryoed ball back
        }
    }

    public void LoseBall()
    {
        activeBalls--;
        if (activeBalls < 1 && remainingBalls > 0)
        {
            SpawnBall(false, true);
        }
        else if (activeBalls <= 0 && remainingBalls <= 0)
        {
            multiplier = 1;
            remainingBalls = startBalls;
            UpdateTopCount();
            UpdateTotalPoints(0);
            startButton.SetActive(true);
            gameOverText.SetActive(true);
        }
    }

    private void ResetPaddle()
    {
        transform.localScale = paddleInitialSize;
        isPaddleBig = false;
        paddleBigRemainingTime = 0;
    }

    private void UpdateRemainingBalls()
    {
        liveCount.text = remainingBalls.ToString();
    }

    private void UpdateTopCount()
    {
        int top = int.Parse(topCount.text);
        if (totalPoints > top)
        {
            topCount.text = totalPoints.ToString();
        }
    }

    private void UpdateTotalPoints(int pts)
    {
        totalPoints = pts;
        scoreCount.text = totalPoints.ToString();
    }

    private void UpdateRoundCount()
    {
        roundCount.text = multiplier.ToString();
    }
}
