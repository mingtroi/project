using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] GameObject pauseMenu;
    private bool isPaused = false; // Biến kiểm tra trạng thái Pause

    public void Start()
    {
        pauseMenu.SetActive(false);
    }
    public void TogglePause()
    {
        isPaused = !isPaused; // Đảo trạng thái Pause

        if (isPaused)
        {
            Pause();
        }
        else
        {
            Resume();
        }
    }

    public void Pause()
    {
        pauseMenu.SetActive(true);
        Time.timeScale = 0; // Dừng game
    }

    public void Resume()
    {
        pauseMenu.SetActive(false);
        Time.timeScale = 1; // Tiếp tục game
    }

    public void Restart()
    {
        Time.timeScale = 1; // Đảm bảo game chạy lại bình thường sau khi restart
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void Home()
    {
        Time.timeScale = 1; // Đảm bảo thời gian chạy bình thường

        if (SceneExists("Main Menu"))
        {
            SceneManager.LoadScene("Main Menu");
        }
        else
        {
            Debug.LogError("Scene 'Main Menu' chưa được thêm vào Build Settings!");
        }
    }

    private bool SceneExists(string sceneName)
    {
        for (int i = 0; i < SceneManager.sceneCountInBuildSettings; i++)
        {
            string scenePath = SceneUtility.GetScenePathByBuildIndex(i);
            string sceneFileName = System.IO.Path.GetFileNameWithoutExtension(scenePath);
            if (sceneFileName == sceneName)
            {
                return true;
            }
        }
        return false;
    }
   



}
