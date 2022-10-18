namespace Zenject.Signals
{
    public class ChangeMoneySignal
    {
        public readonly int MoneyDelta;
        
        public ChangeMoneySignal(int moneyDelta)
        {
            MoneyDelta = moneyDelta;
        }
    }
}