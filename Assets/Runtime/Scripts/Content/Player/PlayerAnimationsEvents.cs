using App.Simples;
using UnityEngine;

public sealed class PlayerAnimationsEvents : MonoBehaviour
{
    private readonly SEvent _onBonfireBuilded = new();
    private readonly SEvent _onAxeHited = new();
    private readonly SEvent _onBonfireSetted = new();

    public SEvent OnBonfireBuilded => _onBonfireBuilded;
    public SEvent OnAxeHited => _onAxeHited;
    public SEvent OnBonfireSetted => _onBonfireSetted;

    private void SetBonfire()
    {
        _onBonfireSetted.Invoke();
    }
    private void AxeHit()
    {
        _onAxeHited.Invoke();
    }
    private void BonfireBuildEnd()
    {
        _onBonfireBuilded.Invoke();
    }
}
