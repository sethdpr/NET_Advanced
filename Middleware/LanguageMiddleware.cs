using System.Globalization;

namespace NET_Advanced.Middleware
{
    public class LanguageMiddleware
    {
        private readonly RequestDelegate _next;

        public LanguageMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var languageCookie = context.Request.Cookies["Language"];

            if (string.IsNullOrEmpty(languageCookie))
            {
                languageCookie = "nl-NL";
            }

            try
            {
                var cultureInfo = new CultureInfo(languageCookie);
                CultureInfo.CurrentCulture = cultureInfo;
                CultureInfo.CurrentUICulture = cultureInfo;
            }

            catch (CultureNotFoundException)
            {
                CultureInfo.CurrentCulture = new CultureInfo("nl-NL");
                CultureInfo.CurrentUICulture = new CultureInfo("nl-NL");
            }

            await _next(context);
        }
    }
}