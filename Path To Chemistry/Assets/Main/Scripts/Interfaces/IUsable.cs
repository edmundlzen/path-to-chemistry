using UnityEngine;

// Use this interface for usable stuff: Weapons, health packs etc etc.
public interface IUsable
{
    void Use(Transform playerCamera);
}