using UnityEngine;
using System.Collections.Generic;
using System.Globalization;

public class BreathDataManager : MonoBehaviour
{
    // Ein einzelner Atemwert aus der CSV-Datei
    // besteht aus einer Zeit und dem dazugehörigen Atemwert.
    [System.Serializable]
    public class BreathSample
    {
        public float time;
        public float breath;

        // Erstellt einen neuen Atemwert mit Zeit und Atemstärke
        public BreathSample(float time, float breath)
        {
            this.time = time;
            this.breath = breath;
        }
    }

    // Die CSV-Datei, die im Unity Inspector zugewiesen wird
    public TextAsset csvFile;

    // Der aktuell verwendete Atemwert
    public float currentBreathValue = 0f;

    // Liste mit allen Atemwerten aus der CSV-Datei
    private List<BreathSample> samples =
        new List<BreathSample>();

    // Gibt an, wie weit die Simulation in der CSV fortgeschritten ist
    private float playbackTime = 0f;

    // Gibt an, ob die Atemdaten gerade abgespielt werden
    private bool playing = false;

    void Start()
    {
        // CSV-Datei beim Start des Spiels laden
        LoadCSV();
    }

    void Update()
    {
        // Wenn keine Atemdaten abgespielt werden, nichts tun
        if (!playing)
        {
            return;
        }

        // Die vergangene Zeit seit dem letzten Frame addieren
        playbackTime += Time.deltaTime;

        // Den passenden Atemwert für die aktuelle Zeit berechnen
        UpdateBreathValue();
         
       if (playbackTime >= GetLastSampleTime())
        {
            // Wenn das Ende der CSV erreicht ist, Wiedergabe beenden
            playing = false;

            // Atemwert wieder auf neutral setzen
            currentBreathValue = 0f;
        }
    }

    public void StartBreathingData()
    {
        // Atemsimulation wieder von Anfang an starten
        playbackTime = 0f;
        currentBreathValue = 0f;
        playing = true;
    }

    void LoadCSV()
    {
        // Prüfen, ob überhaupt eine CSV-Datei zugewiesen wurde
        if (csvFile == null)
        {
            Debug.LogError(
                "Keine CSV-Datei zugewiesen!"
            );

            return;
        }

        // Vor dem Laden alte Daten aus der Liste entfernen
        samples.Clear();

        // CSV-Datei in einzelne Zeilen aufteilen
        string[] lines =
            csvFile.text.Split('\n');

        // Bei Zeile 1 beginnen, da Zeile 0 die Überschrift enthält
        for (int i = 1; i < lines.Length; i++)
        {
            string line =
                lines[i].Trim();

            // Leere Zeilen überspringen
            if (string.IsNullOrEmpty(line))
            {
                continue;
            }

            // Jede Zeile an dem Komma in zwei Werte aufteilen
            string[] values =
                line.Split(',');

            // Zeile überspringen, wenn weniger als zwei Werte vorhanden sind
            if (values.Length < 2)
            {
                continue;
            }

            float time;
            float breath;

            // Zeitwert aus der CSV in eine Zahl umwandeln
            bool timeValid =
                float.TryParse(
                    values[0],
                    NumberStyles.Float,
                    CultureInfo.InvariantCulture,
                    out time
                );

            // Atemwert aus der CSV in eine Zahl umwandeln
            bool breathValid =
                float.TryParse(
                    values[1],
                    NumberStyles.Float,
                    CultureInfo.InvariantCulture,
                    out breath
                );

            // Nur gültige Zeit- und Atemwerte zur Liste hinzufügen
            if (timeValid && breathValid)
            {
                samples.Add(
                    new BreathSample(
                        time,
                        breath
                    )
                );
            }
        }

        Debug.Log(
            "CSV geladen. Samples: "
            + samples.Count
        );
    }

    void UpdateBreathValue()
    {
        // Wenn keine Atemdaten vorhanden sind, neutralen Wert verwenden
        if (samples.Count == 0)
        {
            currentBreathValue = 0f;
            return;
        }

        // Wenn die Simulation noch vor dem ersten CSV-Wert liegt,
        // den ersten Atemwert verwenden
        if (playbackTime <= samples[0].time)
        {
            currentBreathValue =
                samples[0].breath;

            return;
        }

        // Die beiden CSV-Werte suchen, zwischen denen die aktuelle Zeit liegt
        for (int i = 0; i < samples.Count - 1; i++)
        {
            BreathSample current =
                samples[i];

            BreathSample next =
                samples[i + 1];

            if (
                playbackTime >= current.time &&
                playbackTime <= next.time
            )
            {
                // Berechnen, wie weit die aktuelle Zeit
                // zwischen den beiden CSV-Werten liegt
                float percentage =
                    Mathf.InverseLerp(
                        current.time,
                        next.time,
                        playbackTime
                    );

                // Atemwert zwischen den beiden Punkten
                // gleichmäßig interpolieren
                currentBreathValue =
                    Mathf.Lerp(
                        current.breath,
                        next.breath,
                        percentage
                    );

                return;
            }
        }

        // Falls kein Zwischenbereich gefunden wurde,
        // den letzten Atemwert verwenden
        currentBreathValue =
            samples[samples.Count - 1].breath;
    }

    float GetLastSampleTime()
    {
        // Wenn keine Daten vorhanden sind, 0 zurückgeben
        if (samples.Count == 0)
        {
            return 0f;
        }

        // Die Zeit des letzten CSV-Eintrags zurückgeben
        return samples[samples.Count - 1].time;
    }
}
