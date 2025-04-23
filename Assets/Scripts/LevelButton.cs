using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LevelButton : MonoBehaviour
{
    public List<string> sceneNameList;
    public UIController UI;

    [SerializeField] GameObject levelsBoard;
    [SerializeField] GameObject loadingScreen;
    [SerializeField] int selectedFloorIndex;
    public RectTransform selector;
    bool selectorCanMove;
    public Image selectorImage;

    private void Awake()
    {
        loadingScreen.SetActive(false);
        levelsBoard.SetActive(false);
        selectorImage.color = Color.black;
        selectorCanMove = true;
    }


    public void ChooseFloor()
    {
        if(selectorCanMove == true)
            {
                selectorImage.color = Color.black;

                if (selectedFloorIndex < 4)
                {
                    selectedFloorIndex++;
                }
                else
                {
                    selectedFloorIndex = 2;
                }
                Debug.Log("Selected floor: " + selectedFloorIndex);

                //INSERIR INDEX CORRETO DAS CENAS
                if (selectedFloorIndex == 2)
                {
                    // Change the top and bottom properties
                    selector.offsetMin = new Vector2(selector.offsetMin.x, 139); //bottom
                    selector.offsetMax = new Vector2(selector.offsetMax.x, -2); //-top
                }
                //INSERIR INDEX CORRETO DAS CENAS
                else if (selectedFloorIndex == 3)
                {
                    // Change the top and bottom properties
                    selector.offsetMin = new Vector2(selector.offsetMin.x, 75); //bottom
                    selector.offsetMax = new Vector2(selector.offsetMax.x, -65); //-top
                }
                //INSERIR INDEX CORRETO DAS CENAS
                else if (selectedFloorIndex == 4)
                {
                    // Change the top and bottom properties
                    selector.offsetMin = new Vector2(selector.offsetMin.x, 9); //bottom
                    selector.offsetMax = new Vector2(selector.offsetMax.x, -131); //-top
                }
            }
    }

    public void SelectOption()
    {
        if(selectorCanMove == true)
            {
                selectorCanMove = false;
                selectorImage.color = Color.red;
            }
        else if(selectorCanMove == false)
            {
                loadScene(sceneNameList[selectedFloorIndex]);
            }
    }

    void loadScene(string newScene)
    {
        string currentScene = SceneManager.GetActiveScene().name;

        loadingScreen.SetActive(true);

        SceneManager.LoadSceneAsync(newScene);
        SceneManager.UnloadSceneAsync(currentScene);
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            levelsBoard.SetActive(true);
        }
    }
}
