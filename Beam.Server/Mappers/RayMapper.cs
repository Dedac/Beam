namespace Beam.Server.Mappers
{
    public static class RayMapper
    {
        public static Shared.Ray ToShared(this Data.Ray r)
        {
            return new Shared.Ray()
            {
                RayId = r.RayId,
                Text = r.Text,
                FrequencyId = r.FrequencyId,
                PrismCount = 0,
                UserName = "billy"
            };
        }
        public static Data.Ray ToData(this Shared.Ray r)
        {
            return new Data.Ray()
            {
                RayId = r.RayId,
                Text = r.Text,
                FrequencyId = r.FrequencyId
            };
        }
    }
}
