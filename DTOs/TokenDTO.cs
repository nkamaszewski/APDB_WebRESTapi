using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace APDB_WebRESTapi.DTOs
{
    public class TokenDTO
    {
        public string AccessToken { get; set; }
        public Guid RefreshToken { get; set; }
    }
}
