using UnityEngine;

public interface IPlayerState2D
{
    void EnterState(PlayerController2d player);
    void UpdateState(PlayerController2d player);
    void FixedUpdateState(PlayerController2d player);
    void ExitState(PlayerController2d player);
    
    
    void OnCollisionEnter2D(Collision2D collision) { }
}

public abstract class PlayerStateBase2d : IPlayerState2D
{
    public virtual void EnterState(PlayerController2d player) { }
    public abstract void UpdateState(PlayerController2d player); 
    public virtual void FixedUpdateState(PlayerController2d player) { }
    public virtual void ExitState(PlayerController2d player) { }

    public virtual void OnCollisionEnter2D(Collision2D collision) { }
}

