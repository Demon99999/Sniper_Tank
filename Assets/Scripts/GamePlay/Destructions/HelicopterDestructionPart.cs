using UnityEngine;

namespace Assets.Scripts.GamePlay.Destructions
{
    public class HelicopterDestructionPart : CollidingDestructionPart
    {
        public bool IsDesturcted { get; private set; }

        public override void Destruct(Vector3 explosionPosition, uint explosionForce)
        {
            IsDesturcted = true;
            base.Destruct(explosionPosition, explosionForce);
        }
    }
}
