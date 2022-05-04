using System.Collections.Generic;

using UnityEngine;

/// <summary>
/// A class that allows the camera to be clamped inside multiple areas.
/// </summary>
[RequireComponent(typeof(Camera))]
public class CameraBounds : MonoBehaviour
{
    /// <summary>
    /// A property that can be used by the BasicInteraction events list.
    /// Changes the camera's region. The camera's region is equal to the element's number in the corners list.
    /// The camera is clamped into its region.
    /// </summary>
    public int Region
    {
        set
        {
            region = value;
            UpdateValues();
            Vector3 newPos = Vector2.Lerp(corners[value].topRight.position, corners[value].bottomLeft.position, 0.5f);
            newPos.z = transform.position.z;
            transform.position = newPos;
        }
        get => region;
    }

    [SerializeField, Tooltip("Which corners to use from the Corners list to clamp the camera's position")]
    private int region = 0;

    [Tooltip("A list of corners that the camera will use to clamp it's position based off its region")]
    public List<BoundCorners> corners;

    [Tooltip("The Transform to follow")]
    public Transform objectToTrack;


    float maxX;
    float maxY;
    float minX;
    float minY;

    float lastSize;

    Vector2 cameraSize;

    void Start()
    {
        UpdateValues();

        lastSize = cameraSize.x;

        Region = region;
    }

    void LateUpdate()
    {
        if (objectToTrack == null)
            return;

        Vector3 nextPos = new Vector3(objectToTrack.position.x, objectToTrack.position.y, transform.position.z);

        nextPos.x = Mathf.Clamp(nextPos.x, minX, maxX);
        if (nextPos.x < minX || nextPos.x > maxX)
            nextPos.x = transform.position.x;

        nextPos.y = Mathf.Clamp(nextPos.y, minY, maxY);
        if (nextPos.y < minY || nextPos.y > maxY)
            nextPos.y = transform.position.y;

        transform.position = nextPos;
    }

#if UNITY_EDITOR
    void OnDrawGizmos()
    {
        if (corners.Count <= 0)
            return;

        Gizmos.color = Color.yellow;

        for (int i = 0; i < corners.Count; i++)
        {

            if (corners[i].bottomLeft != null)
            {
                if (corners[i].topRight == null)
                {
                    Gizmos.DrawLine(corners[i].bottomLeft.position, new Vector3(corners[i].bottomLeft.position.x + 9001, corners[i].bottomLeft.position.y, corners[i].bottomLeft.position.z));
                    Gizmos.DrawLine(corners[i].bottomLeft.position, new Vector3(corners[i].bottomLeft.position.x, corners[i].bottomLeft.position.y + 9001, corners[i].bottomLeft.position.z));
                }
                else
                {
                    Gizmos.DrawLine(corners[i].bottomLeft.position, new Vector3(corners[i].topRight.position.x, corners[i].bottomLeft.position.y, corners[i].bottomLeft.position.z));
                    Gizmos.DrawLine(corners[i].bottomLeft.position, new Vector3(corners[i].bottomLeft.position.x, corners[i].topRight.position.y, corners[i].bottomLeft.position.z));
                }
            }

            if (corners[i].topRight != null)
            {
                if (corners[i].bottomLeft == null)
                {
                    Gizmos.DrawLine(corners[i].topRight.position, new Vector3(corners[i].topRight.position.x - 9001, corners[i].topRight.position.y, corners[i].topRight.position.z));
                    Gizmos.DrawLine(corners[i].topRight.position, new Vector3(corners[i].topRight.position.x, corners[i].topRight.position.y - 9001, corners[i].topRight.position.z));
                }
                else
                {
                    Gizmos.DrawLine(corners[i].topRight.position, new Vector3(corners[i].bottomLeft.position.x, corners[i].topRight.position.y, corners[i].topRight.position.z));
                    Gizmos.DrawLine(corners[i].topRight.position, new Vector3(corners[i].topRight.position.x, corners[i].bottomLeft.position.y, corners[i].topRight.position.z));
                }
            }
        }
    }
#endif

    void FixedUpdate()
    {
        float size = cameraSize.y * Screen.width / Screen.height;

        if (cameraSize.y != Camera.main.orthographicSize || lastSize != size)
        {
            UpdateValues();

            Region = region;
        }
        lastSize = cameraSize.x;
    }

    /// <summary>
    /// Updates the size of the camera for clamping purposes
    /// </summary>
    private void UpdateValues()
    {
        cameraSize.y = Camera.main.orthographicSize;
        cameraSize.x = cameraSize.y * Screen.width / Screen.height;

        maxX = corners[region].topRight.position.x - cameraSize.x;
        maxY = corners[region].topRight.position.y - cameraSize.y;
        minX = corners[region].bottomLeft.position.x + cameraSize.x;
        minY = corners[region].bottomLeft.position.y + cameraSize.y;
    }

    /// <summary>
    /// A struct that holds two Transform objects labeled topRight and bottomLeft.
    /// </summary>
    [System.Serializable]
    public struct BoundCorners
    {
        [Tooltip("The top right of the camera's view area")]
        public Transform topRight;
        [Tooltip("The bottom left of the camera's view area")]
        public Transform bottomLeft;

        public BoundCorners(Transform topRight, Transform bottomLeft)
        {
            this.topRight = topRight;
            this.bottomLeft = bottomLeft;
        }
    }
}

