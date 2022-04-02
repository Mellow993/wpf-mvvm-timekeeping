namespace Arbeitszeiterfassung.Client.ViewModel
{
    internal class ButtonControl
    {
        public enum State
        {
            None,
            Work,
            Break,
            ContinueWork,
            HomeTime
        }
        public State CurrentState { get; set; }
    }
}
