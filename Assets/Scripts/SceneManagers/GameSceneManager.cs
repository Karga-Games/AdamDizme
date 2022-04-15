using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
//using ElephantSDK;

public class GameSceneManager : MonoBehaviour
{
    public int targetFPS;
    public CameraController _camera;
    public PlayerController _player;
    public static GlobalAudioManager _audioManager;
    public GameObject LevelEndUI;
    public GameObject LevelFailedUI;
    public GameObject Confetti;
    //public GameObject _playerMesh;

    int level;
    [Header("Level Settings")]
    public bool spawnLevel;
    public static bool gameOver = false;


    [Header("Level End Camera Settings")]
    public Vector3 LevelEndCameraFollow;
    public Vector3 LevelEndCameraLook;
    public float LevelEndFollowSpeed;
    public float LevelEndLookSpeed;

    [Header("Level Fail Camera Settings")]
    public Vector3 LevelFailCameraFollow;
    public Vector3 LevelFailCameraLook;
    public float LevelFailFollowSpeed;
    public float LevelFailLookSpeed;

    // Start is called before the first frame update

    private void Awake()
    {
        Application.targetFrameRate = targetFPS;

        LeanTween.init(4000);
    }

    void Start()
    {
        gameOver = false;

        _audioManager = FindObjectOfType<GlobalAudioManager>();

        //Account.Level = 1;

        level = Account.Level;

        if (spawnLevel)
        {
            SpawnLevel();
        }

        if (_camera == null)
        {
            _camera = FindObjectOfType<CameraController>();
        }

        if(_player == null)
        {
            _player = FindObjectOfType<PlayerController>();
        }

        if(_camera != null && _player != null)
        {
            _camera.SetupCamera(_player.transform,CameraModes.Normal);
        }

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Reload()
    {
        SceneManager.LoadScene("GameScene_Ball", LoadSceneMode.Single);
    }

    public void LevelPassed()
    {
        if (!gameOver)
        {

            //PlaySound("LevelPass");
            //Elephant.LevelCompleted(Account.Level);
            gameOver = true;

            /*
            _camera.followSpeed = LevelEndFollowSpeed;
            _camera.lookSpeed = LevelEndLookSpeed;
            _camera.FollowOffset = LevelEndCameraFollow;
            _camera.TargetOffset = LevelEndCameraLook;
            */

            if (LevelEndUI != null)
            {
                LevelEndUI.SetActive(true);
            }
            Instantiate(Confetti, _camera.transform.position + (_camera.transform.forward * 3.5f) + (_camera.transform.up * 1f),Quaternion.identity);

            Debug.Log("Level Ended!");
            Account.Level++;
        }
    }

    public void LevelFailed()
    {
        if (!gameOver)
        {
            //PlaySound("LevelFail");
            //Elephant.LevelFailed(Account.Level);
            gameOver = true;

            /*
            _camera.followSpeed = LevelFailFollowSpeed;
            _camera.lookSpeed = LevelFailLookSpeed;
            _camera.FollowOffset = LevelFailCameraFollow;
            _camera.TargetOffset = LevelFailCameraLook;
            */

            if (LevelFailedUI != null)
            {
                LevelFailedUI.SetActive(true);
            }
            Debug.Log("Level Failed!");
        }
        
    }

    public void NextLevel()
    {
        Reload();
    }


    public void SpawnLevel()
    {
        //Elephant.LevelStarted(Account.Level);

        if (level > 5)
        {
            level = level % 5 + 1;
        }

        GameObject levelPrefab = Resources.Load<GameObject>("Levels/Level" + level.ToString());
        if (levelPrefab == null)
        {
            levelPrefab = Resources.Load<GameObject>("Prefabs/Levels/Level5");
        }

        Instantiate(levelPrefab, Vector3.zero, Quaternion.identity);

    }

    public static void PlaySound(string name, bool refresh = false)
    {
        if (_audioManager != null)
        {
            _audioManager.PlaySound(name, refresh);
        }
    }
    public static void StopSound(string name)
    {
        if (_audioManager != null)
        {
            _audioManager.StopSound(name);
        }
    }

    public static void ChangeSkybox(Material skybox)
    {
        RenderSettings.skybox = skybox;
    }
}
