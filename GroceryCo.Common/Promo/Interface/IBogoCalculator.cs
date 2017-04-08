namespace GroceryCo.Common.Promo.Interfaces
{
    public interface IBogoCalculator : IBatchCalculator
    {
        //i.e. Buy One - the purchase number
        int PurchaseNumber { get; }

        //Get one free - the equivalentModifier : multiply by 0
        decimal EquivalentModifier { get; }
    }
}
