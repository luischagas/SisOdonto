using Microsoft.AspNetCore.Mvc.Razor;
using System;

namespace SisOdonto.Infrastructure.CrossCutting.Extensions
{
    public static class RazorExtensions
    {
        #region Methods

        public static string FormatDocument(this RazorPage page,  string document)
        {
            return Convert.ToUInt64(document).ToString(@"000\.000\.000\-00");
        }

        #endregion Methods
    }
}