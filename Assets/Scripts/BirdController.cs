using UnityEngine;

public class BirdController : MonoBehaviour
{
    // Untere und obere Fluggrenze des Vogels
    public float minHeight = 1.5f;
    public float neutralHeight = 4.5f;
    public float maxHeight = 14f;

    // Verbindung zu den anderen Systemen:
    // MeditationManager steuert, ob die Session läuft.
    // BreathDataManager liefert den aktuellen Atemwert.
    public MeditationManager meditationManager;
    public BreathDataManager breathDataManager;

    void Update()
    {
        // Vor dem Start der Session soll der Vogel nicht bewegt werden
        if (!meditationManager.sessionRunning)
        {
            return;
        }

        // Aktuellen Atemwert aus dem BreathDataManager holen
        float breathValue =
            breathDataManager.currentBreathValue;

        // Zielhöhe des Vogels berechnen
        float targetHeight;

        // Positiver Atemwert bedeutet Einatmen:
        // Der Vogel steigt von der neutralen Höhe nach oben.
        if (breathValue >= 0f)
        {
            targetHeight = Mathf.Lerp(
                neutralHeight,
                maxHeight,
                breathValue
            );
        }
        else
        {
            // Negativer Atemwert bedeutet Ausatmen:
            // Der Vogel sinkt von der neutralen Höhe nach unten.
            targetHeight = Mathf.Lerp(
                neutralHeight,
                minHeight,
                -breathValue
            );
        }

        // Sicherstellen, dass der Vogel niemals
        // unter minHeight oder über maxHeight fliegt
        targetHeight = Mathf.Clamp(
            targetHeight,
            minHeight,
            maxHeight
        );

        // Aktuelle Position des Vogels übernehmen
        Vector3 position = transform.position;

        // Nur die Y-Position (Höhe) verändern
        position.y = targetHeight;

        // Neue Position auf den Vogel anwenden
        transform.position = position;
    }
}

