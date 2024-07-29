using System;

namespace api.Helpers
{
    public static class validarPdf
    {

        public static bool IsPdf(string mimeType)
        {
            return mimeType.Equals("application/pdf", StringComparison.OrdinalIgnoreCase);
        }
    }
}
