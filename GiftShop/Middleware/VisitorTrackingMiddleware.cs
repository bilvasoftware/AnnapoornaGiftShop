using GiftShop.Models;
using GiftShop.Repositories.Interfaces;

namespace GiftShop.Middleware
{
    public class VisitorTrackingMiddleware
    {
        private readonly RequestDelegate _next;

        private const string VisitorCookieName = "AnnapoornaVisitor";

        public VisitorTrackingMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(
            HttpContext context,
            IWebsiteVisitorRepository visitorRepository)
        {
            // Track only GET requests from the customer website.
            // Admin, CSS, JS, images and other static files are excluded.
            if (context.Request.Method == "GET" &&
                !context.Request.Path.StartsWithSegments("/Admin") &&
                !context.Request.Path.StartsWithSegments("/css") &&
                !context.Request.Path.StartsWithSegments("/js") &&
                !context.Request.Path.StartsWithSegments("/images") &&
                !context.Request.Path.StartsWithSegments("/favicon"))
            {
                string visitorKey;

                if (context.Request.Cookies.TryGetValue(
                    VisitorCookieName,
                    out var existingVisitorKey) &&
                    !string.IsNullOrWhiteSpace(existingVisitorKey))
                {
                    visitorKey = existingVisitorKey;
                }
                else
                {
                    visitorKey = Guid.NewGuid().ToString();

                    context.Response.Cookies.Append(
                        VisitorCookieName,
                        visitorKey,
                        new CookieOptions
                        {
                            HttpOnly = true,
                            IsEssential = true,
                            Expires = DateTimeOffset.Now.AddYears(1),
                            SameSite = SameSiteMode.Lax
                        });
                }

                var visitor = new WebsiteVisitor
                {
                    VisitorKey = visitorKey,

                    IPAddress =
                        context.Connection.RemoteIpAddress?.ToString(),

                    PageUrl = context.Request.Path,

                    Browser =
                        context.Request.Headers.UserAgent.ToString(),

                    Device = GetDevice(
                        context.Request.Headers.UserAgent.ToString()),

                    VisitDate = DateTime.Now
                };

                await visitorRepository.AddAsync(visitor);
            }

            await _next(context);
        }

        private string GetDevice(string userAgent)
        {
            if (string.IsNullOrEmpty(userAgent))
                return "Unknown";

            if (userAgent.Contains(
                    "Mobile",
                    StringComparison.OrdinalIgnoreCase) ||
                userAgent.Contains(
                    "Android",
                    StringComparison.OrdinalIgnoreCase) ||
                userAgent.Contains(
                    "iPhone",
                    StringComparison.OrdinalIgnoreCase))
            {
                return "Mobile";
            }

            if (userAgent.Contains(
                    "Tablet",
                    StringComparison.OrdinalIgnoreCase) ||
                userAgent.Contains(
                    "iPad",
                    StringComparison.OrdinalIgnoreCase))
            {
                return "Tablet";
            }

            return "Desktop";
        }
    }
}