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
            var bankDetail = new BankDetail
            {
                BranchOffices = Utility.TryNodesToInnerTextByXPath(summaryDetail, XPathModel.Span, SummaryDetail.BranchOffices).RemoveWhitesSpaces(),
                ATMs = Utility.TryNodesToInnerTextByXPath(summaryDetail, XPathModel.Span, SummaryDetail.ATMs).RemoveWhitesSpaces(),
                Subagents = Utility.TryNodesToInnerTextByXPath(summaryDetail, XPathModel.Span, SummaryDetail.Subagents).RemoveWhitesSpaces(),
                Shareholders = Utility.TryNodesToInnerTextByXPath(summaryDetail, XPathModel.Span, SummaryDetail.Shareholders).RemoveWhitesSpaces(),
                RegistryNumber = Utility.TryNodesToInnerTextByXPath(generalInformation, XPathModel.Span, GeneralInformation.RegistryNumber).RemoveWhitesSpaces(),
                BusinessName = Utility.TryNodesToInnerTextByXPath(generalInformation, XPathModel.Span, GeneralInformation.BusinessName).RemoveWhitesSpaces(),
                RNC = Utility.TryNodesToInnerTextByXPath(generalInformation, XPathModel.Span, GeneralInformation.RNC).RemoveWhitesSpaces(),
                MainOffice = Utility.TryNodesToInnerTextByXPath(generalInformation, XPathModel.Span, GeneralInformation.MainOffice).RemoveWhitesSpaces(),
                Phone = Utility.TryNodesToInnerTextByXPath(generalInformation, XPathModel.Span, GeneralInformation.Phone).RemoveWhitesSpaces(),
                Email = Utility.TryNodesToInnerTextByXPath(generalInformation, XPathModel.Span, GeneralInformation.Email).RemoveWhitesSpaces(),
                WebPage = Utility.TryNodeToAttributeValue(generalInformation, XPathModel.A, GeneralInformation.WebPage, XPathModel.Href, BankUrls.BaseUrl).RemoveWhitesSpaces(),
                SocialNetworks = Utility.TryParse(() => generalInformation[GeneralInformation.SocialNetworks].SelectNodes(XPathModel.ExternalLinks).Select(node => new ExternalLinks {
                    Link = Utility.TryParse(() => node.GetAttributeValue(XPathModel.Href, string.Empty)),
                    Image = Utility.TryParse(() => string.Concat(BankUrls.BaseUrl, node.SelectSingleNode(XPathModel.Img).GetAttributeValue(XPathModel.Src, string.Empty))),
                    Name = Utility.TryParse(() => node.SelectSingleNode(XPathModel.Img).GetAttributeValue(XPathModel.Alt, string.Empty).Split(" ")[0]),
                }).ToList()),
                MobilesAppStore = Utility.TryParse(() => generalInformation[GeneralInformation.MobilesAppStore].SelectNodes(XPathModel.ExternalLinks).Select(node => new ExternalLinks
                {
                    Link = Utility.TryParse(() => node.GetAttributeValue(XPathModel.Href, string.Empty)),
                    Image = Utility.TryParse(() => string.Concat(BankUrls.BaseUrl, node.SelectSingleNode(XPathModel.Img).GetAttributeValue(XPathModel.Src, string.Empty))),
                    Name = Utility.TryParse(() => node.SelectSingleNode(XPathModel.Img).GetAttributeValue(XPathModel.Alt, string.Empty).Split(" ")[0]),
                }).ToList())
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
