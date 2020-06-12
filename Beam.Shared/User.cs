using System.Collections.Generic;
namespace Beam.Shared
{
    public class User
    {
        public bool IsAuthenticated { get; set; }
        public int Id { get; set; }
        public string Name { get; set; }
        public Dictionary<string, string> Claims { get; set; }
    }
}
