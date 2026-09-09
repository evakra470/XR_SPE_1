using UnityEngine;

public class GroundManager : MonoBehaviour
{
    // Referenzen auf die drei Bodenplatten in der Szene
    public Transform ground01;
    public Transform ground02;
    public Transform ground03;

    // Geschwindigkeit, mit der sich der Boden nach links bewegt
    public float moveSpeed = 5f;

    // Prüft, ob die Atemsession gerade läuft
    public MeditationManager meditationManager;

    void Update()
    {
        // Vor dem Start der Session soll sich der Boden nicht bewegen
        if (!meditationManager.sessionRunning)
        {
            return;
        }

        // Bewegung nach links berechnen
        // Time.deltaTime sorgt für eine gleichmäßige Bewegung
        // unabhängig von der Bildrate
        Vector3 movement =
            Vector3.left * moveSpeed * Time.deltaTime;

        // Alle drei Bodenplatten gleichzeitig bewegen
        ground01.position += movement;
        ground02.position += movement;
        ground03.position += movement;

        // Zuerst die aktuell am weitesten links stehende
        // Bodenplatte bestimmen
        Transform leftmost = ground01;

        if (ground02.position.x < leftmost.position.x)
        {
            leftmost = ground02;
        }

        if (ground03.position.x < leftmost.position.x)
        {
            leftmost = ground03;
        }

        // Danach die aktuell am weitesten rechts stehende
        // Bodenplatte bestimmen
        Transform rightmost = ground01;

        if (ground02.position.x > rightmost.position.x)
        {
            rightmost = ground02;
        }

        if (ground03.position.x > rightmost.position.x)
        {
            rightmost = ground03;
        }

        // Renderer der linken und rechten Bodenplatte holen
        // Der Renderer liefert die tatsächlichen sichtbaren Abmessungen
        Renderer leftRenderer =
            leftmost.GetComponent<Renderer>();

        Renderer rightRenderer =
            rightmost.GetComponent<Renderer>();

        // Breite der linken Bodenplatte bestimmen
        float leftWidth =
            leftRenderer.bounds.size.x;

        // Prüfen, ob die linke Bodenplatte weit genug
        // aus dem sichtbaren Bereich herausgelaufen ist
        if (leftRenderer.bounds.max.x < -100f)
        {
            // Die Bodenplatte direkt hinter der rechten
            // Bodenplatte wieder einsetzen
            float newX =
                rightRenderer.bounds.max.x
                + leftWidth / 2f;

            // Nur die X-Position verändern.
            // Y und Z bleiben unverändert.
            leftmost.position = new Vector3(
                newX,
                leftmost.position.y,
                leftmost.position.z
            );
        }
    }
}


