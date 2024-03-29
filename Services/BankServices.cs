﻿using HtmlAgilityPack;
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
            var banks = nodePrincipal.Select(n => new Bank
            {
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
            if (generalInformation?.Count > 9)
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
                SocialNetworks = generalInformation.GetExternalLinks(GeneralInformation.SocialNetworks),
                MobilesAppStore = generalInformation.GetExternalLinks(GeneralInformation.MobilesAppStore),
                AdministrativeCouncil = administrativeCouncil.GetEmployees(),
                MainOfficials = mainOfficials.GetEmployees(),
                FinancialStatements = financialStatements.GetFileTimePeriods(),
                AnnualReports = annualReports.GetFileTimePeriods()
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