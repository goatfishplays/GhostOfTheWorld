using UnityEngine;

public class PlayerCameraControl : MonoBehaviour
{
    public Vector3 rotation;
    public float xRotMult = .25f;
    public float yRotMult = .25f;
    public Vector2 xRotBounds = new Vector2(-89, 89);

    [Header("")]
    public bool controlling = true;

    [Header("Transforms")]
    public Transform playerTransform;
    public Transform playerHeadTransform;
    public Transform cameraTransform;
    // Update is called once per frame
    void LateUpdate()
    {
        cameraTransform.position = playerHeadTransform.position;
        playerTransform.eulerAngles = new Vector3(0.0f, rotation.y, 0.0f);
        playerHeadTransform.eulerAngles = rotation;
        cameraTransform.eulerAngles = rotation;
    }

    public void AddRotation(Vector2 movement)
    {
        // Debug.Log(movement);
        rotation.x += -movement.y * xRotMult;
        if (rotation.x < xRotBounds.x)
        {
            rotation.x = xRotBounds.x;
        }
        else if (rotation.x > xRotBounds.y)
        {
            rotation.x = xRotBounds.y;
        }

        rotation.y += movement.x * yRotMult;

    }
}
