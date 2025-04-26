using UnityEngine;
using TMPro; // Necesario para usar TextMeshPro

public class ScoreManager : MonoBehaviour
{
    public int score = 0; // Variable para almacenar el puntaje
    public TextMeshProUGUI scoreText; // Referencia al objeto de texto de la UI

    void Start()
    {
        UpdateScoreText(); // Actualiza el texto al inicio del juego
    }

    public void AddPoints(int points)
    {
        score += points; // Suma puntos
        UpdateScoreText(); // Actualiza el texto de los puntos
    }

    void UpdateScoreText()
    {
        scoreText.text = "Puntos: " + score.ToString(); // Muestra los puntos en el texto
    }
}

