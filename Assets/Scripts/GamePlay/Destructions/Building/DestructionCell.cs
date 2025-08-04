using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.GamePlay.Destructions.Building
{
    [RequireComponent(typeof(Rigidbody))]
    public class DestructionCell : DestructionPart, IDamageable, IDestructablePart
    {
        [SerializeField] private List<DestructionCell> _neighboringCells;
        [SerializeField] private bool _isFoundation = false;

        public event Action<Vector3, uint> Destructed;

        private bool _isBreaked;
        
        public Transform Transform => transform;

        private void Start()
        {
            _neighboringCells = new List<DestructionCell>();

            _isBreaked = false;
        }

        private void OnDrawGizmos()
        {
            if (_neighboringCells == null)
                return;

            if (_isFoundation)
                Gizmos.color = Color.blue;
            else if (IsConnectedToFondation(new List<DestructionCell>()) && _isBreaked == false)
                Gizmos.color = Color.yellow;
            else
                Gizmos.color = Color.red;

            Gizmos.DrawSphere(transform.position, 0.2f);

            foreach (DestructionCell destructionCell in _neighboringCells)
            {
                if (destructionCell != null)
                    Gizmos.DrawLine(transform.position, destructionCell.transform.position);
            }
        }

        public void Clear() =>
            _neighboringCells.Clear();

        public bool Contains(DestructionCell destructionCell) =>
            _neighboringCells.Contains(destructionCell);

        public void AddNeighboring(DestructionCell destructionCell)
        {
            if (Contains(destructionCell) == false)
                _neighboringCells.Add(destructionCell);
        }

        public void TakeDamage(ExplosionInfo explosionInfo) =>
            Destruct(explosionInfo.ExplosionPosition, explosionInfo.ExplosionForce);

        public void ReportDestruction(List<DestructionCell> checkedCells, Vector3 bulletPosition, uint explosionForce)
        {
            if (IsConnectedToFondation(checkedCells) == false)
                Destruct(bulletPosition, explosionForce);
        }

        public bool IsConnectedToFondation(List<DestructionCell> checkedCells)
        {
            if (_isFoundation)
                return true;

            checkedCells.Add(this);

            foreach (DestructionCell destructionCell in _neighboringCells)
            {
                if (destructionCell == null)
                    continue;
                else if (checkedCells.Contains(destructionCell) || destructionCell._isBreaked)
                    continue;
                else if (destructionCell.IsConnectedToFondation(checkedCells))
                    return true;
            }

            return false;
        }

        public override void Destruct(Vector3 bulletPosition, uint explosionForce)
        {
            if (_isBreaked || _isFoundation)
                return;

            Destructed?.Invoke(bulletPosition, explosionForce);

            _isBreaked = true;

            foreach (DestructionCell neigboringCell in _neighboringCells)
                neigboringCell.ReportDestruction(new List<DestructionCell>(), bulletPosition, explosionForce);

            base.Destruct(bulletPosition, explosionForce);
        }
    }
}