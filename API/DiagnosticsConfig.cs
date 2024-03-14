using System.Diagnostics;

namespace API
{
    public static class DiagnosticsConfig
    {
        public const string ServiceName = "MyService";
        public static ActivitySource ActivitySource = new ActivitySource(ServiceName);
    }
}
