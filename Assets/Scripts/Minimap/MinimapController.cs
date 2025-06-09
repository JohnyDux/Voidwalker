using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinimapController : MonoBehaviour
{
    public Transform playerTransform;
    public PlayerController playerController;

    public RectTransform playerIcon; // Reference to the RectTransform of the cursor
    Quaternion playerIconRotation;
    public float minimapScaleXX = 2.831530568f;
    public float minimapScaleYZ = 2.830384492f;

    public float iconOffsetX;
    public float iconOffsetY;

    public GameObject map;
    public bool mapOn;
    public Transform mapMenuCanvas;

    public RectTransform mapSelectorPosition;
    public float mapSelectorMoveSpeed;
    private Vector2 targetPosition;
    public RectTransform mapMarker;
    public GameObject marker3D;
    public GameObject markerUI;
    public GameObject markerUIInstance;
    public GameObject marker3DInstance;
    public bool markerOn;

    public GameObject hud;

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
            if (mapOn == false)
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
            SwitchHUD(false);
            WorldPositionToMapPosition(playerTransform);
            MoveSelector();

            if (Input.GetKeyDown(KeyCode.Return))
            {
                SetMarker();
            }
        }
        else
        {
            SwitchHUD(true);
        }

        void SwitchHUD(bool state)
        {
            if (state == false)
            {
                hud.SetActive(false);
            }
            else if (state == true)
            {
                hud.SetActive(true);
            }
        }

        void WorldPositionToMapPosition(Transform playerTransform)
        {
            // Get the player's position
            Vector3 playerPosition = playerTransform.position;

            // Convert world position to minimap position
            Vector2 minimapPosition = new Vector2(playerPosition.x * minimapScaleXX + iconOffsetX, playerPosition.z * minimapScaleYZ + iconOffsetY);

            // Update the player icon's position
            playerIcon.anchoredPosition = minimapPosition;

            // Rotate the UI icon to face the player's forward direction
            float playerYRotation = playerTransform.eulerAngles.y;
            Quaternion targetRotation = Quaternion.Euler(0, 0, playerYRotation);


            playerIcon.rotation = targetRotation;
        }

        void MoveSelector()
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

        void SetMarker()
        {
            if (markerOn == false)
            {
                //ligar marker
                markerOn = true;

                //criar marker no UI
                markerUIInstance = Instantiate(markerUI, mapMenuCanvas);
                markerUIInstance.GetComponent<RectTransform>().anchoredPosition = mapSelectorPosition.anchoredPosition;

                //criar marker no mundo
                //marker3DInstance = Instantiate(marker3D, MapPositionToWorldPosition(mapMarker), Quaternion.Euler(0, 0, 0));
            }
            else if (markerUIInstance != null && marker3DInstance != null)
            {
                Destroy(markerUIInstance);
                Destroy(marker3DInstance);
                markerUIInstance = null;
                marker3DInstance = null;
                markerOn = false;
            }
        }
    }
}