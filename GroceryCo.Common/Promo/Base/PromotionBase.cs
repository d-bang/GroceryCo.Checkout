using System;
using GroceryCo.Common.Promo.Interfaces;
using Newtonsoft.Json;

namespace GroceryCo.Common.Promo.Base
{
    public abstract class PromotionBase : IPromotion
    {
        [JsonProperty]
        private Guid itemId;
        [JsonProperty]
        private DateTime startDate;
        [JsonProperty]
        private DateTime endDate;
        [JsonProperty]
        private string description;

        public PromotionBase(DateTime Start, DateTime End, Guid ItemId, string Description)
        {
            //ValidateDates(start, end);

            startDate = Start;
            endDate = End;
            itemId = ItemId;
            description = Description;
        }

        public DateTime StartDate { get => startDate; }
        public DateTime EndDate { get => endDate; }
        public Guid ItemId { get => itemId; }
        public string Description { get => description; }

        protected void ValidateDates(DateTime start, DateTime end)
        {
            if (start >= end)
                throw new ArgumentException("The promotion start date/time must be earlier than the end date/time");
        }
    }
 }
