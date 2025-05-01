using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class CoinManager : MonoBehaviour
{
    public int totalCoins; // Número total de monedas en la escena
    public TextMeshProUGUI gameOverText; // Texto de "Game Over"
    public GameObject restartButton;

    void Start()
    {
        // Encuentra todas las monedas que tengan el tag "Coin"
        totalCoins = GameObject.FindGameObjectsWithTag("Coin").Length;

        // Asegúrate de que el texto de Game Over está oculto al inicio
        if (gameOverText != null)
        {
            gameOverText.gameObject.SetActive(false);
        }
    }

    // Método para llamar cada vez que se recoge una moneda
    public void CoinCollected()
    {
        totalCoins--;

        if (totalCoins <= 0)
        {
            GameOver();
        }
    }

    // Método que muestra el texto de "Game Over"
    void GameOver()
    {
        Debug.Log("!Game Over activado!");

        if (gameOverText != null)
        {
            gameOverText.gameObject.SetActive(true);
        }

        if (restartButton != null)
        {
            restartButton.SetActive(true); // Asegúrate de hacer esto antes de pausar
        }

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        Time.timeScale = 0; // <- Pausa el juego después de mostrar todo

    }


    public void RestartLevel()
    {
        Time.timeScale = 1f;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }


}


