namespace SharedKernel.Common
{
    public class KeyValueModel<TKey, TValue>
    {
        public TKey Key { get; set; }
        public TValue Value { get; set; }
    }
}
