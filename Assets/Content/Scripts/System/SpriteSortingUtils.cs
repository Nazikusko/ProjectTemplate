using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class SpriteSortingUtils 
{
    public static int GetSortingOrderFromSprite(Vector3 position) => -(int)(position.y * 100);
}
