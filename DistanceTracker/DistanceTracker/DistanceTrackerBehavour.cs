using UnityEngine;
using System.IO;
using System.Linq;
using MSCLoader;

namespace DistanceTrackerMod
{
    public class DistanceTrackerBehaviour : MonoBehaviour
    {
        private float distanceLeft = 6584f;
        private float totalDriven = 0.0f;
        private float lastUpdateTime;

        private GUIStyle labelStyle;
        private GUIStyle valueStyle;
        private Font mscFont;

        private void Start()
        {
            lastUpdateTime = Time.time;

            // Use Unity's built-in Arial for cleaner spacing
            mscFont = Resources.GetBuiltinResource<Font>("Arial.ttf");

            labelStyle = new GUIStyle
            {
                fontSize = 15,
                wordWrap = false,
                clipping = TextClipping.Clip,
                normal = { textColor = new Color(1f, 0.92f, 0.016f) },
                font = mscFont
            };

            valueStyle = new GUIStyle
            {
                fontSize = 13,
                wordWrap = false,
                alignment = TextAnchor.MiddleRight,
                clipping = TextClipping.Clip,
                normal = {
                    textColor = Color.white,
                    background = MakeTex(1, 1, Color.black)
                },
                padding = new RectOffset(4, 4, 2, 2),
                font = mscFont
            };

            LoadData();
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.F7))
            {
                distanceLeft = 6584f;
                totalDriven = 0f;
                SaveData();
                ModConsole.Print("DistanceTracker reset.");
            }

            GameObject satsuma = GameObject.Find("SATSUMA(557kg, 248)");
            if (satsuma != null)
            {
                Rigidbody rb = satsuma.GetComponent<Rigidbody>();
                if (rb != null)
                {
                    float speedKmh = rb.velocity.magnitude * 3.6f;
                    float deltaTime = Time.time - lastUpdateTime;
                    lastUpdateTime = Time.time;

                    float kmThisFrame = (speedKmh / 3600f) * deltaTime;
                    distanceLeft = Mathf.Max(0f, distanceLeft - kmThisFrame);
                    totalDriven += kmThisFrame;

                    SaveData();
                }
            }
        }

        private void OnGUI()
        {
            float topOffset = 50f;     // nudged down
            float rightShift = 60f;    // nudged left
            float lineHeight = 22f;

            float xLabel = Screen.width - 220 - rightShift;
            float xValue = Screen.width - 90 - rightShift;

            GUI.Label(new Rect(xLabel, topOffset + 0, 130, lineHeight), "Distance Left:", labelStyle);
            GUI.Label(new Rect(xValue, topOffset + 0, 80, lineHeight), $"{distanceLeft:F2} km", valueStyle);

            GUI.Label(new Rect(xLabel, topOffset + 25, 130, lineHeight), "Total Driven:", labelStyle);
            GUI.Label(new Rect(xValue, topOffset + 25, 80, lineHeight), $"{totalDriven:F2} km", valueStyle);

            GUI.Label(new Rect(xLabel, topOffset + 50, 160, lineHeight), "[F7] Reset", labelStyle);
        }

        private void SaveData()
        {
            File.WriteAllText(DistanceTracker.SavePath, $"{distanceLeft},{totalDriven}");
        }

        private void LoadData()
        {
            if (File.Exists(DistanceTracker.SavePath))
            {
                string[] data = File.ReadAllText(DistanceTracker.SavePath).Split(',');
                if (data.Length == 2 &&
                    float.TryParse(data[0], out float left) &&
                    float.TryParse(data[1], out float driven))
                {
                    distanceLeft = left;
                    totalDriven = driven;
                }
            }
        }

        private Texture2D MakeTex(int width, int height, Color col)
        {
            Color[] pix = new Color[width * height];
            for (int i = 0; i < pix.Length; ++i)
                pix[i] = col;
            Texture2D result = new Texture2D(width, height);
            result.SetPixels(pix);
            result.Apply();
            return result;
        }
    }
}
