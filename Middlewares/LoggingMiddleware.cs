using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APDB_WebRESTapi.Middlewares
{
    public class LoggingMiddleware
    {
        private readonly RequestDelegate _next;
        public LoggingMiddleware(RequestDelegate next)
        {
            _next = next;
        }
        public async Task InvokeAsync(HttpContext httpContext)
        {
            httpContext.Request.EnableBuffering();

            var path = httpContext.Request.Path;
            var query = httpContext.Request?.QueryString.ToString();
            var method = httpContext.Request.Method.ToString();
            var bodyStream = string.Empty;

            using (StreamReader reader
             = new StreamReader(httpContext.Request.Body, Encoding.UTF8, true, 1024, true))
            {
                bodyStream = await reader.ReadToEndAsync();
                httpContext.Request.Body.Position = 0;

            }

            var FileName = "./Middlewares/requestsLog.txt";

            byte[] data = Encoding.UTF8.GetBytes($"Path: {path}; Method: {method}; Query: {query}; Body: {bodyStream};\n");
            FileStream fs = new FileStream(FileName, FileMode.Append);
            fs.Write(data, 0, data.Length);
            fs.Close();

            await _next(httpContext);
        }

    }
}
