using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class BlockRaycastForBattlePlace : MonoBehaviour
{
    public GameManager gameManager;
    public Image image;
    private void Update()
    {
        if (gameManager.battleSocketisActive)
        {
            image.raycastTarget = true;
        }
        else 
        {
            image.raycastTarget = false;
        }
    }
}
