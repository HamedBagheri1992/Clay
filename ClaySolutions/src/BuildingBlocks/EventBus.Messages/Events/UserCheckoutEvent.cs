namespace EventBus.Messages.Events
{
    public class UserCheckoutEvent : IntegrationBaseEvent
    {
        public long UserId { get; set; }
        public string UserName { get; set; }
        public string DisplayName { get; set; }
    }
}
