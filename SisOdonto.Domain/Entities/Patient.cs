using SisOdonto.Domain.Enums.Patient;
using System;
using System.Collections.Generic;

namespace SisOdonto.Domain.Entities
{
    public class Patient : User
    {
        #region Fields

        private IList<Scheduling> _schedules;

        #endregion Fields

        #region Constructors

        public Patient(Guid id, DateTimeOffset birthDate, string cep, string city, string complement, string cpf, string district, string email, string name, string number, string state, Guid healthInsuranceId, string street, EMaritalStatus maritalStatus, EGender gender, string occupation, string telephone, string cellular)
        {
            Id = id;
            BirthDate = birthDate;
            Cep = cep;
            City = city;
            Complement = complement;
            Cpf = cpf;
            District = district;
            Email = email;
            Name = name;
            Number = number;
            State = state;
            HealthInsuranceId = healthInsuranceId;
            Street = street;
            MaritalStatus = maritalStatus;
            Gender = gender;
            Occupation = occupation;
            Telephone = telephone;
            Cellular = cellular;
            _schedules = new List<Scheduling>();
        }

        protected Patient()
        {
            _schedules = new List<Scheduling>();
        }

        #endregion Constructors

        #region Properties

        public string Cellular { get; private set; }
        public EGender Gender { get; private set; }
        public HealthInsurance HealthInsurance { get; private set; }
        public Guid HealthInsuranceId { get; private set; }
        public EMaritalStatus MaritalStatus { get; private set; }
        public string Occupation { get; private set; }
        public IEnumerable<Scheduling> Schedules => _schedules;
        public string Telephone { get; private set; }

        #endregion Properties

        #region Methods

        public void Update(DateTimeOffset birthDate, string cep, string city, string complement, string cpf, string district, string email, string name, string number, string state, string street, EMaritalStatus maritalStatus, EGender gender, string occupation, string telephone, string cellular)
        {
            BirthDate = birthDate;
            Cep = cep;
            City = city;
            Complement = complement;
            Cpf = cpf;
            District = district;
            Email = email;
            Name = name;
            Number = number;
            State = state;
            Street = street;
            MaritalStatus = maritalStatus;
            Gender = gender;
            Occupation = occupation;
            Telephone = telephone;
            Cellular = cellular;
        }

        public void SetHealthInsurance(HealthInsurance healthInsurance)
        {
            if (healthInsurance.IsValid())
                HealthInsurance = healthInsurance;

            AddErrors(healthInsurance.ValidationResult);
        }

        public void AddScheduling(Scheduling scheduling)
        {
            if (scheduling.IsValid())
                _schedules.Add(scheduling);

            AddErrors(scheduling.ValidationResult);
        }

        #endregion Methods
    }
}