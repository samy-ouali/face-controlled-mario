using UnityEngine;
using System.IO;
using System.Collections;
using System.Globalization;

public class OpenFaceReader : MonoBehaviour
{
    [Header("Chemin du CSV généré par OpenFace")]
    [SerializeField] private string folderPath = "/Users/samyouali/Desktop/projetoutput";
    [SerializeField] private float checkInterval = 0.05f;

    public bool mouthJustOpened = false;

    private float previousAU25 = 0f;
    private bool wasMouthOpen = false;
    private string lastFileRead = "";
    private System.DateTime lastWriteTime;

    private const float AU25_THRESHOLD = 0.85f; // seuil haut = bouche bien ouverte

    void Start()
    {
        StartCoroutine(WatchAU25());
    }

    IEnumerator WatchAU25()
    {
        while (true)
        {
            string line = TryReadLastValidFaceLine();
            if (line != null)
            {
                string[] values = line.Split(',');
                float au25 = ParseAU(values, 19);
                int success = int.Parse(values[4]);

                // Log à chaque frame
                Debug.Log($"[AU25 DEBUG] success={success} | AU25={au25:0.00}");

                bool isMouthOpenNow = (success == 1 && au25 >= AU25_THRESHOLD);

                // transition bouche fermée ouverte 
                if (!wasMouthOpen && isMouthOpenNow)
                {
                    mouthJustOpened = true;
                    Debug.Log("Bouche détectée (transition)");
                }
                else
                {
                    mouthJustOpened = false;
                }

                wasMouthOpen = isMouthOpenNow;
                previousAU25 = au25;
            }

            yield return new WaitForSeconds(checkInterval);
        }
    }

    string TryReadLastValidFaceLine()
    {
        var files = Directory.GetFiles(folderPath, "*.csv");
        if (files.Length == 0) return null;

        var latestFile = files[files.Length - 1];
        var newWriteTime = File.GetLastWriteTime(latestFile);

        if (latestFile == lastFileRead && newWriteTime == lastWriteTime)
            return null;

        lastFileRead = latestFile;
        lastWriteTime = newWriteTime;

        try
        {
            using (var fs = new FileStream(latestFile, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            using (var sr = new StreamReader(fs))
            {
                string lastLine = null;
                while (!sr.EndOfStream)
                {
                    string line = sr.ReadLine();
                    if (LineHasValidSuccess(line))
                        lastLine = line;
                }
                return lastLine;
            }
        }
        catch
        {
            return null;
        }
    }

    bool LineHasValidSuccess(string line)
    {
        if (string.IsNullOrWhiteSpace(line)) return false;
        string[] parts = line.Split(',');
        if (parts.Length < 20) return false;
        return parts[4] == "1"; // success == 1
    }

    float ParseAU(string[] values, int index)
    {
        if (index >= 0 && index < values.Length)
        {
            float.TryParse(values[index], NumberStyles.Any, CultureInfo.InvariantCulture, out float result);
            return result;
        }
        return 0f;
    }
}
