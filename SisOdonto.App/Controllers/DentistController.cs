using Microsoft.AspNetCore.Mvc;
using SisOdonto.Application.Interfaces;
using SisOdonto.Application.Models.Dentist;
using SisOdonto.Domain.Interfaces.Notification;
using System;
using System.Threading.Tasks;

namespace SisOdonto.App.Controllers
{
    public class DentistController : BaseController
    {
        #region Fields

        private readonly IDentistService _dentistService;
        
        #endregion Fields

        #region Constructors

        public DentistController(INotifier notifier, IDentistService dentistService)
        : base(notifier)
        {
            _dentistService = dentistService;
        }

        #endregion Constructors

        #region Methods

        public async Task<IActionResult> Index()
        {
            var dentists = await _dentistService.GetAll();

            return View(dentists);
        }

        [HttpGet]
        [Route("details-of-dentist/{id:guid}")]
        public async Task<IActionResult> Details(Guid id)
        {
            var dentist = await _dentistService.Get(id);

            if (ValidOperation() is false)
                return NotFound();

            return PartialView("_DetailsDentist", dentist);
        }

        public async Task<IActionResult> CreateDentist()
        {
            return PartialView("_CreateDentist", new DentistDataModel());
        }

        [HttpPost]
        public async Task<IActionResult> Create(DentistDataModel createDentistRequest)
        {
            if (ModelState.IsValid is false)
                return PartialView("_CreateDentist", createDentistRequest);

            await _dentistService.Create(createDentistRequest);

            if (ValidOperation() is false)
                return PartialView("_CreateDentist", createDentistRequest);

            var url = Url.Action("Index", "Dentist");

            return Json(new { success = true, url, messageText = "Dentista Cadastrado com Sucesso!" });
        }

        [HttpGet]
        [Route("edit-dentist/{id:guid}")]
        public async Task<IActionResult> Edit(Guid id)
        {
            var dentist = await _dentistService.Get(id);

            if (ValidOperation() is false)
                return NotFound();

            return PartialView("_EditDentist", dentist);
        }

        [HttpPost]
        [Route("edit-dentist/{id:guid}")]
        public async Task<IActionResult> Edit(Guid id, DentistDataModel createDentistRequest)
        {
            if (ModelState.IsValid is false)
                return PartialView("_EditDentist", createDentistRequest);

            createDentistRequest.Id = id;

            await _dentistService.Update(createDentistRequest);

            if (ValidOperation() is false)
                return PartialView("_EditDentist", createDentistRequest);

            var url = Url.Action("Index", "Dentist");

            return Json(new { success = true, url });
        }

        [HttpGet]
        [Route("delete-dentist/{id:guid}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var dentist = await _dentistService.Get(id);

            if (ValidOperation() is false)
                return NotFound();

            return PartialView("_DeleteDentist", dentist);
        }

        [HttpPost]
        [Route("delete-dentist/{id:guid}")]
        public async Task<IActionResult> Remove(Guid id)
        {
            if (ModelState.IsValid is false)
                return PartialView("_DeleteDentist", await _dentistService.Get(id));

            await _dentistService.Delete(id);

            if (ValidOperation() is false)
                return PartialView("_DeleteDentist", await _dentistService.Get(id));

            var url = Url.Action("Index", "Dentist");

            return Json(new { success = true, url });
        }

        #endregion Methods
    }
}