using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using SisOdonto.App.ViewModels;
using SisOdonto.Application.Interfaces;
using SisOdonto.Domain.Interfaces.Notification;
using System.Threading.Tasks;
using SisOdonto.Application.Models.Dentist;

namespace SisOdonto.App.Controllers
{
    public class DentistaController : BaseController
    {
        #region Fields

        private readonly IDentistService _dentistService;

        #endregion Fields

        #region Constructors

        public DentistaController(INotifier notifier, IDentistService dentistService)
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

            var url = Url.Action("Index", "Dentista");

            return Json(new { success = true, url });
        }

       

        #endregion Methods
    }
}