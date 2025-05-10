using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinimapController : MonoBehaviour
{
    public Transform playerTransform;
    public RectTransform cursorRect; // Reference to the RectTransform of the cursor

    float zoom;
    public float multiplierX;
    public float multiplierY;
    public float offsetX;
    public float offsetY;

    public GameObject map;
    public bool mapOn;

    private void Start()
    {
        mapOn = false;
        zoom = Input.GetAxis("Mouse ScrollWheel");
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
            ZoomMap(zoom);
        }
    }

    private void ZoomMap(float zoom)
    {

    }
    private void WorldPositionToMapPosition()
    {
        // Get the player's position
        Vector3 worldPosition = playerTransform.position;

        // Scale the player's position to fit the minimap
        float xUIPosition = worldPosition.x * -multiplierX;
        float yUIPosition = worldPosition.z * -multiplierY;

        // Set the minimap's position
        cursorRect.localPosition = new Vector3(xUIPosition + offsetX, yUIPosition + offsetY, 0);
    }
}
