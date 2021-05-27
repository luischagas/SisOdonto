using System;
using System.Collections.Generic;
using System.Linq;

namespace SisOdonto.Domain.Utils
{
    public class DocValidation
    {
        #region Fields

        public const int SizeCpf = 11;

        #endregion Fields

        #region Methods

        public static bool Validate(string cpf)
        {
            var cpfNumbers = Utils.OnlyNumbers(cpf);

            if (SizeValid(cpfNumbers) is false)
                return false;

            return HasRepeatedDigits(cpfNumbers) is false && HasValidDigits(cpfNumbers);
        }

        private static bool HasRepeatedDigits(string valor)
        {
            string[] invalidNumbers =
            {
                "00000000000",
                "11111111111",
                "22222222222",
                "33333333333",
                "44444444444",
                "55555555555",
                "66666666666",
                "77777777777",
                "88888888888",
                "99999999999"
            };
            return invalidNumbers.Contains(valor);
        }

        private static bool HasValidDigits(string value)
        {
            var number = value.Substring(0, SizeCpf - 2);

            var VerifyingDigit = new VerifyingDigit(number)
                .WithAteMultipliers(2, 11)
                .Replacing("0", 10, 11);

            var firstDigit = VerifyingDigit.CalculatesDigit();

            VerifyingDigit.AddDigit(firstDigit);

            var secondDigit = VerifyingDigit.CalculatesDigit();

            return string.Concat(firstDigit, secondDigit) == value.Substring(SizeCpf - 2, 2);
        }

        private static bool SizeValid(string valor)
        {
            return valor.Length == SizeCpf;
        }

        #endregion Methods
    }

    public class VerifyingDigit
    {
        #region Fields

        private const int Modulo = 11;
        private readonly List<int> _multipliers = new() { 2, 3, 4, 5, 6, 7, 8, 9 };
        private readonly IDictionary<int, string> _replacements = new Dictionary<int, string>();
        private bool _complementaryModule = true;
        private string _number;

        #endregion Fields

        #region Constructors

        public VerifyingDigit(string number)
        {
            _number = number;
        }

        #endregion Constructors

        #region Methods

        public void AddDigit(string digit)
        {
            _number = string.Concat(_number, digit);
        }

        public string CalculatesDigit()
        {
            return (_number.Length > 0) is false ? "" : GetDigitSum();
        }

        public VerifyingDigit Replacing(string substitute, params int[] digits)
        {
            foreach (var i in digits)
                _replacements[i] = substitute;

            return this;
        }

        public VerifyingDigit WithAteMultipliers(int firstMultiplier, int lastMultiplier)
        {
            _multipliers.Clear();

            for (var i = firstMultiplier; i <= lastMultiplier; i++)
                _multipliers.Add(i);

            return this;
        }

        private string GetDigitSum()
        {
            var sum = 0;

            for (int i = _number.Length - 1, m = 0; i >= 0; i--)
            {
                var product = (int)char.GetNumericValue(_number[i]) * _multipliers[m];
                sum += product;

                if (++m >= _multipliers.Count) m = 0;
            }

            var mod = (sum % Modulo);

            var result = _complementaryModule ? Modulo - mod : mod;

            return _replacements.ContainsKey(result) ? _replacements[result] : result.ToString();
        }

        #endregion Methods
    }
}