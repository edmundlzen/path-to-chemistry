using System.Collections.Generic;
using UnityEngine;

public enum EntityStates
{
    Patrol,
    Investigate,
    Chase,
    Attack,
    Flee
}
public interface IEntity
{
    int health { get; set; }
    int damage { get; set; }
    float speed { get; set; }
    Dictionary<string, int> drops { get; set; }
    EntityStates currentState { get; set; }

    void ChangeState(EntityStates state);
    void Attacked(int damage);
}