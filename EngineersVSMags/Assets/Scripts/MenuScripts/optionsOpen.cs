using UnityEngine;

public class optionsOpen : MonoBehaviour
{
    public GameObject panel;

    private void Start()
    {
        panel.SetActive(false);
    }

    public void OnClick() 
    {
        panel.SetActive(true);
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape)) 
        {
            panel.SetActive(false);
        }
    }
}
