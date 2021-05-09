using System.Collections.Generic;
using UnityEngine;
using Photon.Realtime;

public class RoomList : MonoBehaviour
{
    public GameObject itemPrefab;

    public void UpdateList(List<RoomInfo> rooms, Launcher launcher)
    {
        Debug.Log(rooms.Count + " rooms and " + transform.childCount + " previously");
        for (int i = 0; i < transform.childCount; i += 1)
        {
            Destroy(transform.GetChild(i).gameObject);
        }

        foreach (RoomInfo room in rooms)
        {
            if (room.IsVisible && room.IsOpen && !room.RemovedFromList)
            {
                GameObject item = Instantiate(itemPrefab);
                item.GetComponent<RoomInfoPanel>()
                    .UpdateRoomInfo(room, launcher);

                item.transform.SetParent(transform, false);
            }
        }
    }
}
