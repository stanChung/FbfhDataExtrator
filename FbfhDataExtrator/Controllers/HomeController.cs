using HtmlAgilityPack;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Net;
using System.Text;
using System.Transactions;
using System.Web;
using System.Web.Configuration;
using System.Web.Mvc;

namespace FbfhDataExtrator.Controllers
{
    public class HomeController : Controller
    {
        string urlRoot = @"fbfh.trade.gov.tw";


        public ActionResult Index()
        {
            //parse();
            return View();
        }



        public ActionResult QueryForward()
        {
            var total = 0;
            var lstObj = parse(Request.Form, out total);
            var mode = new { total = total, rows = lstObj };

            return View(mode);
        }

        public ActionResult PagingQuery(int page, int rows)
        {
            var urlQuery = $@"https://{ urlRoot}/rich/text/fbj/asp/fbje140L.asp?ScrollAction=Page 1&ban_no=&txtCheckCode=&isCA=";
            var cc = new CookieContainer();
            var resultHtml = string.Empty;
            var lst = new List<Models.CustomerData>();

            return Json(new { total = "", rows = "" });
        }

        public string GetCustDetailHtml(Models.CustomerData q)
        {
            var result = getCustDetail(q);
            return result;
        }

        /// <summary>
        /// 儲存資料
        /// </summary>
        /// <param name="lst"></param>
        /// <returns></returns>
        public ActionResult SaveData(List<Models.CustomerData> lst)
        {
            var resutl = new { isSuccess = true, msg = "資料轉換成功" };
            try
            {
                importDetail(lst);
            }
            catch (Exception ex)
            {
                resutl = new { isSuccess = false, msg = ex.Message.Replace("\r\n", "<br/>") };
            }

            return Json(resutl, JsonRequestBehavior.AllowGet);

        }


        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }



        private IEnumerable<object> parse(NameValueCollection nc, out int total)
        {
            total = 0;
            var urlQuery = $@"https://{ urlRoot}/rich/text/fbj/asp/fbje140L.asp";

            var cc = new CookieContainer();
            var resultHtml = string.Empty;
            var lst = new List<Models.CustomerData>();


            using (var hwc = new HttpWebClient(cc) { Encoding = System.Text.Encoding.UTF8 })
            {

                resultHtml = Encoding.UTF8.GetString(hwc.UploadValues(urlQuery, nc));
                Session["ch"] = cc.GetCookieHeader(new Uri($"https://{urlRoot}"));

                HtmlDocument doc = new HtmlDocument();
                doc.LoadHtml(resultHtml);
                var nodeTr = doc.DocumentNode.SelectSingleNode("//table").SelectNodes("//tr");

                if (nodeTr.Count > 2)
                {
                    #region 取總筆數
                    total = int.Parse(doc.DocumentNode.SelectSingleNode("//big").InnerHtml.Substring(
                        doc.DocumentNode.SelectSingleNode("//big").InnerHtml.LastIndexOf("共") + 1).Split(',')[0].Replace("筆記錄 ", ""));
                    #endregion

                    for (int i = 2; i < nodeTr.Count; i++)
                    {
                        //查詢參數(統編,PCY)
                        var parms = nodeTr[i].ChildNodes[3].ChildNodes[1].ChildNodes[1].GetAttributeValue("href", "").Substring(21).Replace("'", "").Replace("(", "").Replace(")", "").Split(new char[] { ',' });

                        lst.Add(new Models.CustomerData()
                        {
                            ComopanyCName = nodeTr[i].ChildNodes[1].InnerText.Trim(),//廠商公司中文名稱
                            CompanyEName = nodeTr[i].ChildNodes[3].InnerText.Trim(),//廠商公司英文名稱
                            COMPANY_OWNER = nodeTr[i].ChildNodes[5].InnerText.Trim(),//負責人姓名
                            CompanyUni = parms[0].Trim(),//統編
                            PCY = parms[1].Trim()//查詢用PCY


                        });
                    }


                }

            }


            return lst;
        }

        private string getCustDetail(Models.CustomerData q)
        {

            var urlQuery = $@"https://{urlRoot}/rich/text/fbj/asp/fbje140Q2.asp?uni_no={q.CompanyUni}&pcy={q.PCY}";
            var result = string.Empty;

            var cc = new CookieContainer();
            cc.SetCookies(new Uri($"https://{urlRoot}"), Session["ch"].ToString());
            using (var hwc = new HttpWebClient(cc) { Encoding = Encoding.UTF8 })
            {
                //hwc.DownloadString(urlRoot);
                //cc.Add(new Cookie("CheckCode", "", "/", "fbfh.trade.gov.tw"));

                result = hwc.DownloadString(urlQuery);
            }
            var doc = new HtmlDocument();
            doc.LoadHtml(result);
            doc.DocumentNode.SelectSingleNode("//body").ChildNodes[9].Remove();

            return doc.DocumentNode.SelectSingleNode("//body").InnerHtml;
        }

