public interface IPlayerState
{
    void Enter(PlayerMove3D player);
    void Exit(PlayerMove3D player);
    void Update(PlayerMove3D player);
    void FixedUpdate(PlayerMove3D player);
}

public abstract class PlayerState
{
    public virtual void Enter(PlayerMove3D player) { }
    public virtual void Exit(PlayerMove3D player) { }
    public virtual void Update(PlayerMove3D player) { }
    public virtual void FixedUpdate(PlayerMove3D player) { }
}


