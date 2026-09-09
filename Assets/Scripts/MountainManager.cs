using UnityEngine;

public class MountainManager : MonoBehaviour
{
    // Referenzen auf die drei Berge in der Szene
    public Transform mountain01;
    public Transform mountain02;
    public Transform mountain03;

    // Geschwindigkeit der Berge und Abstand zwischen ihnen
    public float moveSpeed = 3f;
    public float mountainSpacing = 40f;

    // Prüft, ob die Atemsession gerade läuft
    public MeditationManager meditationManager;

    void Update()
    {
        // Vor dem Start der Session sollen sich die Berge nicht bewegen
        if (!meditationManager.sessionRunning)
        {
            return;
        }

        // Alle Berge gleich schnell nach links bewegen
        MoveMountain(mountain01);
        MoveMountain(mountain02);
        MoveMountain(mountain03);

        // Den jeweils linken Berg wieder nach ganz rechts setzen
        RecycleMountains();
    }

    void MoveMountain(Transform mountain)
    {
        // Bewegt den übergebenen Berg pro Frame nach links.
        // Time.deltaTime sorgt dafür, dass die Bewegung
        // unabhängig von der Bildrate gleich schnell bleibt.
        mountain.position +=
            Vector3.left * moveSpeed * Time.deltaTime;
    }

    void RecycleMountains()
    {
        // Zuerst den aktuell am weitesten links stehenden Berg bestimmen
        Transform leftmost = mountain01;

        if (mountain02.position.x < leftmost.position.x)
        {
            leftmost = mountain02;
        }

        if (mountain03.position.x < leftmost.position.x)
        {
            leftmost = mountain03;
        }

        // Danach den aktuell am weitesten rechts stehenden Berg bestimmen
        Transform rightmost = mountain01;

        if (mountain02.position.x > rightmost.position.x)
        {
            rightmost = mountain02;
        }

        if (mountain03.position.x > rightmost.position.x)
        {
            rightmost = mountain03;
        }

        // Wenn der linke Berg weit genug aus dem Sichtbereich
        // herausgelaufen ist, wird er rechts wieder eingesetzt.
        if (leftmost.position.x < -60f)
        {
            leftmost.position = new Vector3(
                rightmost.position.x + mountainSpacing,
                leftmost.position.y,
                leftmost.position.z
            );
        }
    }
}


