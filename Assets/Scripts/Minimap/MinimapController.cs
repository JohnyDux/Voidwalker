using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinimapController : MonoBehaviour
{
    public Transform playerTransform;
    public PlayerController playerController;

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
            SwitchHUD(false);
            WorldPositionToMapPosition();
            MoveSelector();
        }
        else
        {
            SwitchHUD(true);
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

    void SwitchHUD(bool state)
    {
        if(state == false)
        {
            hud.SetActive(false);
        }
        else if (state == true)
        {
            hud.SetActive(true);
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

        Vector3 moveDir = playerController.moveDirection;

        float forwardDot = Vector3.Dot(moveDir, transform.forward);
        float rightDot = Vector3.Dot(moveDir, transform.right);
        float backwardDot = Vector3.Dot(moveDir, -transform.forward);
        float leftDot = Vector3.Dot(moveDir, -transform.right);

        //Correct icon's rotation
        // Determine the direction
        if (forwardDot > 0.7f)
        {
            //Quaternion targetRotation = Quaternion.Euler(0, 0, targetRotationZ);
            //playerIcon.rotation = targetRotation;

            Debug.Log("Moving Forward");
        }
        else if (backwardDot > 0.7f)
        {
            playerIcon.Rotate(0, 0, -moveDir.magnitude);
            Debug.Log("Moving Backward");
        }
        else if (rightDot > 0.7f)
        {
            playerIcon.Rotate(0, 0, moveDir.magnitude);
            Debug.Log("Moving Right");
        }
        else if (leftDot > 0.7f)
        {
            playerIcon.Rotate(0, 0, -moveDir.magnitude);
            Debug.Log("Moving Left");
        }
        

        //na direção do x o x é positivo, se for direção contrária, x é negativo
        //na direção do z o z é positivo, se for direção contrária, z é negativo
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
