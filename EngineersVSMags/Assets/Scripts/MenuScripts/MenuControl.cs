using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class MenuControl : MonoBehaviour
{
    private string playerF;
    private string enemyF;
    private bool isServer;
    public GameObject panel;

    private void Awake()
    {
        //panel.SetActive(false);

        UnionClass.isServ = true;
      
        UnionClass.plF = "ING";
        UnionClass.emF = "MAG";
    }
    public void Exit()
    {
        Application.Quit();
    }

    public void IAmEng() 
    {
        UnionClass.isServ = true;
        UnionClass.plF = "ING";
        UnionClass.emF = "MAG";
    }

    public void IAmMag() 
    {
        UnionClass.isServ = false;
        UnionClass.plF = "MAG";
        UnionClass.emF = "ING";
    }

    public void Play() 
    {
        panel.SetActive(true);
        panel.GetComponent<Animator>().enabled = true;
    }

    public void ToGame() 
    {
        SceneManager.LoadScene(1);
    }
}