        /// <summary>
        /// 處理廠商資料的轉換與儲存
        /// </summary>
        /// <param name="lst"></param>
        /// <returns></returns>
        private IEnumerable<object> importDetail(List<Models.CustomerData> lst)
        {


            var strSql = @"
                    INSERT INTO FBFH_VENDER VALUES(    
                                :COMPANY_UNI,
                                :ORIGINAL_REGISTERED_DATE,
                                :APPROVAL_DATE,
                                :COMPANY_CNAME,
                                :COMPANY_ENAME,
                                :COMPANY_CADDRESS,
                                :COMPANY_EADDRESS,
                                :COMPANY_OWNER,
                                :COMPANY_TEL_1,
                                :COMPANY_TEL_2,
                                :COMPANY_FAX,
                                :COMPANY_ORIGINAL_CNAME,
                                :COMPANY_ORIGNAL_ENAME,
                                :IMPORT_QUALIFICATION,
                                :EXPORT_QUALIFICATION)
                ";


            using (var ts = new TransactionScope())
            using (var con = new OracleConnection(WebConfigurationManager.ConnectionStrings["FBFH"].ConnectionString))
            using (var cmd = new OracleCommand(strSql, con))
            {
                cmd.BindByName = true;
                cmd.Parameters.Add(":COMPANY_UNI", OracleDbType.NVarchar2);//0 公司統編
                cmd.Parameters.Add(":ORIGINAL_REGISTERED_DATE", OracleDbType.Date);//1 原登記日期
                cmd.Parameters.Add(":APPROVAL_DATE", OracleDbType.Date);//2 核發日期
                cmd.Parameters.Add(":COMPANY_CNAME", OracleDbType.NVarchar2);//3 公司中文名稱
                cmd.Parameters.Add(":COMPANY_ENAME", OracleDbType.NVarchar2);//4 公司英文名稱
                cmd.Parameters.Add(":COMPANY_CADDRESS", OracleDbType.NVarchar2);//5 中文營業地址
                cmd.Parameters.Add(":COMPANY_EADDRESS", OracleDbType.NVarchar2);//6 英文營業地址
                cmd.Parameters.Add(":COMPANY_OWNER", OracleDbType.NVarchar2);//7 代表人
                cmd.Parameters.Add(":COMPANY_TEL_1", OracleDbType.NVarchar2);//8 公司電話1
                cmd.Parameters.Add(":COMPANY_TEL_2", OracleDbType.NVarchar2);//9 公司電話2
                cmd.Parameters.Add(":COMPANY_FAX", OracleDbType.NVarchar2);//10 公司傳真
                cmd.Parameters.Add(":COMPANY_ORIGINAL_CNAME", OracleDbType.NVarchar2);//11 原公司中文名稱
                cmd.Parameters.Add(":COMPANY_ORIGNAL_ENAME", OracleDbType.NVarchar2);//12 原公司英文名稱
                cmd.Parameters.Add(":IMPORT_QUALIFICATION", OracleDbType.Char);//13 進口資格
                cmd.Parameters.Add(":EXPORT_QUALIFICATION", OracleDbType.Char);//14 出口資格

                con.Open();

                lst.ForEach(x =>
                {
                    var result = getCustDetail(x);
                    var doc = new HtmlDocument();
                    doc.LoadHtml(result);
                    try
                    {
                            #region 載入資料
                            var dataNodes = doc.DocumentNode.SelectNodes("//td");
                        cmd.Parameters[0].Value = x.CompanyUni;//公司統編
                            cmd.Parameters[1].Value = DateTime.Parse(dataNodes[4].InnerHtml);//原登記日期
                            cmd.Parameters[2].Value = DateTime.Parse(dataNodes[6].InnerHtml);//核發日期
                            cmd.Parameters[3].Value = x.ComopanyCName.Replace("\r\n", "").Replace("\t", "");//公司中文名稱
                            cmd.Parameters[4].Value = x.CompanyEName.Replace("\r\n", "").Replace("\t", "");//公司英文名稱
                            cmd.Parameters[5].Value = dataNodes[12].InnerHtml.Replace("\r\n", "").Replace("\t", "");//中文營業地址
                            cmd.Parameters[6].Value = dataNodes[14].InnerHtml.Replace("\r\n", "").Replace("\t", "");//英文營業地址
                            cmd.Parameters[7].Value = x.COMPANY_OWNER;//代表人
                            cmd.Parameters[8].Value = dataNodes[18].InnerHtml.Replace("\r\n", "").Replace("\t", "");//公司電話1
                            cmd.Parameters[9].Value = dataNodes[20].InnerHtml.Replace("\r\n", "").Replace("\t", "");//公司電話2
                            cmd.Parameters[10].Value = dataNodes[22].InnerHtml.Replace("\r\n", "").Replace("\t", "");//公司傳真
                            cmd.Parameters[11].Value = dataNodes[24].InnerHtml.Replace("\r\n", "").Replace("\t", "");//原中文公司名稱
                            cmd.Parameters[12].Value = dataNodes[26].InnerHtml.Replace("\r\n", "").Replace("\t", "");//原英文公司名稱
                            cmd.Parameters[13].Value = dataNodes[38].InnerHtml.Equals("有") ? "1" : "0";//進口資格
                            cmd.Parameters[14].Value = dataNodes[39].InnerHtml.Equals("有") ? "1" : "0";//出口資格

                            #endregion

                            cmd.ExecuteNonQuery();
                    }
                    catch (Exception ex)
                    {
                        throw new Exception(
                            $"廠商{x.CompanyUni} {x.ComopanyCName}，資料無法轉換成功，請聯絡資訊人員處理。\r\n\r\n錯誤描述：{ex.Message}");
                    }

                });

                ts.Complete();


            }



            return lst;
        }
    }
}