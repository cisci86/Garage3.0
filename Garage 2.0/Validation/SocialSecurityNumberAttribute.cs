using System.ComponentModel.DataAnnotations;

namespace Garage_2._0.Validation
{
    public class SocialSecurityNumberAttribute : ValidationAttribute
    {
        const string errorMessage = "Enter in format: YYYYMMDD-xxxx";

        protected override ValidationResult IsValid(object? value, ValidationContext validationContext)
        {
            if (value is string input)
            {
                var divided = input.Split('-');
                //Checks that the format is right (only two parts after dividing with -)
                if (divided.Length != 2)
                    return new ValidationResult(errorMessage);

                var dateOfBirth = divided[0];

                //Checks that the length of the birth-date is correct
                if (dateOfBirth.Length != 8)
                    return new ValidationResult(errorMessage);

                //Checks that it is only numbers
                if (!int.TryParse(dateOfBirth, out _))
                    return new ValidationResult(errorMessage);

                //goes through several checks to see that the date of birth is correct
                var (sucess, message) = CheckDateOfBirth(dateOfBirth);
                if (!sucess)
                    return new ValidationResult(message);

                //the same checks for the last 4 digits (length and that it is numbers)
                var controllnumber = divided[1];

                if (controllnumber.Length != 4)
                    return new ValidationResult(errorMessage);

                if (!int.TryParse(controllnumber, out _))
                    return new ValidationResult(errorMessage);

                //Checks that the last of the 4 digits is correct (controlnumber)
                if(!CheckControllNumber(dateOfBirth, controllnumber))
                    return new ValidationResult("Check the last 4 digits");

                return ValidationResult.Success;
            }
            return new ValidationResult(errorMessage);
        }
        private (bool, string) CheckDateOfBirth(string dateOfBirth)
        {
            int year = int.Parse(dateOfBirth.AsSpan(0, 4));
            int month = int.Parse(dateOfBirth.AsSpan(4, 2));
            int date = int.Parse(dateOfBirth.AsSpan(6, 2));

            if (year < 1900 || year > DateTime.Now.Year)
                return (false, $"Enter a correct year (1900 - {DateTime.Now.Year})");

            if (month < 1 || month > 12)
                return (false, ("Enter a correct month"));

            //Checks if the month is February and then if it is a Leap year or not and check that the date is in range
            if (month == 2 && !FebruaryValidation(year, date))
                return (false, ("Enter a correct date"));

            int[] monthWith31Days = { 1, 3, 5, 7, 8, 10, 12 };
            if (month != 2 && monthWith31Days.Contains(month) && (date < 1 || date > 31))
                return (false, ("Enter a correct date"));

            if (month != 2 && !monthWith31Days.Contains(month) && (date < 1 || date > 30))
                return (false, ("Enter correct date"));
            return (true, (""));
        }
        // checks if the date is in range depending if it is a Leap year or not 
        private bool FebruaryValidation(int year, int date)
        {
            if (IsLeapYear(year) && (date < 1 || date > 29))
                return false;

            if (!IsLeapYear(year) && (date < 1 || date > 28))
                return false;

            return true;
        }

        //Checks if it is a Leap year (don't check for the division by 400 because it's not a problem right now)
        private bool IsLeapYear(int year)
        {
            if (year % 4 == 0)
                return true;
            return false;
        }

        private bool CheckControllNumber(string dateOfBirth, string numbers)
        {
            string numbersToCheck = dateOfBirth.Substring(2) + numbers;
            string newNumbers = "";
            //Takes number 1,3,5,7,9 and multiply it by 2 and number 2,4,6,8 as they are (should by multiplied by 1)
            for (int i = 0; i < numbersToCheck.Length - 1; i++)
            {
                if (i % 2 == 0)
                    newNumbers += (int.Parse(numbersToCheck[i].ToString()) * 2).ToString();
                else
                    newNumbers += numbersToCheck[i].ToString();
            }
            //Sets the last of the 4 digits (control number)
            int controlNumber = int.Parse(numbers[3].ToString());
            int controlNumberToCheck = 0;

            //goes true all of the new numbers one by one and adds them together
            foreach(char n in newNumbers)
            {
                controlNumberToCheck += int.Parse(n.ToString());
            }

            //The formula to calculate the correct control number
            controlNumberToCheck = (10 - (controlNumberToCheck % 10)) % 10;

            if(controlNumberToCheck == controlNumber)
                return true;

            return false;
        }
    }
}
