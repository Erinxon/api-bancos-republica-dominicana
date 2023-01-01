namespace banks.Models
{
    public static class BankUrls
    {
        public const string BaseUrl = "https://sb.gob.do";
        public const string BankShortUrl = "/supervisados/entidades-de-intermediacion-financiera/?size=96";
    }
    public static class XPathModel
    {
        public const string ContentPageEntities = "//div[@class='content_page_entities main_layout_reverse bottom_border_radius']/div[@class='documents_list_section bottom_border_radius']/div[@class='entities_cards_container']/div";
        public const string Name = "a[@class='name_container']/span";
        public const string Type = "label[@class='entity_type_container']";
        public const string Image = "a[@class='img_container']/img";
        public const string TotalAssets = "div[@class='assets_info card colorBlueLight06 topBox']/div/span";
        public const string TotalMarket = "div[@class='total_market_employee_amount_container']/div/div/span";
        public const string EmloyeeAmount = "div[@class='total_market_employee_amount_container']/div/div/span";
        public const string InfoDissolution = "div[@class='description_info card colorBlueLight06']/div[@class='info_container']/label";
        public const string LinkOfficialNotice = "span/a";
        public const string Status = "div[@class='estatus_info card colorBlueLight06']/span";
        public const string LinkDetail = "div[@class='btn_details_container']/a";
        public const string SummaryDetail = "//div[@class='individual_page_entities main_layout_reverse bottom_border_radius']/div[@class='header_entity_section ']/div[@class='finantial_entity_info_container is_operating ']/div[@class='finantial_entity_info_card card colorBlue']/div";
        public const string GeneralInformation = "//div[@class='individual_page_entities main_layout_reverse bottom_border_radius']/div[@class='general_info_entity_section   ']/div/div[@class='general_info_box']/div";
        public const string Span = "span";
        public const string Href = "href";
        public const string Label = "label";
        public const string Src = "src";
        public const string A = "a";
        public const string Img = "img";
        public const string Alt = "alt";
        public const string P = "p";
        public const string Title = "title";
        public const string ExternalLinks = "div[@class='icons_container']/a";
        public const string AdministrativeCouncil = "//div[@class='employee_finantial_info_cards_container']/div[contains(@class, 'administrative_council_tile_cards_container')]/div/div[@class='title_sub_title_box']";
        public const string MainOfficials = "//div[@class='employee_finantial_info_cards_container']/div[contains(@class, 'main_officials_tile_cards_container')]/div/div[@class='title_sub_title_box']";
        public const string FinancialStatements = "//div[@class='employee_finantial_info_cards_container']/div[contains(@class, 'financial_statements_tile_cards_container')]/div/div[@class='title_sub_title_box']";
        public const string AnnualReports = "//div[@class='employee_finantial_info_cards_container']/div[contains(@class, 'annual_reports_tile_cards_container')]/div/div[@class='title_sub_title_box']";
    }

    public static class SummaryDetail
    {
        public const int BranchOffices = 0;
        public const int ATMs = 1;
        public const int Subagents = 2;
        public const int Shareholders = 3;
    }

    public static class GeneralInformation
    {
        public const int RegistryNumber = 0;
        public const int BusinessName = 1;
        public const int RNC = 2;
        public const int AuthorizedOffer = 3;
        public const int MainOffice = 3;
        public const int Phone = 4;
        public const int Email = 5;
        public const int WebPage = 6;
        public const int SocialNetworks = 7;
        public const int MobilesAppStore = 8;
    }

    public static class InfoDissolution
    {
        public const int Description = 0;
        public const int LinkOfficialNotice = 1;
        public const int LinkFrequentQuestions = 2;
    }

    public static class ReplaceText
    {
        public const string FileTimePeriodSize = "Tamaño:";
        public const string FileTimePeriodFormat = "Formato:";
    }

    public static class SplitWords
    {
        public const string FileTimePeriod = "-";
        public const string ExternalLinks = " ";
    }
}
