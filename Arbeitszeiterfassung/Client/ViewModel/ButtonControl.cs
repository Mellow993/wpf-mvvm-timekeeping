namespace Arbeitszeiterfassung.Client.ViewModel
{
    class ButtonControl
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
