using UnityEngine;

public class SkyboxMover : MonoBehaviour
{
    // Geschwindigkeit, mit der sich die Skybox dreht
    public float rotationSpeed = 0.3f;

    // Prüft, ob die Atemsession gerade läuft
    public MeditationManager meditationManager;

    void Update()
    {
        // Vor dem Start der Session soll sich die Skybox nicht bewegen
        if (!meditationManager.sessionRunning)
        {
            return;
        }

        // Drehung der Skybox bei jedem Frame leicht erhöhen
        // Time.deltaTime sorgt für eine gleichmäßige Geschwindigkeit
        // unabhängig von der Bildrate
        RenderSettings.skybox.SetFloat(
            "_Rotation",
            RenderSettings.skybox.GetFloat("_Rotation")
            + rotationSpeed * Time.deltaTime
        );
    }
}
