﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Documents;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using StockMonitor.Models.ApiModels;
using StockMonitor.Models.JSONModels;

namespace StockMonitor.Helpers
{
    public static class RetrieveJsonDataHelper
    {
        private const string FmgBaseUrl = "https://financialmodelingprep.com/api/v3";
        private const string FmgDataDailyUrl = "/historical-price-full/";
        private const string FmgCompanyProfileUrl = "/company/profile/";
        private const string FmgMajorIndexesUrl = "/majors-indexes/";
        private const string FmgQuoteOnlyPriceUrl = "/stock/real-time-price/";
        private const string Fmg1MinQuoteUrl = "/historical-chart/1min/";
        private const string FmgInvestmentValuationRatiosUrl = "/financial-ratios/";
        private const string FmgStockListUrl = "/company/stock/list/";


        public static List<FmgMajorIndex> RetrieveFmgMajorIndexes()
        {
            string url = FmgBaseUrl + FmgMajorIndexesUrl;
            string response = RetrieveFromUrl(url).Result;
            if (response == "{ }" || string.IsNullOrEmpty(response))
            {
                throw new ArgumentException("FmgMajorIndexes null. " + url);
            }
            List<FmgMajorIndex> result = ParseStringToFmgMajorIndexList(response);
            return result;
        }

        public static async Task<FmgQuoteOnlyPrice> RetrieveFmgQuoteOnlyPrice(string symbol)
        {
            string url = FmgBaseUrl + FmgQuoteOnlyPriceUrl + symbol;
            var responseTask = RetrieveFromUrl(url);
            string response = await responseTask;
            if (response == "{ }" || string.IsNullOrEmpty(response))
            {
                throw new ArgumentException("FmgQuoteOnlyPrice null. " + symbol);
            }
            FmgQuoteOnlyPrice result = ParseStringToFmgQuoteOnlyPrice(response);
            return result;
        }

        private static FmgQuoteOnlyPrice ParseStringToFmgQuoteOnlyPrice(string response)
        {
            try
            {
                return JsonConvert.DeserializeObject<FmgQuoteOnlyPrice>(response);
            }
            catch (Newtonsoft.Json.JsonSerializationException ex)
            {
                throw new SystemException("Parse quote exception. " + ex.Message);
            }
        }

        public static List<FmgQuoteOnlyPrice> RetrieveAllFmgQuoteOnlyPrice()
        {
            string url = FmgBaseUrl + FmgQuoteOnlyPriceUrl;
            string response = RetrieveFromUrl(url).Result;
            if (response == "{ }" || string.IsNullOrEmpty(response))
            {
                throw new ArgumentException("FmgQuoteDailyPrice null. " + url);
            }
            List<FmgQuoteOnlyPrice> result = ParseStringToAllFmgQuoteOnlyPrice(response);
            return result;
        }

        private static List<FmgQuoteOnlyPrice> ParseStringToAllFmgQuoteOnlyPrice(string response)
        {
            try
            {
                List<FmgQuoteOnlyPrice> fmgQuoteOnlyPriceList = JsonConvert.DeserializeObject<JObject>(response)
                    .Value<JArray>("stockList").ToObject<List<FmgQuoteOnlyPrice>>();
                return fmgQuoteOnlyPriceList;
            }
            catch (Newtonsoft.Json.JsonSerializationException ex)
            {
                throw new SystemException("Parse quote LIST exception. " + ex.Message);
            }
        }

        private static List<FmgMajorIndex> ParseStringToFmgMajorIndexList(string response)
        {
            try
            {
                List<FmgMajorIndex> majorIndexList = JObject.Parse(response).GetValue("majorIndexesList")
                    .ToObject<List<FmgMajorIndex>>();

                return majorIndexList;
            }
            catch (Newtonsoft.Json.JsonSerializationException ex)
            {
                throw new SystemException("Parse Major index list exception. " + ex.Message);
            }
        }


        public static FmgCompanyProfile RetrieveFmgCompanyProfile(string symbol)
        {
            string url = FmgBaseUrl + FmgCompanyProfileUrl + symbol;
            string response = RetrieveFromUrl(url).Result;
            if (response == "{ }" || string.IsNullOrEmpty(response))
            {
                throw new ArgumentException("FmgCompanyProfile null. " + symbol);
            }
            FmgCompanyProfile result = ParseStringToFmgCompanyProfile(response);
            return result;
        }


