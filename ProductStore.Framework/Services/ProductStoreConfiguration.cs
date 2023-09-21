using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using ProductStore.Framework.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductStore.Framework.Services
{
    public class ProductStoreConfiguration: IConfigureOptions<JwtSettings>
    {
        private readonly IConfiguration _configuration;
        public ProductStoreConfiguration(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public void Configure(JwtSettings options)
        {
            _configuration.GetSection(nameof(JwtSettings)).Bind(options);
        }
    }
}
