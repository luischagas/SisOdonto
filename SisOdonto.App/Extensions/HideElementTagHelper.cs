using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.AspNetCore.Routing;
using System;

namespace SisOdonto.App.Extensions
{
    [HtmlTargetElement("*", Attributes = "supress-by-action")]
    public class ApagaElementoByActionTagHelper : TagHelper
    {
        #region Fields

        private readonly IHttpContextAccessor _contextAccessor;

        #endregion Fields

        #region Constructors

        public ApagaElementoByActionTagHelper(IHttpContextAccessor contextAccessor)
        {
            _contextAccessor = contextAccessor;
        }

        #endregion Constructors

        #region Properties

        [HtmlAttributeName("supress-by-action")]
        public string ActionName { get; set; }

        #endregion Properties

        #region Methods

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            if (context == null)
                throw new ArgumentNullException(nameof(context));
            if (output == null)
                throw new ArgumentNullException(nameof(output));

            var action = _contextAccessor.HttpContext.GetRouteData().Values["action"].ToString();

            if (ActionName.Contains(action)) return;

            output.SuppressOutput();
        }

        #endregion Methods
    }

    [HtmlTargetElement("*", Attributes = "supress-by-claim-name")]
    [HtmlTargetElement("*", Attributes = "supress-by-claim-value")]
    public class HideElementTagHelper : TagHelper
    {
        #region Fields

        private readonly IHttpContextAccessor _contextAccessor;

        #endregion Fields

        #region Constructors

        public HideElementTagHelper(IHttpContextAccessor contextAccessor)
        {
            _contextAccessor = contextAccessor;
        }

        #endregion Constructors

        #region Properties

        [HtmlAttributeName("supress-by-claim-name")]
        public string IdentityClaimName { get; set; }

        [HtmlAttributeName("supress-by-claim-value")]
        public string IdentityClaimValue { get; set; }

        #endregion Properties

        #region Methods

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            if (context == null)
                throw new ArgumentNullException(nameof(context));
            if (output == null)
                throw new ArgumentNullException(nameof(output));

            var hasAcess = CustomAuthorization.ValidarClaimsUsuario(_contextAccessor.HttpContext, IdentityClaimName, IdentityClaimValue);

            if (hasAcess) 
                return;

            output.SuppressOutput();
        }

        #endregion Methods
    }
}