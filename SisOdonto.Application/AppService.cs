using System;
using System.Linq;
using System.Security.Claims;
using SisOdonto.Domain.Shared;
using System.Threading.Tasks;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Identity;
using SisOdonto.Domain.Entities;
using SisOdonto.Domain.Interfaces.Notification;
using SisOdonto.Domain.Notification;

namespace SisOdonto.Application
{
    public abstract class AppService
    {
        #region Fields

        private readonly INotifier _notifier;

        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<IdentityUser> _userManager;

        #endregion Fields

        #region Constructors

        public AppService(IUnitOfWork unitOfWork, INotifier notifier, UserManager<IdentityUser> userManager)
        {
            _unitOfWork = unitOfWork;
            _notifier = notifier;
            _userManager = userManager;
        }

        #endregion Constructors

        #region Methods

        public async Task<bool> CommitAsync()
        {
            if (await _unitOfWork.CommitAsync())
                return await Task.FromResult(true);

            return await Task.FromResult(false);
        }

        protected void Notify(ValidationResult validationResult)
        {
            foreach (var error in validationResult.Errors)
                Notify(error.ErrorMessage);
        }

        protected void Notify(string mensagem)
        {
            _notifier.Handle(new Notification(mensagem));
        }

        protected bool ExecutarValidacao<TV, TE>(TV validacao, TE entidade) where TV : AbstractValidator<TE> where TE : Entity<TE>
        {
            var validator = validacao.Validate(entidade);

            if (validator.IsValid) return true;

            Notify(validator);

            return false;
        }

        public async Task AddClaimAsync(Claim claim, Guid userId)
        {
            var user = await _userManager.FindByIdAsync(Convert.ToString(userId));

            var userClaims = await _userManager.GetClaimsAsync(user);

            if (userClaims.Any(c => string.Equals(c.Type, claim.Type, StringComparison.CurrentCultureIgnoreCase)) &&
                userClaims.Any(c => string.Equals(c.Value, claim.Value, StringComparison.CurrentCultureIgnoreCase)))
                return;

            var userClaim = userClaims.FirstOrDefault(uc => string.Equals(uc.Type, claim.Type, StringComparison.CurrentCultureIgnoreCase));

            if (userClaim != null)
                await _userManager.RemoveClaimAsync(user, userClaim);

            await _userManager.AddClaimAsync(user, claim);
        }

        #endregion Methods
    }
}