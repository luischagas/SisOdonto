using Microsoft.AspNetCore.Mvc;
using SisOdonto.Application.Interfaces;
using SisOdonto.Application.Models.Patient;
using SisOdonto.Domain.Interfaces.Notification;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace SisOdonto.App.Controllers
{
    public class PatientController : BaseController
    {
        #region Fields

        private readonly IPatientService _patientService;
        private readonly IHealthInsuranceService _healthInsuranceService;

        #endregion Fields

        #region Constructors

        public PatientController(INotifier notifier, IPatientService patientService, IHealthInsuranceService healthInsuranceService)
        : base(notifier)
        {
            _patientService = patientService;
            _healthInsuranceService = healthInsuranceService;
        }

        #endregion Constructors

        #region Methods

        public async Task<IActionResult> Index()
        {
            var patients = await _patientService.GetAll();

            return View(patients);
        }

        [HttpGet]
        [Route("details-of-patient/{id:guid}")]
        public async Task<IActionResult> Details(Guid id)
        {
            var patient = await _patientService.Get(id);

            if (ValidOperation() is false)
                return NotFound();

            return PartialView("_DetailsPatient", patient);
        }

        [HttpGet]
        [Route("report")]
        public async Task<IActionResult> Report()
        {
            var patients = await _patientService.GetAllToReport();

            if (ValidOperation() is false)
                return NotFound();

            var patientsDataModel = new PatientDataModels
            {
                Patient = patients,
                Report = new ReportDataModel()
                {
                    CountHealthInsurancePatients = patients.Count(p => p.HealthInsurance is not null),
                    CountParticularPatients = patients.Count(p => p.HealthInsurance is null)
                }
            };

            return View("Report", patientsDataModel);
        }

        public async Task<IActionResult> CreatePatient()
        {
            var patientDataModel = new PatientDataModel
            {
                HealthInsurances = await _healthInsuranceService.GetAll()
            };


            return PartialView("_CreatePatient", patientDataModel);
        }

        [HttpPost]
        public async Task<IActionResult> Create(PatientDataModel request)
        {
            if (ModelState.IsValid is false)
            {
                request.HealthInsurances = await _healthInsuranceService.GetAll();

                return PartialView("_CreatePatient", request);
            }
            
            await _patientService.Create(request);

            if (ValidOperation() is false)
            {
                request.HealthInsurances = await _healthInsuranceService.GetAll();

                return PartialView("_CreatePatient", request);
            }
            
            var url = Url.Action("Index", "Patient");

            return Json(new { success = true, url, messageText = "Paciente Cadastrado com Sucesso!" });
        }

        [HttpGet]
        [Route("edit-patient/{id:guid}")]
        public async Task<IActionResult> Edit(Guid id)
        {
            var patient = await _patientService.Get(id);

            if (ValidOperation() is false)
                return NotFound();

            return PartialView("_EditPatient", patient);
        }

        [HttpPost]
        [Route("edit-patient/{id:guid}")]
        public async Task<IActionResult> Edit(Guid id, PatientDataModel request)
        {
            if (ModelState.IsValid is false)
            {
                request.HealthInsurances = await _healthInsuranceService.GetAll();

                return PartialView("_EditPatient", request);
            }
            
            request.Id = id;

            await _patientService.Update(request);

            if (ValidOperation() is false)
            {
                request.HealthInsurances = await _healthInsuranceService.GetAll();

                return PartialView("_EditPatient", request);
            }
            
            var url = Url.Action("Index", "Patient");

            return Json(new { success = true, url });
        }

        [HttpGet]
        [Route("delete-patient/{id:guid}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var patient = await _patientService.Get(id);

            if (ValidOperation() is false)
                return NotFound();

            return PartialView("_DeletePatient", patient);
        }

        [HttpPost]
        [Route("delete-patient/{id:guid}")]
        public async Task<IActionResult> Remove(Guid id)
        {
            if (ModelState.IsValid is false)
                PartialView("_DeletePatient", await _patientService.Get(id));

            await _patientService.Delete(id);

            if (ValidOperation() is false)
                return PartialView("_DeletePatient", await _patientService.Get(id));

            var url = Url.Action("Index", "Patient");

            return Json(new { success = true, url });
        }

        #endregion Methods
    }
}