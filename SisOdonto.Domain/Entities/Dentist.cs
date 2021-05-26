using SisOdonto.Domain.Enums;
using System;
using System.Collections.Generic;
using SisOdonto.Domain.Enums.Dentist;

namespace SisOdonto.Domain.Entities
{
    public class Dentist : User

    {
        #region Fields

        private IList<Scheduling> _schedules;

        #endregion Fields

        #region Constructors

        public Dentist(Guid id, DateTimeOffset birthDate, string cep, string city, string complement, string cpf, string district, string email, string name, string number, string state, string street, string cro, EExpertise expertise)
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
            Street = street;
            Cro = cro;
            Expertise = expertise;
            _schedules = new List<Scheduling>();
        }

        protected Dentist()
        {
            _schedules = new List<Scheduling>();
        }

        #endregion Constructors

        #region Properties

        public string Cro { get; private set; }

        public EExpertise Expertise { get; private set; }

        public IEnumerable<Scheduling> Schedules => _schedules;

        #endregion Properties

        #region Methods

        public void AddScheduling(Scheduling scheduling)
        {
            if (scheduling.IsValid())
                _schedules.Add(scheduling);
        }

        public void Update(DateTimeOffset birthDate, string cep, string city, string complement, string cpf, string district, string email, string name, string number, string state, string street, string cro, EExpertise expertise)
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
            Cro = cro;
            Expertise = expertise;
        }

        #endregion Methods
    }
}