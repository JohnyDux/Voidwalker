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

    private void Start()
    {
        mapOn = false;
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
}
