using System.IO;
using MSCLoader;
using UnityEngine;

namespace DistanceTrackerMod
{
    public class DistanceTracker : Mod
    {
        public static string SavePath => Path.Combine(Application.persistentDataPath, "DistanceTracker_save.txt");

        public override string ID => "DistanceTracker";
        public override string Name => "Distance Tracker";
        public override string Author => "Dingus Dongulus";
        public override string Version => "1.0";

        public override void ModSetup()
        {
            GameObject trackerGO = new GameObject("DistanceTrackerBehaviour");
            trackerGO.AddComponent<DistanceTrackerBehaviour>();
            Object.DontDestroyOnLoad(trackerGO);

            ModConsole.Print("Distance Tracker loaded.");
        }
    }
}
