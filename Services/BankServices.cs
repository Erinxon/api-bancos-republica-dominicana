using HtmlAgilityPack;
using Microsoft.Extensions.Options;
using System.Threading.Tasks;
using System.Web;
using System.Linq;
using banks.Models;
using System.Collections.Generic;
using System;
using banks.Utilities;

namespace banks.Services
{
    public interface IBankServices
    {
        List<Bank> GetBank();
        BankDetail GetBankDetail();
        Task LoadHtmlDocument(string URL = null);
    }
    public class BankServices : IBankServices
    {
        private HtmlDocument htmlDocument;

        public List<Bank> GetBank()
        {
            var nodePrincipal = this.htmlDocument.DocumentNode.SelectNodes(XPathModel.ContentPageEntities);
            var banks = nodePrincipal.Select(n => new Bank { 
                Name = Utility.TryNodeToInnerText(n.SelectSingleNode(XPathModel.Name)).RemoveWhitesSpaces(),
                Image = Utility.TryNodeToAttributeValue(n.SelectSingleNode(XPathModel.Image), XPathModel.Src, BankUrls.BaseUrl),
                Type = Utility.TryNodeToInnerText(n.SelectSingleNode(XPathModel.Type)).RemoveWhitesSpaces(),
                TotalAssets = Utility.TryNodeToInnerText(n.SelectSingleNode(XPathModel.TotalAssets)).RemoveWhitesSpaces(),
                EmloyeeAmount = Utility.TryNodesToInnerText(n.SelectNodes(XPathModel.EmloyeeAmount), 1),
                TotalMarket = Utility.TryNodesToInnerText(n.SelectNodes(XPathModel.TotalMarket), 0),
                InfoDissolution = Utility.TryParse(() =>
                {
                    return n.SelectNodes(XPathModel.InfoDissolution).Select(node => new Dissolution
                    {
                        Description = Utility.TryNodesToInnerText(node.SelectNodes(XPathModel.P), InfoDissolution.Description),
                        LinkOfficialNotice = Utility.TryNodeToAttributeValue(node.SelectNodes(XPathModel.P), XPathModel.LinkOfficialNotice, InfoDissolution.LinkOfficialNotice, XPathModel.Href, BankUrls.BaseUrl),
                        LinkFrequentQuestions = Utility.TryNodeToAttributeValue(node.SelectNodes(XPathModel.P), XPathModel.A, InfoDissolution.LinkFrequentQuestions, XPathModel.Href, BankUrls.BaseUrl),
                    }).SingleOrDefault();
                }),
                Status = Utility.TryNodeToInnerText(n.SelectSingleNode(XPathModel.Status)).RemoveWhitesSpaces(),
                LinkDetail = Utility.TryNodeToAttributeValue(n.SelectSingleNode(XPathModel.LinkDetail), XPathModel.Href, BankUrls.BaseUrl).RemoveWhitesSpaces(),
            }).ToList();
            return banks;
        }

