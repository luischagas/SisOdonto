using SisOdonto.Application.Models.Dentist;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using SisOdonto.Application.Models.Patient;
using SisOdonto.Domain.Enums.Dentist;
using SisOdonto.Domain.Enums.Scheduling;

namespace SisOdonto.Application.Models.Scheduling
{
    public class SchedulingDataModel
    {
        #region Properties

        public Guid Id { get; set; }

        [Required(ErrorMessage = "O campo {0} é obrigatório")]
        [DisplayName("Data e Hora")]
        public DateTime DateTime { get; set; }

        [DisplayName("Observação")]
        public string Obs { get; set; }

        [Required(ErrorMessage = "O campo {0} é obrigatório")]
        [DisplayName("Tipo de Consulta")]
        public ETypeScheduling Type { get; set; }

        [DisplayName("Tipo de Especialidade")]
        public EExpertise TypeExpertise { get; set; }

        [Required(ErrorMessage = "O campo {0} é obrigatório")]
        [DisplayName("Dentista")]
        public Guid DentistId { get; set; }

        public DentistDataModel Dentist { get; set; }

        public IEnumerable<DentistDataModel> Dentists { get; set; }

        [DisplayName("Patient")]
        public Guid PatientId { get; set; }


        public PatientDataModel Patient { get; set; }

        public IEnumerable<PatientDataModel> Patients { get; set; }

        #endregion Properties
    }
}