        private static FmgCompanyProfile ParseStringToFmgCompanyProfile(string response)
        {
            try
            {
                var profile = JObject.Parse(response).GetValue("profile").ToObject<FmgCompanyProfile>();
                return profile;
            }
            catch (Newtonsoft.Json.JsonSerializationException ex)
            {
                throw new SystemException("Parse company profile exception. " + ex.Message);
            }
        }


        public static List<FmgCandleDaily> RetrieveFmgDataDaily(string companySymbol)
        {
            string url = FmgBaseUrl + FmgDataDailyUrl + companySymbol;
            List<FmgCandleDaily> resultTask = RequestFmgDataDaily(url).Result;

            return resultTask;
        }


        private static async Task<List<FmgCandleDaily>> RequestFmgDataDaily(string url)
        {
            Task<string> responseTask = RetrieveFromUrl(url);
            string response = await responseTask;
            if (response == "{ }" || string.IsNullOrEmpty(response))
            {
                throw new ArgumentException("FmgDataDaily null. " + url);
            }
            return ParseStringToFmgDataDaily(response);
        }

        private static async Task<string> RetrieveFromUrl(string url)
        {
            HttpClient client = new HttpClient();
            using (HttpResponseMessage response = await client.GetAsync(url))
            {
                response.EnsureSuccessStatusCode();
                string responseBody = await response.Content.ReadAsStringAsync();
                return responseBody;
            }
        }

        private static List<FmgCandleDaily> ParseStringToFmgDataDaily(string response)
        {
            try
            {
                var jsonSet = JsonConvert.DeserializeObject<JObject>(response);
                var dataDailyList = jsonSet.Value<JArray>("historical").ToObject<List<FmgCandleDaily>>();

                return dataDailyList;
            }
            catch (Newtonsoft.Json.JsonSerializationException ex)
            {
                throw new SystemException("Parse data daily exception. " + ex.Message);
            }
        }

        public static async Task<List<Fmg1MinQuote>> RetrieveAllFmg1MinQuote(string symbol)
        {
            string url = FmgBaseUrl + Fmg1MinQuoteUrl + symbol;
            string response = await RetrieveFromUrl(url);
            if (response == "{ }" || string.IsNullOrEmpty(response))
            {
                throw new ArgumentException("AllFmgMinQuote null. " + symbol);
            }
            List<Fmg1MinQuote> fmg1MinQuoteList = ParseStringToFmg1MinQuoteList(response);
            return fmg1MinQuoteList;
        }

        private static List<Fmg1MinQuote> ParseStringToFmg1MinQuoteList(string response)
        {
            try
            {
                return JsonConvert.DeserializeObject<List<Fmg1MinQuote>>(response);
            }
            catch (SystemException ex)
            {
                throw new SystemException("Parse 1 min quote list exception. " + ex.Message);
            }
        }

        public static FmgInvestmentValuationRatios RetrieveFmgInvestmentValuationRatios(string symbol)
        {
            string url = FmgBaseUrl + FmgInvestmentValuationRatiosUrl + symbol;
            string response = RetrieveFromUrl(url).Result;
            if (response == "{ }" || string.IsNullOrEmpty(response))
            {
                throw new SystemException("FmgInvestmentValuationRatios null. " + symbol);
            }
            FmgInvestmentValuationRatios fmgInvRatioList = ParseStringToFmgInvestmentValuationRatios(response);
            return fmgInvRatioList;
        }

        private static FmgInvestmentValuationRatios ParseStringToFmgInvestmentValuationRatios(string response)
        {
            try
            {
                var jsonSet = JsonConvert.DeserializeObject<JObject>(response).Value<JArray>("ratios");
                var tmp = jsonSet[0].Value<JObject>("investmentValuationRatios");
                var result = tmp.ToObject<FmgInvestmentValuationRatios>();
                return result;
            }
            catch (Newtonsoft.Json.JsonSerializationException ex)
            {
                throw new SystemException("Parse investment ratios exception. " + ex.Message);
            }
        }

        public static List<FmgStockListEntity> RetrieveStockList()
        {
            string url = FmgBaseUrl + FmgStockListUrl;
            string response = RetrieveFromUrl(url).Result;
            if (response == "{ }" || string.IsNullOrEmpty(response))
            {
                throw new ArgumentException("stockList null. " + url);
            }
            List<FmgStockListEntity> entityList =
                JObject.Parse(response).GetValue("symbolsList").ToObject<List<FmgStockListEntity>>();
            return entityList.Where(p => !string.IsNullOrEmpty(p.Name)).Select(p => p).ToList();
        }
    }
}
