using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Localization;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ProductStore.Framework.Services
{
    public class LanguageService
    {
        /*private readonly IStringLocalizer<Resource> _localizer;
        private readonly ILogger<LanguageService> _logger;

        public LanguageService(IStringLocalizer<Resource> localizer, ILogger<LanguageService> logger)
        {
            _localizer = localizer;
            _logger = logger;
        }
        public CultureInfo GetRequestCulture(string language)
        {
            CultureInfo culture = new CultureInfo(language);
            return culture;
        }

        public string Translate(string key, string language)
        {
            try
            {
                CultureInfo culture = new CultureInfo(language);
                return _localizer[key,culture].Value;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to translate key: {Key}", key);
                return key; 
            }
        }*/
    }
}
