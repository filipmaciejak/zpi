using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// This class is only here temporarily to test interactions without mobile controls
public class PlayerController : MonoBehaviour
{
    [SerializeField] Player player;

    void Update()
    {
        player.UpdateMoveInput(Input.GetAxisRaw("Horizontal"));
        if (Input.GetButtonDown("Jump"))
        {
            player.Jump();
        }
        if (Input.GetButtonDown("Fire1"))
        {
            if (!player.usedModule)
            {
                player.Interact();
            }
            else
            {
                player.FinishInteraction();
            }
        }
    }
}