        public BankDetail GetBankDetail()
        {
            var summaryDetail = this.htmlDocument.DocumentNode.SelectNodes(XPathModel.SummaryDetail);
            var generalInformation = this.htmlDocument.DocumentNode.SelectNodes(XPathModel.GeneralInformation);
            HtmlNode authorizedOffer = null;
            var administrativeCouncil = this.htmlDocument.DocumentNode.SelectNodes(XPathModel.AdministrativeCouncil);
            var mainOfficials = this.htmlDocument.DocumentNode.SelectNodes(XPathModel.MainOfficials);
            var financialStatements = this.htmlDocument.DocumentNode.SelectNodes(XPathModel.FinancialStatements);
            var annualReports = this.htmlDocument.DocumentNode.SelectNodes(XPathModel.AnnualReports);
            if (generalInformation.Count > 9)
            {
                authorizedOffer = generalInformation[GeneralInformation.AuthorizedOffer];
                generalInformation.Remove(GeneralInformation.AuthorizedOffer);
            }
            var bankDetail = new BankDetail
            {
                BranchOffices = Utility.TryNodesToInnerTextByXPath(summaryDetail, XPathModel.Span, SummaryDetail.BranchOffices),
                ATMs = Utility.TryNodesToInnerTextByXPath(summaryDetail, XPathModel.Span, SummaryDetail.ATMs),
                Subagents = Utility.TryNodesToInnerTextByXPath(summaryDetail, XPathModel.Span, SummaryDetail.Subagents),
                Shareholders = Utility.TryNodesToInnerTextByXPath(summaryDetail, XPathModel.Span, SummaryDetail.Shareholders),
                RegistryNumber = Utility.TryNodesToInnerTextByXPath(generalInformation, XPathModel.Span, GeneralInformation.RegistryNumber),
                BusinessName = Utility.TryNodesToInnerTextByXPath(generalInformation, XPathModel.Span, GeneralInformation.BusinessName),
                RNC = Utility.TryNodesToInnerTextByXPath(generalInformation, XPathModel.Span, GeneralInformation.RNC),
                AuthorizedOffer = Utility.TryNodeToInnerTextByXPth(authorizedOffer, XPathModel.Span),
                MainOffice = Utility.TryNodesToInnerTextByXPath(generalInformation, XPathModel.Span, GeneralInformation.MainOffice),
                Phone = Utility.TryNodesToInnerTextByXPath(generalInformation, XPathModel.Span, GeneralInformation.Phone),
                Email = Utility.TryNodesToInnerTextByXPath(generalInformation, XPathModel.Span, GeneralInformation.Email),
                WebPage = Utility.TryNodeToAttributeValue(generalInformation, XPathModel.A, GeneralInformation.WebPage, XPathModel.Href, string.Empty),
                SocialNetworks = Utility.TryParse(() => generalInformation[GeneralInformation.SocialNetworks]?.SelectNodes(XPathModel.ExternalLinks)?.Select(node => new ExternalLinks {
                    Link = Utility.TryParse(() => node?.GetAttributeValue(XPathModel.Href, string.Empty)),
                    Image = Utility.TryParse(() => string.Concat(BankUrls.BaseUrl, node?.SelectSingleNode(XPathModel.Img)?.GetAttributeValue(XPathModel.Src, string.Empty))),
                    Name = Utility.TryParse(() => node?.SelectSingleNode(XPathModel.Img)?.GetAttributeValue(XPathModel.Title, string.Empty)?.Split(SplitWords.ExternalLinks)[0]),
                }).ToList()),
                MobilesAppStore = Utility.TryParse(() => generalInformation[GeneralInformation.MobilesAppStore]?.SelectNodes(XPathModel.ExternalLinks)?.Select(node => new ExternalLinks
                {
                    Link = Utility.TryParse(() => node?.GetAttributeValue(XPathModel.Href, string.Empty)),
                    Image = Utility.TryParse(() => string.Concat(BankUrls.BaseUrl, node?.SelectSingleNode(XPathModel.Img)?.GetAttributeValue(XPathModel.Src, string.Empty))),
                    Name = Utility.TryParse(() => node?.SelectSingleNode(XPathModel.Img)?.GetAttributeValue(XPathModel.Title, string.Empty)?.Split(SplitWords.ExternalLinks)[0]),
                }).ToList()),
                AdministrativeCouncil = Utility.TryParse(() => administrativeCouncil?.Select(a => new Employee
                {
                    Name = Utility.TryParse(() => a?.SelectSingleNode(XPathModel.Label).InnerText)?.RemoveWhitesSpaces(),
                    Position = Utility.TryParse(() => a?.SelectSingleNode(XPathModel.Span).InnerText)?.RemoveWhitesSpaces(),
                })).ToList(),
                MainOfficials = Utility.TryParse(() => mainOfficials?.Select(m => new Employee
                {
                    Name = Utility.TryParse(() => m?.SelectSingleNode(XPathModel.Label).InnerText)?.RemoveWhitesSpaces(),
                    Position = Utility.TryParse(() => m?.SelectSingleNode(XPathModel.Span).InnerText)?.RemoveWhitesSpaces(),
                })).ToList(),
                FinancialStatements = Utility.TryParse(() => financialStatements?.Select(f => new FileTimePeriod
                {
                   Link = Utility.TryParse(() => string.Concat(BankUrls.BaseUrl, f?.SelectSingleNode(XPathModel.A)?.GetAttributeValue(XPathModel.Href, string.Empty))),
                   Date = Utility.TryParse(() => f?.SelectSingleNode(XPathModel.A)?.InnerText)?.RemoveWhitesSpaces(),
                   Size = Utility.TryParse(() => f?.SelectSingleNode(XPathModel.Span)?.InnerText)?.Split(SplitWords.FileTimePeriod)[0]?.Replace(ReplaceText.FileTimePeriodSize, string.Empty)?.RemoveWhitesSpaces(),
                   Format = Utility.TryParse(() => f?.SelectSingleNode(XPathModel.Span)?.InnerText)?.Split(SplitWords.FileTimePeriod)[1]?.Replace(ReplaceText.FileTimePeriodFormat, string.Empty)?.RemoveWhitesSpaces(),
                })).ToList(),
                AnnualReports = Utility.TryParse(() => annualReports?.Select(f => new FileTimePeriod
                {
                    Link = Utility.TryParse(() => string.Concat(BankUrls.BaseUrl, f?.SelectSingleNode(XPathModel.A)?.GetAttributeValue(XPathModel.Href, string.Empty))),
                    Date = Utility.TryParse(() => f?.SelectSingleNode(XPathModel.A).InnerText)>.RemoveWhitesSpaces(),
                    Size = Utility.TryParse(() => f?.SelectSingleNode(XPathModel.Span).InnerText)?.Split(SplitWords.FileTimePeriod)[0]?.Replace(ReplaceText.FileTimePeriodSize, string.Empty)?.RemoveWhitesSpaces(),
                    Format = Utility.TryParse(() => f?.SelectSingleNode(XPathModel.Span).InnerText)?.Split(SplitWords.FileTimePeriod)[1]?.Replace(ReplaceText.FileTimePeriodFormat, string.Empty)?.RemoveWhitesSpaces(),
                })).ToList()
            };
            return bankDetail;
        }

        public async Task LoadHtmlDocument(string URL = null)
        {
            HtmlWeb htmlWeb = new();
            HtmlDocument htmlDoc = await htmlWeb.LoadFromWebAsync(URL is not null ? URL : string.Concat(BankUrls.BaseUrl, BankUrls.BankShortUrl));
            htmlDoc.LoadHtml(HttpUtility.HtmlDecode(htmlDoc.DocumentNode.InnerHtml.ToString()));
            this.htmlDocument = htmlDoc;
        }

    }
}
