using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Profiling;
using UnityEngine.Rendering;
using TMPro;

public class TestValues : MonoBehaviour
{
    public GameObject gameInfo;

    public TextMeshProUGUI playerPositionText;
    public GameObject player;
    public UIController uIController;
    public TextMeshProUGUI mousePositionText;

    //CPU
    public TextMeshProUGUI numberProcessorsText;
    public TextMeshProUGUI operatingSystemText;
    public TextMeshProUGUI processorTypeText;
    public TextMeshProUGUI graphicsDeviceText;

    //Memory
    public TextMeshProUGUI allocatedMemoryText; 
    public TextMeshProUGUI reservedMemoryyText; 
    public TextMeshProUGUI unusedReservedMemoryText;

    //Physics
    public TextMeshProUGUI performanceText;
    private int collisionCount = 0;
    private int triggerCount = 0;

    //Rendering
    public TextMeshProUGUI trianglesText;
    public TextMeshProUGUI verticesText;

    void Update()
    {
        //Activate/Deactivate Panel
        if (Input.GetKeyDown(KeyCode.F5))
        {
            if(gameInfo.activeInHierarchy == false)
            {
                gameInfo.SetActive(true);
            }
            else if (gameInfo.activeInHierarchy == true)
            {
                gameInfo.SetActive(false);
            }
        }

        //CPU
        numberProcessorsText.text = ($"Number of Processors: {Environment.ProcessorCount}");
        operatingSystemText.text = ($"Operating System: {SystemInfo.operatingSystem}");
        processorTypeText.text = ($"Processor Type: {SystemInfo.processorType}");
        graphicsDeviceText.text = ($"Graphics Device: {SystemInfo.graphicsDeviceName}");

        //Memory
        playerPositionText.text = "X: " + player.transform.position.x + "\nY: " + player.transform.position.y + "\nZ: " + player.transform.position.z;
        allocatedMemoryText.text = "Allocated Memory: " + Profiler.GetTotalAllocatedMemoryLong().ToString();
        reservedMemoryyText.text = "Reserved Memory: " + Profiler.GetTotalReservedMemoryLong().ToString();
        unusedReservedMemoryText.text = "Unused Reserved Memory: " + Profiler.GetTotalUnusedReservedMemoryLong().ToString();

        //Rendering
        int totalTriangles = 0;
        int totalVertices = 0;

        MeshFilter[] meshFilters = FindObjectsOfType<MeshFilter>();
        foreach (MeshFilter meshFilter in meshFilters)
        {
            Mesh mesh = meshFilter.sharedMesh;
            if (mesh != null)
            {
                totalTriangles += mesh.triangles.Length / 3; // Each triangle has 3 vertices
                totalVertices += mesh.vertexCount;
            }
        }
        trianglesText.text = "Number of Triangles: " + totalTriangles;
        verticesText.text = "Number of Vertices: " + totalVertices;

        //Physics
        int rigidbodyCount = FindObjectsOfType<Rigidbody>().Length;
        int colliderCount = FindObjectsOfType<Collider>().Length;
        int activeJointCount = FindObjectsOfType<Joint>().Length;
        float physicsTime = Time.fixedDeltaTime;

        string performanceMetrics = $"FPS: {MathF.Round(1.0f / Time.deltaTime)}\n" +
                                    $"Collisions: {collisionCount}\n" +
                                    $"Triggers: {triggerCount}\n" +
                                    $"Rigidbodies: {rigidbodyCount}\n" +
                                    $"Colliders: {colliderCount}\n" +
                                    $"Joints: {activeJointCount}\n" +
                                    $"Physics Time: {physicsTime:F4} sec";

        if (performanceText != null)
        {
            performanceText.text = performanceMetrics;
        }
    }
}
