using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewBehaviourScript : MonoBehaviour
{
    [Header("XR Rig")]
    [SerializeField] private Transform _xrRig;
    [SerializeField] private Transform _rigOffset;
    [SerializeField] private Transform _rigCamera;

    [Header("Transforms")]
    [SerializeField] private Transform _desiredTransform;

    // Set the player position and rotation, useful to force rotation after loading scene
    public void SetPlayerPositionAndRotation()
    {
        Vector3 newPos = new Vector3(_desiredTransform.position.x, _xrRig.position.y, _desiredTransform.position.z);

        _xrRig.SetPositionAndRotation(newPos, _desiredTransform.rotation);

        _rigOffset.localRotation = Quaternion.identity;
        _rigOffset.localRotation = Quaternion.Inverse(Quaternion.Euler(0, _rigCamera.eulerAngles.y, 0)) * _xrRig.rotation;
    }
}
