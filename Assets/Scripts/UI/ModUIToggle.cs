using UnityEngine;
using UnityEngine.InputSystem;


public class ModUIToggle : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public GameObject modUI;
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void OnMenu()
    {
        Debug.Log("Menu button pressed");
        if (modUI != null)
        {
            if (modUI.activeSelf)
            {
                modUI.SetActive(false);
            }
            else
            {
                modUI.SetActive(true);
            }
        }
    }
}
