namespace CoC.Bot.Data
{
    using System.Collections.Generic;

    public class TroopRequestDictionary : Dictionary<Troop, TroopRequestData>
    {
        public void Add(Troop key, string keyboards, bool isDonateAll)
        {
            var val = new TroopRequestData() { Keyboards = keyboards, IsDonateAll = isDonateAll };
            Add(key, val);
        }
    }

    public class TroopRequestData
    {
        public string Keyboards;
        public bool IsDonateAll;
    }
}