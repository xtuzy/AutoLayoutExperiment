namespace Kiwi_Ts
{
    public enum SymbolType
    {
        Invalid,//无效
        External,//外部,外观
        Slack,//松的,懈怠的
        Error,
        Dummy//假的
    };


    public class Symbol:IKeyId
    {
        public static readonly Symbol Invalid = new Symbol(SymbolType.Invalid,-1);

        public SymbolType Type { get; }
        public int Id { get; }

        public int id() => Id;

        public Symbol()
        {
        }

        public Symbol(SymbolType type, int id)
        {
            Id = id;
            Type = type;
        }
    }
}
