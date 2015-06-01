using UnityEngine;

public class Menu : MonoBehaviour
{
    public void Play()
    {
        Debug.Log("[Menu] Play");
        Application.LoadLevel("Game");
    }

    public void Options()
    {
        Debug.Log("[Menu] Options");
        Application.LoadLevel("Options");
    }

    public void Quit()
    {
        Debug.Log("[Menu] Quit");
        Application.Quit();
    }
}
