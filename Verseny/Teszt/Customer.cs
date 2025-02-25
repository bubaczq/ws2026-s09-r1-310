using System;

namespace Verseny
{
    public class Customer
    {
        public string id { get; set; } = string.Empty;
        public string firstName { get; set; } = string.Empty;
        public string lastName { get; set; } = string.Empty;
        public string age { get; set; } = string.Empty;
        public string gender { get; set; } = string.Empty;
        public string postalCode { get; set; } = string.Empty;
        public string email { get; set; } = string.Empty;
        public string phone { get; set; } = string.Empty;
        public string membership { get; set; } = string.Empty;
        public string joinedAt { get; set; } = string.Empty;
        public string lastPurchaseAt { get; set; } = string.Empty;
        public string totalSpending { get; set; } = string.Empty;
        public string averageOrderValue { get; set; } = string.Empty;
        public string frequency { get; set; } = string.Empty;
        public string preferredCategory { get; set; } = string.Empty;
        public string churned { get; set; } = string.Empty;

        public Customer(string csvLine)
        {
            string[] values = csvLine.Split(',');

            if (values.Length < 15) throw new ArgumentException("A CSV sor nem tartalmaz elegendő mezőt.");
            id = values[0]?.Trim() ?? "";
            firstName = values[1]?.Trim() ?? "";
            lastName = values[2]?.Trim() ?? "";
            age = values[3]?.Trim() ?? "";
            gender = values[4]?.Trim() ?? "";
            postalCode = values[5]?.Trim() ?? "";
            email = values[6]?.Trim() ?? "";
            phone = values[7]?.Trim() ?? "";
            membership = values[8]?.Trim() ?? "";
            joinedAt = values[9]?.Trim() ?? "";
            lastPurchaseAt = values[10]?.Trim() ?? "";
            totalSpending = values[11]?.Trim() ?? "";
            averageOrderValue = values[12]?.Trim() ?? "";
            frequency = values[13]?.Trim() ?? "";
            preferredCategory = values[14]?.Trim() ?? "";
            churned = values[15]?.Trim() ?? "";
        }
    }
}
