using Microsoft.AspNetCore.Mvc.Razor;
using System;
using SisOdonto.Domain.Enums.Dentist;

namespace SisOdonto.Infrastructure.CrossCutting.Extensions
{
    public static class RazorExtensions
    {
        #region Methods

        public static string FormatDocument(this RazorPage page, string document)
        {
            return Convert.ToUInt64(document).ToString(@"000\.000\.000\-00");
        }

        public static string FormatDate(this RazorPage page, DateTime date)
        {
            return date.ToString("dd/MM/yyyy");
        }


        public static string FormatHour(this RazorPage page, DateTime date)
        {
            return date.ToString("H:mm");
        }


        public static string FormatExpertise(this RazorPage page, EExpertise document)
        {
            switch (document)
            {
                case EExpertise.GeneralPractitioner:
                {
                    return "Clínico Geral";
                }
                case EExpertise.Orthodontics:
                {
                    return "Ortodondia";
                }
                case EExpertise.Implantology:
                {
                    return "Implantodontia";
                }
                case EExpertise.Endodontics:
                {
                    return "Endodontia";
                }
            }

            return "";

           
        }

        #endregion Methods
    }
}