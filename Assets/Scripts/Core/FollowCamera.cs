using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace RPG.Core
{
    public class FollowCamera : MonoBehaviour
    {
        [SerializeField] Transform player;
              void Update()
        {
            Vector3 pos = player.position;
            pos.y = transform.position.y;
            pos.z = player.position.z - 5;

            transform.position = Vector3.MoveTowards(transform.position, pos, Time.deltaTime * 10);
        }
    }
}
