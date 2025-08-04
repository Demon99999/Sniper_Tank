using System;

namespace Assets.Scripts.Data
{
    [Serializable]
    public class Wallet
    {
        public uint Value;

        public Wallet(uint startValue)
        {
            Value = startValue;
        }

        public event Action ValueChanged;

        public bool TryTake(uint value)
        {
            if (value > Value)
            {
                return false;
            }

            Value -= value;
            ValueChanged?.Invoke();

            return true;
        }

        public void Give(uint value)
        {
            Value += value;
        }
    }
}
