namespace WorldOfPowerTools.ProductService.Data
{
    public class EntityNotFoundException : Exception
    {
        public EntityNotFoundException(string message) : base(message) { }
    }
}