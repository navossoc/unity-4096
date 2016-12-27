using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour
{
    /*
     * Methods
     */

    public void Play()
    {
        Debug.Log("[Menu] Play");
        SceneManager.LoadScene("Game");
    }

    public void Options()
    {
        Debug.Log("[Menu] Options");
        SceneManager.LoadScene("Options");
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
