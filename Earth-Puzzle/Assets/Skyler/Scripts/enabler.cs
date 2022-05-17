using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class enabler : MonoBehaviour
{
    public void Enable()
    {
        GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.None;
    }
}
