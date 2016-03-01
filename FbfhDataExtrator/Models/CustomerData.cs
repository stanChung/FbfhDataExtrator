using System;

namespace FbfhDataExtrator.Models
{
    /// <summary>
    /// 廠商資料
    /// </summary>
    public class CustomerData
    {
        /// <summary>
        /// 統編
        /// </summary>
        public string CompanyUni { get; set; }

        public string PCY { get; set; }
        /// <summary>
        /// 公司中文名稱
        /// </summary>
        public string ComopanyCName { get; set; }
        /// <summary>
        /// 公司英文名稱
        /// </summary>
        public string CompanyEName { get; set; }
        /// <summary>
        /// 代表人姓名
        /// </summary>
        public string COMPANY_OWNER { get; set; }

        ///// <summary>
        ///// 原始登記日期
        ///// </summary>
        //public DateTime ORIGINAL_REGISTERED_DATE { get; set; }
        ///// <summary>
        ///// 核發日期
        ///// </summary>
        //public DateTime APPROVAL_DATE { get; set; }
        ///// <summary>
        ///// 中文營業地址
        ///// </summary>
        //public string COMPANY_CADDRESS { get; set; }
        ///// <summary>
        ///// 英文營業地址
        ///// </summary>
        //public string COMPANY_EADDRESS { get; set; }
        ///// <summary>
        ///// 公司電話1
        ///// </summary>
        //public string COMPANY_TEL_1 { get; set; }
        ///// <summary>
        ///// 公司電話2
        ///// </summary>
        //public string COMPANY_TEL_2 { get; set; }
        ///// <summary>
        ///// 公司傳真
        ///// </summary>
        //public string COMPANY_FAX { get; set; }
        ///// <summary>
        ///// 廠商原中文名稱
        ///// </summary>
        //public string COMPANY_ORIGINAL_CNAME { get; set; }
        ///// <summary>
        ///// 廠商原英文名稱
        ///// </summary>
        //public string COMPANY_ORIGINAL_ENAME { get; set; }
        ///// <summary>
        ///// 進口資格
        ///// </summary>
        //public string IMPORT_QUALIFICATION { get; set; }
        ///// <summary>
        ///// 出口資格
        ///// </summary>
        //public string EXPORT_QUALIFICATION { get; set; }

    }
}