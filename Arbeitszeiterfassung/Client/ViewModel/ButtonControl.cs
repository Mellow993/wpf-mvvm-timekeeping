namespace Arbeitszeiterfassung.Client.ViewModel
{
    internal class ButtonControl
    {
        public enum State
        {
            None,
            Work,
            Break,
            P6,
            ContinueWork,
            HomeTime
        }
        public State CurrentState { get; set; }
    }
}
