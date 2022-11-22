using System.Collections.Generic;

namespace banks.Models
{
    public class Bank
    {
        public string Name { get; set; }
        public string Type { get; set; }
        public string Image { get; set; }
        public string TotalAssets { get; set; }
        public string TotalMarket { get; set; }
        public string EmloyeeAmount { get; set; }
        public Dissolution InfoDissolution { get; set; }
        public string Status { get; set; }
        public string LinkDetail { get; set; }
    }

    public class Dissolution
    {
        public string Description { get; set; }
        public string LinkOfficialNotice { get; set; }
        public string LinkFrequentQuestions { get; set; }
    }

    public class BankDetail
    {
        public string BranchOffices { get; set; }
        public string ATMs { get; set; }
        public string Subagents { get; set; }
        public string Shareholders { get; set; }
        public string RegistryNumber { get; set; }
        public string BusinessName { get; set; }
        public string RNC { get; set; }
        public string MainOffice { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string WebPage { get; set; }
        public List<ExternalLinks> SocialNetworks { get; set; }
        public List<ExternalLinks> MobilesAppStore { get; set; }
    }

    public class ExternalLinks
    {
        public string Name { get; set; }
        public string Link { get; set; }
        public string Image { get; set; }
    }

}
