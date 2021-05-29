using Microsoft.AspNetCore.Mvc;
using SisOdonto.Application.Interfaces;
using SisOdonto.Application.Models.Scheduling;
using SisOdonto.Domain.Interfaces.Notification;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using SisOdonto.Application.Models.Patient;
using SisOdonto.Infrastructure.CrossCutting.Extensions;

namespace SisOdonto.App.Controllers
{
    public class SchedulingController : BaseController
    {
        #region Fields

        private readonly ISchedulingService _schedulingService;
        private readonly IDentistService _dentistService;
        private readonly IPatientService _patientService;
        private readonly UserManager<IdentityUser> _userManager;

        #endregion Fields

        #region Constructors

        public SchedulingController(INotifier notifier, 
            ISchedulingService schedulingService, 
            IDentistService dentistService, 
            IPatientService patientService, 
            UserManager<IdentityUser> userManager)
            : base(notifier)
        {
            _schedulingService = schedulingService;
            _dentistService = dentistService;
            _patientService = patientService;
            _userManager = userManager;
        }

        #endregion Constructors

        #region Methods

        public async Task<IActionResult> Index()
        {
            var userId = _userManager.GetUserId(User);

            var schedules = await _schedulingService.GetAll(Guid.Parse(userId));

            return View(schedules);
        }

        [HttpGet]
        [Route("details-of-scheduling/{id:guid}")]
        public async Task<IActionResult> Details(Guid id)
        {
            var scheduling = await _schedulingService.Get(id);

            if (ValidOperation() is false)
                return NotFound();

            return PartialView("_DetailsScheduling", scheduling);
        }

        [HttpGet]
        [ActionName("DetailsByCpf")]
        public async Task<IActionResult> DetailsByCpf(string cpf)
        {
            var patient = await _patientService.GetByCpf(cpf);

            if (ValidOperation() is false)
                return Json(new { success = false });

            return Json(new {success = true, data = patient});
        }

        [HttpGet]
        [ActionName("GetExpertise")]
        public async Task<IActionResult> GetExpertise(Guid dentistId)
        {
            var dentist = await _dentistService.Get(dentistId);

            if (ValidOperation() is false)
                return Json(new { success = false });

            return Json(new { success = true, data = dentist.Expertise.GetDisplayValue() });
        }

        public async Task<IActionResult> CreateScheduling()
        {
            var schedulingDataModel = new SchedulingDataModel()
            {
                Dentists = await _dentistService.GetAll()
            };

            return PartialView("_CreateScheduling", schedulingDataModel);
        }

        [HttpPost]
        public async Task<IActionResult> Create(SchedulingDataModel request)
        {
            await _schedulingService.Create(request);

            if (ValidOperation() is false)
            {
                request.Dentists = await _dentistService.GetAll();
                return PartialView("_CreateScheduling", request);
            }

            var url = Url.Action("Index", "Scheduling");

            return Json(new { success = true, url, messageText = "Agendamento Cadastrado com Sucesso!" });
        }

        [HttpGet]
        [Route("edit-scheduling/{id:guid}")]
        public async Task<IActionResult> Edit(Guid id)
        {
            var scheduling = await _schedulingService.Get(id);

            if (ValidOperation() is false)
                return NotFound();

            return PartialView("_EditScheduling", scheduling);
        }

        [HttpPost]
        [Route("edit-scheduling/{id:guid}")]
        public async Task<IActionResult> Edit(Guid id, SchedulingDataModel request)
        {
            request.Id = id;

            await _schedulingService.Update(request);

            if (ValidOperation() is false)
            {
                request.Dentists = await _dentistService.GetAll();

                return PartialView("_EditScheduling", request);
            }

            var url = Url.Action("Index", "Scheduling");

            return Json(new { success = true, url });
        }

        [HttpGet]
        [Route("delete-scheduling/{id:guid}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var scheduling = await _schedulingService.Get(id);

            if (ValidOperation() is false)
                return NotFound();

            return PartialView("_DeleteScheduling", scheduling);
        }

        [HttpPost]
        [Route("delete-scheduling/{id:guid}")]
        public async Task<IActionResult> Remove(Guid id)
        {
            var userId = _userManager.GetUserId(User);

            if (ModelState.IsValid is false)
                return PartialView("Index", await _schedulingService.GetAll(Guid.Parse(userId)));

            await _schedulingService.Delete(id);

            if (ValidOperation() is false)
                return PartialView("Index", await _schedulingService.GetAll(Guid.Parse(userId)));

            var url = Url.Action("Index", "Scheduling");

            return Json(new { success = true, url });
        }

        #endregion Methods
    }
}