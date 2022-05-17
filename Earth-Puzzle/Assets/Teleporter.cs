using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class Teleporter : MonoBehaviour
{
    public void Teleport(Vector3 position)
    {
        transform.transform.position = position;
    }
}
