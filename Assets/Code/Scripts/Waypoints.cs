using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Waypoints : MonoBehaviour
{
    public static Transform[] path1;  // Danh sách ?i?m ?i cho ???ng 1 (Path)
    public static Transform[] path2;  // Danh sách ?i?m ?i cho ???ng 2 (Path2)

    void Awake()
    {
        if (gameObject.name == "Path")  // N?u là Path, l?y danh sách ?i?m c?a Path
        {
            path1 = new Transform[transform.childCount];
            for (int i = 0; i < path1.Length; i++)
            {
                path1[i] = transform.GetChild(i);
            }
        }
        else if (gameObject.name == "Path2")  // N?u là Path2, l?y danh sách ?i?m c?a Path2
        {
            path2 = new Transform[transform.childCount];
            for (int i = 0; i < path2.Length; i++)
            {
                path2[i] = transform.GetChild(i);
            }
        }
    }
}
