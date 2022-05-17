using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class Teleporter : MonoBehaviour
{
    public void Teleport(Transform position)
    {
        transform.transform.position = position.position;
    }
}
