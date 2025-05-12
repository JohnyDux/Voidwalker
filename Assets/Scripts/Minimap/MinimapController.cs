using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinimapController : MonoBehaviour
{
    public Transform playerTransform;
    public RectTransform playerIcon; // Reference to the RectTransform of the cursor
    public float minimapScale = 0.1f;

    public float iconOffsetX;
    public float iconOffsetY;

    public GameObject map;
    public bool mapOn;

    public RectTransform mapSelectorPosition;
    public float mapSelectorMoveSpeed;
    private Vector2 targetPosition;
    public GameObject mapMarker;
    public bool markerOn;

    private void Start()
    {
        mapOn = false;
        markerOn = false;

        targetPosition = mapSelectorPosition.anchoredPosition;
    }

    private void Update()
    {
        map.SetActive(mapOn);

        if (Input.GetKeyDown(KeyCode.M))
        {
            if(mapOn == false)
            {
                //ligar mapa
                mapOn = true;
            }
            else
            {
                mapOn = false;
            }
        }

        if (mapOn)
        {
            WorldPositionToMapPosition();
            MoveSelector();
        }

        mapMarker.SetActive(markerOn);

        if (Input.GetKeyDown(KeyCode.Return))
        {
            if (markerOn == false)
            {
                //ligar marker
                markerOn = true;
            }
            else
            {
                markerOn = false;
            }
        }
    }
    private void WorldPositionToMapPosition()
    {
        // Get the player's position
        Vector3 playerPosition = playerTransform.position;

        // Convert world position to minimap position
        Vector2 minimapPosition = new Vector2(playerPosition.x * minimapScale + iconOffsetX, playerPosition.z * minimapScale + iconOffsetY);

        // Update the player icon's position
        playerIcon.anchoredPosition = minimapPosition;
    }

    private void MoveSelector()
    {
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            mapSelectorPosition.anchoredPosition -= new Vector2(mapSelectorMoveSpeed * Time.deltaTime, 0);
        }
        if (Input.GetKey(KeyCode.RightArrow))
        {
            mapSelectorPosition.anchoredPosition += new Vector2(mapSelectorMoveSpeed * Time.deltaTime, 0);
        }
        if (Input.GetKey(KeyCode.UpArrow))
        {
            mapSelectorPosition.anchoredPosition += new Vector2(0, mapSelectorMoveSpeed * Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.DownArrow))
        {
            mapSelectorPosition.anchoredPosition -= new Vector2(0, mapSelectorMoveSpeed * Time.deltaTime);
        }
    }

    //private void SetMarker()
    //{
    //    markerPosition = 
    //    Instantiate(mapMarker, markerPosition);
    //}
}
