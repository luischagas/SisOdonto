using Microsoft.AspNetCore.Identity;
using SisOdonto.Application.Interfaces;
using SisOdonto.Application.Models.Dentist;
using SisOdonto.Domain.Entities;
using SisOdonto.Domain.Interfaces.Notification;
using SisOdonto.Domain.Interfaces.Repositories;
using SisOdonto.Domain.Shared;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Security.Policy;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.WebUtilities;
using SisOdonto.Domain.Interfaces.Services;

namespace SisOdonto.Application.Services
{
    public class DentistService : AppService, IDentistService
    {
        #region Fields

        private readonly IDentistRepository _dentistRepository;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IEmailService _emailService;

        #endregion Fields

        #region Constructors

        public DentistService(IUnitOfWork unitOfWork,
            INotifier notifier,
            UserManager<IdentityUser> userManager,
            IDentistRepository dentistRepository,
            IEmailService emailService)
            : base(unitOfWork, notifier, userManager)
        {
            _userManager = userManager;
            _dentistRepository = dentistRepository;
            _emailService = emailService;
        }

        public async Task<DentistDataModel> Get(Guid id)
        {
            DentistDataModel dentistModel = null;

            var dentist = await _dentistRepository.GetAsync(id);

            if (dentist is null)
                Notify("Dados do Dentista não encontrado.");
            else
            {
                dentistModel = new DentistDataModel()
                {
                    Id = dentist.Id,
                    BirthDate = dentist.BirthDate,
                    Cep = dentist.Cep,
                    City = dentist.City,
                    Complement = dentist.Complement,
                    Cpf = dentist.Cpf,
                    Cro = dentist.Cro,
                    Street = dentist.Street,
                    Number = dentist.Number,
                    State = dentist.State,
                    District = dentist.District,
                    Email = dentist.Email,
                    Expertise = dentist.Expertise,
                    Name = dentist.Name
                };
            }

            return dentistModel;
        }

        public async Task<IEnumerable<DentistDataModel>> GetAll()
        {
            var dentists = await _dentistRepository.GetAllAsync();

            var dentistsModel = new List<DentistDataModel>();

            foreach (var dentist in dentists)
            {
                dentistsModel.Add(new DentistDataModel()
                {
                    Id = dentist.Id,
                    BirthDate = dentist.BirthDate,
                    Cep = dentist.Cep,
                    City = dentist.City,
                    Complement = dentist.Complement,
                    Cpf = dentist.Cpf,
                    Cro = dentist.Cro,
                    Street = dentist.Street,
                    Number = dentist.Number,
                    State = dentist.State,
                    District = dentist.District,
                    Email = dentist.Email,
                    Expertise = dentist.Expertise,
                    Name = dentist.Name
                });
            }

            return dentistsModel;
        }

        public async Task Create(DentistDataModel request)
        {
            var hasDentist = await _dentistRepository.GetAsync(request.Cpf);

            if (hasDentist is not null)
            {
                Notify("Já existe um dentista cadastrado com este cpf informado.");
                return;
            }

            var userId = Guid.NewGuid();

            if (request.CreateUser)
            {
                var user = new IdentityUser { UserName = request.Email, Email = request.Email };

                userId = Guid.Parse(user.Id);

                var password = GeneratePassword();

                var result = await _userManager.CreateAsync(user, password);

                if (result.Succeeded)
                {
                    var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);

                    await _userManager.ConfirmEmailAsync(user, code);

                    await AddClaimAsync(new Claim("kind", "dentist"), Guid.Parse(user.Id));

                    _emailService.SendEmail(user.Email, "Credenciais de Acesso - SisOdonto", "Credenciais", $"Usuário: {user.Email} <br> Senha: {password}");
                }
                else
                {
                    Notify("Erro ao criar usuário.");
                    return;
                }
            }
            
            var dentist = new Dentist(userId, request.BirthDate, request.Cep, request.City, request.Complement, request.Cpf, request.District, request.Email, request.Name, request.Number, request.State, request.Street, request.Cro, request.Expertise);

            if (dentist.IsValid())
                await _dentistRepository.AddAsync(dentist);
            else
            {
                var userManager = await _userManager.FindByIdAsync(userId.ToString());

                if (userManager is not null)
                    await _userManager.DeleteAsync(userManager);

                Notify(dentist.ValidationResult);
                return;
            }

            if (await CommitAsync() is false)
            {
                var userManager = await _userManager.FindByIdAsync(userId.ToString());

                if (userManager is not null)
                    await _userManager.DeleteAsync(userManager);
            }
        }

        public async Task Update(DentistDataModel request)
        {
            var dentist = await _dentistRepository.GetAsync(request.Id);

            if (dentist is null)
            {
                Notify("Dados do Dentista não encontrado.");
                return;
            }

            dentist.Update(request.BirthDate, request.Cep, request.City, request.Complement, request.Cpf, request.District, request.Email, request.Name, request.Number, request.State, request.Street, request.Cro, request.Expertise);

            if (dentist.IsValid())
                _dentistRepository.Update(dentist);
            else
            {
                Notify(dentist.ValidationResult);
                return;
            }

            await CommitAsync();
        }

        public async Task Delete(Guid id)
        {
            var dentist = await _dentistRepository.GetAsync(id);

            if (dentist is null)
            {
                Notify("Dados do Dentista não encontrado.");
                return;
            }

            dentist.Delete();

            if (dentist.IsValid())
                _dentistRepository.Update(dentist);
            else
            {
                Notify(dentist.ValidationResult);
                return;
            }

            if (await CommitAsync())
            {
                var userManager = await _userManager.FindByIdAsync(id.ToString());

                if (userManager is not null)
                    await _userManager.DeleteAsync(userManager);

            }
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