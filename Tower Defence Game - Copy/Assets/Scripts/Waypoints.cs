using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Waypoints : MonoBehaviour
{
    //public static Transform Wall;
    public Transform[] WPoint;
    public static Waypoints primary;
    public static List<Waypoints> secondaries;

    void Start()
    {
        if (!primary)
        {
            primary = this;
            this.name = "Waypoint (Primary)";
        }
        secondaries = new List<Waypoints>(FindObjectsOfType<Waypoints>());
        secondaries.Remove(primary);

        if (secondaries == null)
        {
            print("secondary is empty");
            return;
        }
        else
        {
            foreach(Waypoints secwaypoint in secondaries)
            {
                secwaypoint.name = "Waypoint (Secondary)";
            }
        }
            //secondaries[0].name = "Waypoint (Secondary)";

        WPoint = new Transform[transform.childCount];
        for (int i = 0; i < WPoint.Length; i++)
        {
            WPoint[i] = transform.GetChild(i);
        }
    }
}
