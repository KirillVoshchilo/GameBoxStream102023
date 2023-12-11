using App.Simples;
using UnityEngine;

namespace App.Content.Player
{
    public sealed class PlayerAnimationsEvents : MonoBehaviour
    {
        private readonly SEvent _onBonfireBuilded = new();
        private readonly SEvent _onAxeHited = new();
        private readonly SEvent _onBonfireSetted = new();
        private readonly SEvent _onStep = new();

        public SEvent OnBonfireBuilded => _onBonfireBuilded;
        public SEvent OnAxeHited => _onAxeHited;
        public SEvent OnBonfireSetted => _onBonfireSetted;
        public SEvent OnStep => _onStep;

        private void SetBonfire()
            => _onBonfireSetted.Invoke();
        private void AxeHit()
            => _onAxeHited.Invoke();
        private void BonfireBuildEnd()
            => _onBonfireBuilded.Invoke();
        private void Step()
            => _onStep.Invoke();
    }
}