namespace FIFA_API.Models.Controllers
{
    public class StripeSuccessResult
    {
        public int IdCommande { get; set; }
        public string EstimateUnit { get; set; }
        public long EstimateMaxValue { get; set; }
    }
}
