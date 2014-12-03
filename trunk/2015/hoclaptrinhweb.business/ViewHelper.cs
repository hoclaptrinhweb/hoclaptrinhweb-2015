using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace hoclaptrinhweb.business
{
    class ViewHelper
    {
        public static string RemoveSpecialCharacters(string str)
        {
            return str.Replace(",", "").Replace("_", "").Replace("+", "").Replace("&", "").Replace("=", "").Replace("?", "").Replace("@", "").Replace("!", " ").Replace("~", "").Replace("#", "").Replace("*", "").Replace("<", "").Replace(">", "").Replace("(", "").Replace(")", "").Replace("%", "").Replace("`", "").Replace("~", "");
        }
        public bool IsValidMail(string email)
        {
            if (string.IsNullOrEmpty(email))
                return false;
            string sMailPattern = @"\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*";
            return Regex.IsMatch(email.Trim(), sMailPattern);
        }
        public bool IsValidVietNamPhoneNumber(string phoneNum)
        {
            if (string.IsNullOrEmpty(phoneNum))
                return false;
            string sMailPattern = @"^((09(\d){8})|(01(\d){9}))$";
            return Regex.IsMatch(phoneNum.Trim(), sMailPattern);
        }

        public string SimplifySearchKeyword(string keyword)
        {
            if (string.IsNullOrEmpty(keyword))
                return null;
            keyword = Regex.Replace(keyword, @"[^a-z0-9A-ZÀÁÂÃÈÉÊÌÍÒÓÔÕÙÚĂĐĨŨƠàáâãèéêìíòóôõùúăđĩũơƯĂẠẢẤẦẨẪẬẮẰẲẴẶẸẺẼỀẾỂưăạảấầẩẫậắằẳẵặẹẻẽềếểỄỆỈỊỌỎỐỒỔỖỘỚỜỞỠỢỤỦỨỪễệỉịọỏốồổỗộớờởỡợụủứừỬỮỰỲỴÝỶỸửữựỳỵỷỹ \.\-_]", "").Replace("  ", " ").Replace("  ", " ").Trim();
            if (keyword.Length > 50)
                keyword = keyword.Substring(0, 50);
            return keyword;
        }

        public static string ConvertToUnicode(string utf8_decstr)
        {
            if (string.IsNullOrEmpty(utf8_decstr)) return null;
            utf8_decstr = FromUTF8(utf8_decstr);
            return DecimalToWideString(utf8_decstr);
        }
        public static string DecimalToWideString(string DecStr)
        {
            StringBuilder J = new StringBuilder(DecStr);
            MatchCollection MC = Regex.Matches(DecStr, @"&#(\d+);");
            if (MC.Count == 0)
            {
                return DecStr;
            }
            int Last = 0;
            foreach (Match M in MC)
            {
                ushort CharCode;
                try
                {
                    CharCode = Convert.ToUInt16(M.Groups[1].Value);
                }
                catch
                {
                    continue;
                }
                char Ch = Convert.ToChar(CharCode);
                J.Remove(M.Index - Last, M.Length);
                J.Insert(M.Index - Last, Ch);
                Last += M.Groups[0].Value.Length - 1;
            }
            return J.ToString();
        }
        public static string ShowPhoneNumber(string phoneNumber)
        {
            return string.IsNullOrEmpty(phoneNumber) ? phoneNumber : phoneNumber.Replace(phoneNumber.Substring(0, phoneNumber.Length - 6), "xxxx");
        }
        public static string FromUTF8(string utf8)
        {
            byte[] bs = Encoding.Default.GetBytes(utf8);
            return Encoding.UTF8.GetString(bs);
        }
        public static string ConvertToUnSign(string s)
        {
            Regex regex = new Regex("\\p{IsCombiningDiacriticalMarks}+");
            string temp = s.Normalize(NormalizationForm.FormD);
            return regex.Replace(temp, String.Empty).Replace('\u0111', 'd').Replace('\u0110', 'D');
        }
        public static string ShowTime(string datetime)
        {
            DateTime tmp = Convert.ToDateTime(datetime);
            string time = string.Empty;

            if ((DateTime.Now - tmp).Days > 0)
            {
                time = string.Format("tham gia {0} ngày trước", (DateTime.Now - tmp).Days);
            }
            else if ((DateTime.Now - tmp).Hours > 0)
            {
                time = string.Format("tham gia {0} giờ trước", (DateTime.Now - tmp).Hours);
            }
            else if ((DateTime.Now - tmp).Minutes > 0)
            {
                time = string.Format("tham gia {0} phút trước", (DateTime.Now - tmp).Minutes);
            }
            else
            {
                time = "vừa tham gia";
            }
            return time;
        }
    }
}
