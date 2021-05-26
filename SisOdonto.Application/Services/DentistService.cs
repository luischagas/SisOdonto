using Microsoft.AspNetCore.Identity;
using SisOdonto.Domain.Entities;
using SisOdonto.Domain.Interfaces.Notification;
using SisOdonto.Domain.Interfaces.Repositories;
using SisOdonto.Domain.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SisOdonto.Application.Interfaces;
using SisOdonto.Application.Models.Dentist;

namespace SisOdonto.Application.Services
{
    public class DentistService : AppService, IDentistService
    {
        #region Fields

        private readonly IDentistRepository _dentistRepository;
        private readonly UserManager<IdentityUser> _userManager;

        #endregion Fields

        #region Constructors

        public DentistService(IUnitOfWork unitOfWork,
            INotifier notifier,
            UserManager<IdentityUser> userManager,
            IDentistRepository dentistRepository)
            : base(unitOfWork, notifier, userManager)
        {
            _userManager = userManager;
            _dentistRepository = dentistRepository;
        }

        public async Task Create(DentistDataModel request)
        {
            var hasDentist = await _dentistRepository.GetAsync(request.Cpf);

            if (hasDentist is not null)
            {
                Notify("Já existe um dentista cadastrado com este cpf informado.");
                return;
            }

            var user = new IdentityUser { UserName = request.Name, Email = request.Email };

            var result = await _userManager.CreateAsync(user, GeneratePassword());

            if (result.Succeeded)
            {
                var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);

                await _userManager.ConfirmEmailAsync(user, code);

                //AddClaimAsync(new Claim("Schedul", ""))
            }

            var dentist = new Dentist(Guid.Parse(user.Id), request.BirthDate, request.Cep, request.City, request.Complement, request.Cpf, request.District, request.Email, request.Name, request.Number, request.State, request.Street, request.Cro, request.Expertise);

            if (dentist.IsValid())
                await _dentistRepository.AddAsync(dentist);
            else
            {
                await _userManager.DeleteAsync(user);
                Notify(dentist.ValidationResult);
                return;
            }

            await CommitAsync();
        }

        public async Task<IEnumerable<DentistDataModel>> GetAll()
        {
            var dentists = await _dentistRepository.GetAllAsync();

            var dentistsModel = new List<DentistDataModel>();

            foreach (var dentist in dentists)
            {
                dentistsModel.Add(new DentistDataModel()
                {
                    BirthDate = dentist.BirthDate,
                    Cep = dentist.Cep,
                    City = dentist.City,
                    Complement = dentist.Complement,
                    Cpf = dentist.Cpf,
                    Cro = dentist.Cro,
                    District = dentist.District,
                    Email = dentist.Email, 
                    Expertise = dentist.Expertise,
                    Name = dentist.Name
                });
            }

            return dentistsModel;
        }

        private string GeneratePassword()
        {
            var options = _userManager.Options.Password;

            int length = options.RequiredLength;

            bool nonAlphanumeric = options.RequireNonAlphanumeric;
            bool digit = options.RequireDigit;
            bool lowercase = options.RequireLowercase;
            bool uppercase = options.RequireUppercase;

            StringBuilder password = new StringBuilder();
            Random random = new Random();

            while (password.Length < length)
            {
                char c = (char)random.Next(32, 126);

                password.Append(c);

                if (char.IsDigit(c))
                    digit = false;
                else if (char.IsLower(c))
                    lowercase = false;
                else if (char.IsUpper(c))
                    uppercase = false;
                else if (!char.IsLetterOrDigit(c))
                    nonAlphanumeric = false;
            }

            if (nonAlphanumeric)
                password.Append((char)random.Next(33, 48));
            if (digit)
                password.Append((char)random.Next(48, 58));
            if (lowercase)
                password.Append((char)random.Next(97, 123));
            if (uppercase)
                password.Append((char)random.Next(65, 91));

            return password.ToString();
        }

        #endregion Constructors
    }
}