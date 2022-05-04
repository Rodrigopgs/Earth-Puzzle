using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class PickupGrid : MonoBehaviour
{
    public static PickupGrid Instance { get; private set; }

    public Vector2 latticeOrigin;
    public int latticeWidth;
    public int latticeHeight;

    private Lattice2D<Vector2> pickupPositions;

    void Start()
    {
        Instance = this;

        pickupPositions = new Lattice2D<Vector2>(latticeWidth, latticeHeight, latticeOrigin);

        for (int w = 0; w < latticeWidth; w++)
        {
            for (int h = 0; h < latticeHeight; h++)
            {
                pickupPositions[w, h] = pickupPositions.LatticeToWorldPosition(w, h);
            }
        }
    }
}
