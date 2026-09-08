using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace CMNirdesh.Models
{
    public static class FileValidation
    {
        // Allowed safe extensions
        private static readonly HashSet<string> AllowedExtensions = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
        {
            ".jpg", ".jpeg", ".png", ".gif"
        };

        // Explicitly blocked dangerous extensions
        private static readonly HashSet<string> BlockedExtensions = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
        {
            ".exe", ".cmd", ".sh", ".ps1", ".bat", ".msi",
            ".aspx", ".php", ".jsp", ".jspx", ".pl", ".py", ".rb", ".cgi"
        };

        // Allowed image MIME types
        private static readonly HashSet<string> AllowedMimeTypes = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
        {
            "image/jpeg", "image/png", "image/gif"
        };

        // Check file extension safety
        public static bool IsExtensionSafe(string fileName)
        {
            if (string.IsNullOrWhiteSpace(fileName))
                return false;

            string ext = Path.GetExtension(fileName);
            if (string.IsNullOrEmpty(ext))
                return false;

            // block dangerous extensions directly
            if (BlockedExtensions.Contains(ext))
                return false;

            // only allow known image extensions
            return AllowedExtensions.Contains(ext);
        }

        // Check if MIME type is allowed (based on actual upload header)
        public static bool IsMimeTypeSafe(HttpPostedFileBase file)
        {
            if (file == null)
                return false;

            string mimeType = file.ContentType;
            return AllowedMimeTypes.Contains(mimeType);
        }

        // Prevent double extensions (like "shell.aspx.jpg")
        public static bool HasNoDoubleExtension(string fileName)
        {
            if (string.IsNullOrWhiteSpace(fileName))
                return false;

            int dotCount = Path.GetFileName(fileName).Count(c => c == '.');
            return dotCount <= 1;
        }

        // Validate complete file
        public static bool IsValidImageUpload(HttpPostedFileBase file)
        {
            if (file == null || file.ContentLength == 0)
                return false;

            string fileName = file.FileName;

            // Combine all validations
            return IsExtensionSafe(fileName)
                && HasNoDoubleExtension(fileName)
                && IsMimeTypeSafe(file);
        }
    }
}
