using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class MeditationManager : MonoBehaviour
{
    // Dauer einer Atemsession in Sekunden
    public float sessionLength = 30f;

    // Textfeld für die Anzeige der verbleibenden Zeit
    public TextMeshProUGUI timerText;

    // Start- und Endbildschirm der Session
    public GameObject startScreen;
    public GameObject endScreen;

    // Buttons für Neustart und Beenden
    public GameObject restartButton;
    public GameObject exitButton;

    // Animator des Vogels
    public Animator birdAnimator;

    // Gibt an, ob die Session aktuell läuft
    public bool sessionRunning = false;

    // Verbleibende Zeit der aktuellen Session
    private float remainingTime;

    void Start()
    {
        // Session am Anfang noch nicht starten
        sessionRunning = false;

        // Startbildschirm anzeigen und Endbildschirm verstecken
        startScreen.SetActive(true);
        endScreen.SetActive(false);
        timerText.gameObject.SetActive(false);

        // Buttons am Anfang verstecken
        restartButton.SetActive(false);
        exitButton.SetActive(false);

        // Vogelanimation am Anfang ausschalten
        birdAnimator.enabled = false;

        // Timer auf die eingestellte Sessiondauer setzen
        remainingTime = sessionLength;
        UpdateTimer();
    }

    void Update()
    {
        // Wenn die Session nicht läuft, nichts tun
        if (!sessionRunning)
        {
            return;
        }

        // Vergangene Zeit von der verbleibenden Zeit abziehen
        remainingTime -= Time.deltaTime;

        // Prüfen, ob die Session beendet ist
        if (remainingTime <= 0)
        {
            remainingTime = 0;
            UpdateTimer();

            EndSession();

            return;
        }

        // Timer während der laufenden Session aktualisieren
        UpdateTimer();
    }

    void UpdateTimer()
    {
        // Minuten und Sekunden aus der verbleibenden Zeit berechnen
        int minutes = Mathf.FloorToInt(remainingTime / 60);
        int seconds = Mathf.FloorToInt(remainingTime % 60);

        // Zeit im Format MM:SS anzeigen
        timerText.text = string.Format(
            "{0:00}:{1:00}",
            minutes,
            seconds
        );
    }

    public void StartSession()
    {
        // Startbildschirm ausblenden und Endbildschirm verstecken
        startScreen.SetActive(false);
        endScreen.SetActive(false);

        // Timer anzeigen
        timerText.gameObject.SetActive(true);
         
        // Buttons anzeigen
        restartButton.SetActive(true);
        exitButton.SetActive(true);

        // Timer auf die volle Sessiondauer zurücksetzen
        remainingTime = sessionLength;
        UpdateTimer();

        // Session starten
        sessionRunning = true;

        // Vogelanimation starten
        birdAnimator.enabled = true;
    }

    void EndSession()
    {
        // Session beenden
        sessionRunning = false;

        // Endbildschirm anzeigen und Timer verstecken
        endScreen.SetActive(true);
        timerText.gameObject.SetActive(false);

        // Vogelanimation stoppen
        birdAnimator.enabled = false;

        Debug.Log("Atemsession beendet.");
    }

    public void RestartSession()
    {
        // Aktuelle Szene neu laden und dadurch
        // die Session komplett zurücksetzen
        SceneManager.LoadScene(
            SceneManager.GetActiveScene().name
        );
    }

    public void ExitGame()
    {
        // Anwendung beenden
        Application.Quit();
    }
}

