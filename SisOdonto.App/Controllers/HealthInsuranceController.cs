using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SisOdonto.Application.Interfaces;
using SisOdonto.Application.Models.HealthInsurance;
using SisOdonto.Application.Models.HealthInsurance;
using SisOdonto.Domain.Interfaces.Notification;

namespace SisOdonto.App.Controllers
{
    public class HealthInsuranceController : BaseController
    {
        #region Fields

        private readonly IHealthInsuranceService _healthInsuranceService;

        #endregion Fields

        #region Constructors

        public HealthInsuranceController(INotifier notifier, IHealthInsuranceService healthInsuranceService)
        : base(notifier)
        {
            _healthInsuranceService = healthInsuranceService;
        }

        #endregion Constructors

        #region Methods

        public async Task<IActionResult> Index()
        {
            var healthInsurances = await _healthInsuranceService.GetAll();

            return View(healthInsurances);
        }

        [HttpGet]
        [Route("details-of-health-insurance/{id:guid}")]
        public async Task<IActionResult> Details(Guid id)
        {
            var healthInsurance = await _healthInsuranceService.Get(id);

            if (ValidOperation() is false)
                return NotFound();

            return PartialView("_DetailsHealthInsurance", healthInsurance);
        }

        public async Task<IActionResult> CreateHealthInsurance()
        {
            return PartialView("_CreateHealthInsurance", new HealthInsuranceDataModel());
        }

        [HttpPost]
        public async Task<IActionResult> Create(HealthInsuranceDataModel request)
        {
            if (ModelState.IsValid is false)
                return PartialView("_CreateHealthInsurance", request);

            await _healthInsuranceService.Create(request);

            if (ValidOperation() is false)
                return PartialView("_CreateHealthInsurance", request);

            var url = Url.Action("Index", "HealthInsurance");

            return Json(new { success = true, url, messageText = "Convênio Cadastrado com Sucesso!" });
        }

        [HttpGet]
        [Route("edit-health-insurance/{id:guid}")]
        public async Task<IActionResult> Edit(Guid id)
        {
            var healthInsurance = await _healthInsuranceService.Get(id);

            if (ValidOperation() is false)
                return NotFound();

            return PartialView("_EditHealthInsurance", healthInsurance);
        }

        [HttpPost]
        [Route("edit-health-insurance/{id:guid}")]
        public async Task<IActionResult> Edit(Guid id, HealthInsuranceDataModel request)
        {
            if (ModelState.IsValid is false)
                return PartialView("_EditHealthInsurance", request);

            request.Id = id;

            await _healthInsuranceService.Update(request);

            if (ValidOperation() is false)
                return PartialView("_EditHealthInsurance", request);

            var url = Url.Action("Index", "HealthInsurance");

            return Json(new { success = true, url });
        }

        [HttpGet]
        [Route("delete-health-insurance/{id:guid}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var healthInsurance = await _healthInsuranceService.Get(id);

            if (ValidOperation() is false)
                return NotFound();

            return PartialView("_DeleteHealthInsurance", healthInsurance);
        }

        [HttpPost]
        [Route("delete-health-insurance/{id:guid}")]
        public async Task<IActionResult> Remove(Guid id)
        {
            if (ModelState.IsValid is false)
                return PartialView("_DeleteHealthInsurance", await _healthInsuranceService.Get(id));

            await _healthInsuranceService.Delete(id);

            if (ValidOperation() is false)
                return PartialView("_DeleteHealthInsurance", await _healthInsuranceService.Get(id));

            var url = Url.Action("Index", "HealthInsurance");

            return Json(new { success = true, url });
        }

        #endregion Methods
    }
}