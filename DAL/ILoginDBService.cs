using APDB_WebRESTapi.DTOs;
using APDB_WebRESTapi.DTOs.Requests;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace APDB_WebRESTapi.DAL
{
    public interface ILoginDBService
    {
        public TokenDTO GetToken(LoginRequest loginRequest);
        public TokenDTO RefreshToken(string refToken);
    }
}
