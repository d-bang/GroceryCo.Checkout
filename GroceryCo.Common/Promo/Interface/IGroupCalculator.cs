namespace GroceryCo.Common.Promo.Interfaces
{
    public interface IGroupCalculator : IBatchCalculator
    {
        //i.e. Buy three - GroupNumber - to apply a flat rate price to.
        int GroupNumber { get; }
    }
}
