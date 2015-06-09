using UnityEngine;

public class MenuController : MonoBehaviour
{
    /*
     * Methods
     */

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

#if UNITY_STANDALONE
    private void Awake()
    {
        Screen.SetResolution(384, 640, false);
    }
#endif
}
