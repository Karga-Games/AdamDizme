using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StopPoint : MonoBehaviour
{
    public GameObject confetti;
    LevelFinisher finisher;
    RunnerPlayerController playerController;
    CameraController cameraController;
    CrowdController crowdController;
    GameSceneManager gameSceneManager;
    bool passed = false;
    // Start is called before the first frame update
    void Start()
    {
        playerController = FindObjectOfType<RunnerPlayerController>();
        cameraController = FindObjectOfType<CameraController>();
        finisher = GetComponentInParent<LevelFinisher>();
        crowdController = FindObjectOfType<CrowdController>();
        gameSceneManager = FindObjectOfType<GameSceneManager>();
        passed = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (passed)
        {
            playerController.speed = 0;
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        Stickman stickman = other.GetComponent<Stickman>();
        if (stickman != null && !passed)
        {
            passed = true;
            finisher.passed = false;

            GameObject objectToLook = crowdController.GetLastMan().gameObject;

            cameraController.objectToFollow = null;

            LeanTween.move(cameraController.gameObject, objectToLook.transform.position+cameraController.FollowOffset, 2f).setEaseInOutQuad();
            cameraController.SetupLookTarget(objectToLook.transform);
            cameraController.LockRotation.X = true;

            StartCoroutine(GeneralFunctions.executeAfterSec(() => {

                Instantiate(confetti, cameraController.transform).transform.localPosition = new Vector3(0, 0, 2f);

                gameSceneManager.LevelPassed();

            },2f));
        }
    }
}
