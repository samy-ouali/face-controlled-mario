using UnityEngine;
using System.Diagnostics;
using System.IO;

public class OpenFaceLauncher : MonoBehaviour
{
    Process openFaceProcess;

    void Start()
    {
        LaunchOpenFace();
    }

    void LaunchOpenFace()
    {
        string openFacePath = "/Users/samyouali/Desktop/OpenFace/build/bin/FeatureExtraction";
        string outputPath = "/Users/samyouali/Desktop/projetoutput";

        if (!Directory.Exists(outputPath))
        {
            Directory.CreateDirectory(outputPath);
        }

        ProcessStartInfo startInfo = new ProcessStartInfo();
        startInfo.FileName = "/bin/bash";

        //Lancement OpenFace 160x120
        startInfo.Arguments = $"-c \"'{openFacePath}' -device 0 -ofau -aus -cam_width 160 -cam_height 120 -out_dir '{outputPath}'\"";

        startInfo.UseShellExecute = false;
        startInfo.RedirectStandardOutput = true;
        startInfo.RedirectStandardError = true;
        startInfo.CreateNoWindow = true;

        try
        {
            openFaceProcess = Process.Start(startInfo);
            UnityEngine.Debug.Log("✅ OpenFace lancé avec webcam basse résolution");
        }
        catch (System.Exception e)
        {
            UnityEngine.Debug.LogError("Erreur lancement OpenFace: " + e.Message);
        }
    }

    void OnApplicationQuit()
    {
        if (openFaceProcess != null && !openFaceProcess.HasExited)
        {
            openFaceProcess.Kill();
            UnityEngine.Debug.Log("OpenFace arrêté.");
        }
    }
}