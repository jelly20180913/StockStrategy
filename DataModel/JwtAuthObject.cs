 

namespace DataModel
{
    public class JwtAuthObject
    {
        public int Id { get; set; }
        public long exp { get; set; }
        public long iat { get; set; }
    }
}
