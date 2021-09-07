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
    float speed { get; set; }
    EntityStates currentState { get; set; }

    void ChangeState(EntityStates state);
    void TakeDamage(int damage);
}