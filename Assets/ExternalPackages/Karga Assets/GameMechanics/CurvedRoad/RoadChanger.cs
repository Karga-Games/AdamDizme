using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
[ExecuteInEditMode]
public class RoadChanger : MonoBehaviour
{
    public RoadMeshGenerator ChangePlayerRoadTo;
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
    }

    private void OnTriggerEnter(Collider other)
    {
        RoadPlayerController _playerController = other.GetComponent<RoadPlayerController>();
        if (_playerController != null)
        {
            _playerController.ChangeRoad(ChangePlayerRoadTo);
        }
    }
}
