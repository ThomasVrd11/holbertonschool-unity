using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

public class PlayerController : MonoBehaviour
{

    #region Variables

    [SerializeField] private float speed = 5.0f;
    [SerializeField] Rigidbody rb;
    public Text WinLoseText;
    public Image WinLoseBG;
    public Text healthText;
    public Text scoreText;
    public int health = 5;
    private int score = 0;

    #endregion
    #region Methods
    void FixedUpdate()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        Vector3 move = new Vector3(horizontalInput, 0, verticalInput);
        rb.linearVelocity = new Vector3(horizontalInput * speed, rb.linearVelocity.y, verticalInput * speed);
    }

    void Update()
    {
        if (Input.GetKey("escape"))
        {
            SceneManager.LoadScene("menu");
        }
        if (health == 0)
            {
                GameOver();
                StartCoroutine(LoadScene(3));
            }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Pickup"))
        {
            score++;
            SetScoreText();
            other.gameObject.SetActive(false);
        }
        else if (other.gameObject.CompareTag("Trap"))
        {
            health--;
            SetHealthText();
        }
        else if (other.gameObject.CompareTag("Goal"))
        {
            RestartGame();
        }
    }

    private void SetScoreText()
    {
        scoreText.text = $"Score: {score}";
    }

    private void SetHealthText()
    {
        healthText.text = $"Health: {health}";
    }
    
    private IEnumerator LoadScene(float seconds)
    {
        // wait for specified duration
        yield return new WaitForSeconds(seconds);
        // reload the active scene
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    private void GameOver()
    {   
        WinLoseBG.gameObject.SetActive(true);
        WinLoseText.gameObject.SetActive(true);
        WinLoseText.text = "Game Over!";
        WinLoseBG.color = Color.red;
    }
    private void RestartGame()
    {
        WinLoseBG.gameObject.SetActive(true);
            WinLoseText.gameObject.SetActive(true);
            WinLoseText.text = "You Win!";
            WinLoseBG.color = Color.green;
            WinLoseText.color = Color.black;
        StartCoroutine(LoadScene(3));
    }
    #endregion
}

