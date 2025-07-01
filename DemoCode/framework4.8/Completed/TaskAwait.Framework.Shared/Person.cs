using System;

namespace TaskAwait.Framework.Shared
{
    public class Person
    {
        public int Id { get; set; }
        public string GivenName { get; set; }
        public string FamilyName { get; set; }
        public DateTime StartDate { get; set; }
        public int Rating { get; set; }
        public string FormatString { get; set; }

        public Person(int id, string givenName, string familyName,
            DateTime startDate, int rating, string formatString = "")
        {
            Id = id;
            GivenName = givenName;
            FamilyName = familyName;
            StartDate = startDate;
            Rating = rating;
            FormatString = formatString;
        }

        public override string ToString()
        {
            if (string.IsNullOrEmpty(FormatString))
            {
                return $"{GivenName} {FamilyName}";
            }
            return string.Format(FormatString, GivenName, FamilyName);
        }
    }
}
