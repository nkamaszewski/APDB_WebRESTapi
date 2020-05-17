using APDB_WebRESTapi.DTOs;
using APDB_WebRESTapi.DTOs.Requests;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace APDB_WebRESTapi.DAL
{
    public class LoginDBService : ILoginDBService
    {
        private string SqlConn = "Data Source=db-mssql;Initial Catalog=s16456;Integrated Security=True";
        private IConfiguration _configuration;
        public LoginDBService(IConfiguration Configuration)
        {
            _configuration = Configuration;
        }
        public TokenDTO GetToken(LoginRequest loginRequest) {

            using (var client = new SqlConnection(SqlConn))
            using (var command = new SqlCommand())
            {
                command.Connection = client;
                command.CommandText = $"SELECT * FROM Student WHERE IndexNumber=@login AND Password=@password";
                command.Parameters.AddWithValue("login", loginRequest.Login);
                command.Parameters.AddWithValue("password", loginRequest.Password);

                client.Open();
                var dataReader = command.ExecuteReader();

                if (!dataReader.Read()) {
                    return null;
                };

               

                var claims = new[]
           {
                new Claim(ClaimTypes.NameIdentifier, dataReader["IndexNumber"].ToString()),
                new Claim(ClaimTypes.Name, dataReader["FirstName"].ToString() + "_" + dataReader["LastName"].ToString()),
                new Claim(ClaimTypes.Role, "admin"),
                new Claim(ClaimTypes.Role, "student")
            };

                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["SecretKey"]));
                var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

                var token = new JwtSecurityToken(issuer: "Gakko", audience: "Students", claims: claims, expires: DateTime.Now.AddMinutes(10), signingCredentials: creds);

                Guid newRefreshToken = Guid.NewGuid();
                
                dataReader.Close();
                command.Parameters.Clear();

                command.CommandText = $"UPDATE Student SET RefreshToken=@newRefreshToken WHERE IndexNumber=@login";
                command.Parameters.AddWithValue("newRefreshToken", newRefreshToken);
                command.Parameters.AddWithValue("login", loginRequest.Login);
                command.ExecuteReader();

                return new TokenDTO()
                {
                    AccessToken = new JwtSecurityTokenHandler().WriteToken(token),
                    RefreshToken = newRefreshToken,
                };

            }
        }

        public TokenDTO RefreshToken(string refToken)
        {
            using (var client = new SqlConnection(SqlConn))
            using (var command = new SqlCommand())
            {
                command.Connection = client;
                command.CommandText = $"SELECT * FROM Student WHERE RefreshToken=@refToken";
                command.Parameters.AddWithValue("refToken", refToken);
            
                client.Open();
                var dataReader = command.ExecuteReader();

                if (!dataReader.Read())
                {
                    return null;
                };

                var indexNumber = dataReader["IndexNumber"].ToString();

                var claims = new[]
           {
                new Claim(ClaimTypes.NameIdentifier, dataReader["IndexNumber"].ToString()),
                new Claim(ClaimTypes.Name, dataReader["FirstName"].ToString() + "_" + dataReader["LastName"].ToString()),
                new Claim(ClaimTypes.Role, "admin"),
                new Claim(ClaimTypes.Role, "student")
            };

                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["SecretKey"]));
                var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

                var token = new JwtSecurityToken(issuer: "Gakko", audience: "Students", claims: claims, expires: DateTime.Now.AddMinutes(10), signingCredentials: creds);

                Guid newRefreshToken = Guid.NewGuid();

                dataReader.Close();

                command.CommandText = $"UPDATE Student SET RefreshToken=@newRefreshToken WHERE IndexNumber=@indexNumber";
                command.Parameters.AddWithValue("newRefreshToken", newRefreshToken);
                command.Parameters.AddWithValue("indexNumber", indexNumber);

                command.ExecuteReader();

                return new TokenDTO()
                {
                    AccessToken = new JwtSecurityTokenHandler().WriteToken(token),
                    RefreshToken = newRefreshToken,
                };
            }
        }
    }
}
