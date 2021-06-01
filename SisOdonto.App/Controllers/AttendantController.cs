using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SisOdonto.Application.Interfaces;
using SisOdonto.Application.Models.Attendant;
using SisOdonto.Application.Models.Attendant;
using SisOdonto.Domain.Interfaces.Notification;

namespace SisOdonto.App.Controllers
{
    public class AttendantController : BaseController
    {
        #region Fields

        private readonly IAttendantService _attendantService;

        #endregion Fields

        #region Constructors

        public AttendantController(INotifier notifier, IAttendantService attendantService)
        : base(notifier)
        {
            _attendantService = attendantService;
        }

        #endregion Constructors

        #region Methods

        public async Task<IActionResult> Index()
        {
            var attendants = await _attendantService.GetAll();

            return View(attendants);
        }

        [HttpGet]
        [Route("details-of-attendant/{id:guid}")]
        public async Task<IActionResult> Details(Guid id)
        {
            var attendant = await _attendantService.Get(id);

            if (ValidOperation() is false)
                return NotFound();

            return PartialView("_DetailsAttendant", attendant);
        }

        public async Task<IActionResult> CreateAttendant()
        {
            return PartialView("_CreateAttendant", new AttendantDataModel());
        }

        [HttpPost]
        public async Task<IActionResult> Create(AttendantDataModel request)
        {
            if (ModelState.IsValid is false)
                return PartialView("_CreateAttendant", request);

            await _attendantService.Create(request);

            if (ValidOperation() is false)
                return PartialView("_CreateAttendant", request);

            var url = Url.Action("Index", "Attendant");

            return Json(new { success = true, url, messageText = "Atendente Cadastrado com Sucesso!" });
        }

        [HttpGet]
        [Route("edit-attendant/{id:guid}")]
        public async Task<IActionResult> Edit(Guid id)
        {
            var attendant = await _attendantService.Get(id);

            if (ValidOperation() is false)
                return NotFound();

            return PartialView("_EditAttendant", attendant);
        }

        [HttpPost]
        [Route("edit-attendant/{id:guid}")]
        public async Task<IActionResult> Edit(Guid id, AttendantDataModel request)
        {
            if (ModelState.IsValid is false)
                return PartialView("_EditAttendant", request);

            request.Id = id;

            await _attendantService.Update(request);

            if (ValidOperation() is false)
                return PartialView("_EditAttendant", request);

            var url = Url.Action("Index", "Attendant");

            return Json(new { success = true, url });
        }

        [HttpGet]
        [Route("delete-attendant/{id:guid}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var attendant = await _attendantService.Get(id);

            if (ValidOperation() is false)
                return NotFound();

            return PartialView("_DeleteAttendant", attendant);
        }

        [HttpPost]
        [Route("delete-attendant/{id:guid}")]
        public async Task<IActionResult> Remove(Guid id)
        {
            if (ModelState.IsValid is false)
                return PartialView("_DeleteAttendant", await _attendantService.Get(id));

            await _attendantService.Delete(id);

            if (ValidOperation() is false)
                return PartialView("_DeleteAttendant", await _attendantService.Get(id));

            var url = Url.Action("Index", "Attendant");

            return Json(new { success = true, url });
        }

        #endregion Methods
    }
}