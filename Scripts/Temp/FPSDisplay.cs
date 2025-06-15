using UnityEngine;
using TMPro;
using System.Diagnostics;

public class FPSDisplay : MonoBehaviour
{
    public TextMeshProUGUI fpsText; // Reference to the UI Text component
    private float deltaTime = 0.0f;

    private Stopwatch stopwatch;
    private long lastFrameGCCollection;

    void Start()
    {
        stopwatch = new Stopwatch();
        stopwatch.Start();
        lastFrameGCCollection = System.GC.CollectionCount(0); // Track GC calls
    }

    void Update()
    {
        // Calculate the time between frames for FPS
        deltaTime += (Time.deltaTime - deltaTime) * 0.1f;
        float fps = 1.0f / deltaTime;

        // Memory usage (in MB)
        float memoryUsage = System.GC.GetTotalMemory(false) / (1024f * 1024f);

        // Garbage Collection events since last frame
        long gcCollections = System.GC.CollectionCount(0) - lastFrameGCCollection;
        lastFrameGCCollection = System.GC.CollectionCount(0);

        // Latency approximation (frame time in ms)
        float frameTime = deltaTime * 1000.0f;

        // Update the UI Text with all stats
        fpsText.text = $"FPS: {Mathf.Ceil(fps)}\n" +
                       $"Memory: {memoryUsage:0.00} MB\n" +
                       //$"GC Calls: {gcCollections}\n" +
                       $"Frame Time: {frameTime:0.00} ms";
    }
}
