using Microsoft.AspNetCore.Identity;
using SisOdonto.Application.Interfaces;
using SisOdonto.Application.Models.Attendant;
using SisOdonto.Domain.Entities;
using SisOdonto.Domain.Interfaces.Notification;
using SisOdonto.Domain.Interfaces.Repositories;
using SisOdonto.Domain.Interfaces.Services;
using SisOdonto.Domain.Shared;
using SisOdonto.Domain.Utils;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace SisOdonto.Application.Services
{
    public class AttendantService : AppService, IAttendantService
    {
        #region Fields

        private readonly IAttendantRepository _attendantRepository;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IEmailService _emailService;
        #endregion Fields

        #region Constructors

        public AttendantService(IUnitOfWork unitOfWork,
            INotifier notifier,
            UserManager<IdentityUser> userManager,
            IAttendantRepository attendantRepository,
            IEmailService emailService)
            : base(unitOfWork, notifier, userManager)
        {
            _userManager = userManager;
            _attendantRepository = attendantRepository;
            _emailService = emailService;
        }

        public async Task<AttendantDataModel> Get(Guid id)
        {
            AttendantDataModel attendantModel = null;

            var attendant = await _attendantRepository.GetAsync(id);

            if (attendant is null)
                Notify("Dados do Atendente não encontrado.");
            else
            {
                attendantModel = new AttendantDataModel()
                {
                    Id = attendant.Id,
                    BirthDate = attendant.BirthDate,
                    Cep = attendant.Cep,
                    City = attendant.City,
                    Complement = attendant.Complement,
                    Cpf = attendant.Cpf,
                    Cellular = attendant.Cellular,
                    Street = attendant.Street,
                    Number = attendant.Number,
                    State = attendant.State,
                    District = attendant.District,
                    Email = attendant.Email,
                    Telephone = attendant.Telephone,
                    Name = attendant.Name
                };
            }

            return attendantModel;
        }

        public async Task<IEnumerable<AttendantDataModel>> GetAll()
        {
            var attendants = await _attendantRepository.GetAllAsync();

            var attendantsModel = new List<AttendantDataModel>();

            foreach (var attendant in attendants)
            {
                attendantsModel.Add(new AttendantDataModel()
                {
                    Id = attendant.Id,
                    BirthDate = attendant.BirthDate,
                    Cep = attendant.Cep,
                    City = attendant.City,
                    Complement = attendant.Complement,
                    Cpf = attendant.Cpf,
                    Cellular = attendant.Cellular,
                    Street = attendant.Street,
                    Number = attendant.Number,
                    State = attendant.State,
                    District = attendant.District,
                    Email = attendant.Email,
                    Telephone = attendant.Telephone,
                    Name = attendant.Name
                });
            }

            return attendantsModel;
        }

        public async Task Create(AttendantDataModel request)
        {
            var hasAttendant = await _attendantRepository.GetAsync(request.Cpf);

            if (hasAttendant is not null)
            {
                Notify("Já existe um atendente cadastrado com este cpf informado.");
                return;
            }

            var userId = Guid.NewGuid();

            if (request.CreateUser)
            {
                var user = new IdentityUser { UserName = request.Email, Email = request.Email };

                userId = Guid.Parse(user.Id);

                var options = _userManager.Options.Password;

                var password = Utils.GeneratePassword(options.RequiredLength, options.RequireNonAlphanumeric, options.RequireDigit, options.RequireLowercase, options.RequireUppercase);

                var result = await _userManager.CreateAsync(user, password);

                if (result.Succeeded)
                {
                    var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);

                    await _userManager.ConfirmEmailAsync(user, code);

                    await AddClaimAsync(new Claim("kind", "attendant"), Guid.Parse(user.Id));

                    _emailService.SendEmail(user.Email, "Credenciais de Acesso - SisOdonto", "Credenciais", $"Usuário: {user.Email} <br> Senha: {password}");
                }
                else
                {
                    Notify("Erro ao criar usuário.");
                    return;
                }
            }

            var attendant = new Attendant(userId, request.BirthDate, request.Cep, request.City, request.Complement, request.Cpf.Replace(".", "").Replace("-", ""), request.District, request.Email, request.Name, request.Number, request.State, request.Street, request.Telephone.Replace("(", "").Replace(")", "").Replace("-", "").Trim(), request.Cellular.Replace("(", "").Replace(")", "").Replace("-", "").Trim());

            if (attendant.IsValid())
                await _attendantRepository.AddAsync(attendant);
            else
            {
                var userManager = await _userManager.FindByIdAsync(userId.ToString());

                if (userManager is not null)
                    await _userManager.DeleteAsync(userManager);

                Notify(attendant.ValidationResult);
                return;
            }

            if (await CommitAsync() is false)
            {
                Notify("Erro ao salvar dados.");

                var userManager = await _userManager.FindByIdAsync(userId.ToString());

                if (userManager is not null)
                    await _userManager.DeleteAsync(userManager);
            }
        }

        public async Task Update(AttendantDataModel request)
        {
            var attendant = await _attendantRepository.GetAsync(request.Id);

            if (attendant is null)
            {
                Notify("Dados do Atendente não encontrado.");
                return;
            }

            attendant.Update(request.BirthDate, request.Cep, request.City, request.Complement, request.Cpf.Replace(".", "").Replace("-", ""), request.District, request.Email, request.Name, request.Number, request.State, request.Street, request.Telephone.Replace("(", "").Replace(")", "").Replace("-", "").Trim(), request.Cellular.Replace("(", "").Replace(")", "").Replace("-", "").Trim());

            if (attendant.IsValid())
                _attendantRepository.Update(attendant);
            else
            {
                Notify(attendant.ValidationResult);
                return;
            }

            if (await CommitAsync() is false)
                Notify("Erro ao salvar dados.");
        }

        public async Task Delete(Guid id)
        {
            var attendant = await _attendantRepository.GetAsync(id);

            if (attendant is null)
            {
                Notify("Dados do atendente não encontrado.");
                return;
            }

            attendant.Delete();

            if (attendant.IsValid())
                _attendantRepository.Update(attendant);
            else
            {
                Notify(attendant.ValidationResult);
                return;
            }

            if (await CommitAsync())
            {
                var userManager = await _userManager.FindByIdAsync(id.ToString());

                if (userManager is not null)
                    await _userManager.DeleteAsync(userManager);
            } else
                Notify("Erro ao salvar dados.");
        }

        #endregion Constructors
    }
}