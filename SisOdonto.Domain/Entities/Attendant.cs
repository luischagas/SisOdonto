using System;

namespace SisOdonto.Domain.Entities
{
    public class Attendant : User

    {
        #region Constructors

        public Attendant(Guid id, DateTimeOffset birthDate, string cep, string city, string complement, string cpf, string district, string email, string name, string number, string state, string street, string telephone, string cellular)
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
            Telephone = telephone;
            Cellular = cellular;
        }

        protected Attendant()
        {
        }

        #endregion Constructors

        #region Properties

        public string Cellular { get; private set; }

        public string Telephone { get; private set; }

        #endregion Properties

        #region Methods

        public void Update(DateTimeOffset birthDate, string cep, string city, string complement, string cpf, string district, string email, string name, string number, string state, string street, string telephone, string cellular)
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
            Telephone = telephone;
            Cellular = cellular;
        }

        #endregion Methods
    }
}