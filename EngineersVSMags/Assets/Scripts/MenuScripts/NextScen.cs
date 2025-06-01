using UnityEngine;
using UnityEngine.SceneManagement;

public class NextScen : MonoBehaviour
{
    public void OpenNextScen() 
    {
        SceneManager.LoadScene(1);
    }
}
