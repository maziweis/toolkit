using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web.Script.Serialization;

namespace ResourceToolkit
{
    public static class DealData
    {

        /// <summary>
        /// 截取括号中的ID
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static int RetSubstringBracket(string str)
        {

            int id = 0;
            int count_id = 0;
            string pattern = @"\(.*?\)";//匹配模式
            Regex regex = new Regex(pattern, RegexOptions.Compiled);
            MatchCollection matchall = regex.Matches(str);
            foreach (Match match_one in matchall)
            {
                count_id++;
                if (count_id == matchall.Count)
                {
                    if (match_one.Success)
                    {
                        id = int.Parse(match_one.ToString().Replace("(", "").Replace(")", ""));
                    }
                }
            }

            return id;
        }
        /// <summary>
        /// 截取下划线后面的ID
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static int RetSubstringUnderline(string str)
        {
            int? id = 0;
            if (str != string.Empty)
            {
                id = int.Parse(str.Substring(str.LastIndexOf('_') + 1, str.Length - str.LastIndexOf('_') - 1));
            }
            return (int)id;
        }

        /// <summary>
        /// 截取下划线前面的文本
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string stringUnderline(string str)
        {
            string text = "";
            if (str != string.Empty)
            {
                text = str.Split('_')[0];
            }
            return (string)text;
        }
        /// <summary>
        /// 根据数字选择整个文本
        /// </summary>
        /// <param name="x"></param>
        /// <returns></returns>

        public static string SelectString(List<String> test, int x)
        {
            string str = "";
            List<String> L1 = new List<string>();
            L1 = test;
            foreach (var item in L1)
            {
                if (RetSubstringBracket(item) == x)
                {
                    str = item;
                }
            }
            return str;
        }
        /// <summary>
        /// 将json 反序列化为Dic
        /// </summary>
        /// <param name="jsonData"></param>
        /// <returns></returns>
        public static Dictionary<string, object> JsonToDic(string jsonData)
        {
            JavaScriptSerializer jss = new JavaScriptSerializer();
            try
            {
                return jss.Deserialize<Dictionary<string, object>>(jsonData);
            }
            catch (System.Exception ex)
            {
                throw new Exception(ex.Message);
            }

        }
        public static TempModel.Resource Get_Data_From_Server(TempModel.Resource MatchModel, string temp)
        {
            Dictionary<string, object> dic = JsonToDic(temp); // 将JSON 数据转成dic
            foreach (KeyValuePair<string, object> item in dic)
            {

                if (item.Key.ToString() == "Success")
                {
                    MatchModel.IsSuccess = Convert.ToBoolean(item.Value);
                }

            }
            if (MatchModel.IsSuccess)
            {
                foreach (KeyValuePair<string, object> item1 in dic)
                {

                    if (item1.Key.ToString() == "Data")
                    {
                        var subItem = (Dictionary<string, object>)item1.Value;
                        foreach (var str in subItem)
                        {
                            if (str.Key.ToString() == "ID")
                            {
                                MatchModel.FileID = new Guid(str.Value.ToString());
                            }
                            if (str.Key.ToString() == "FileExtension")
                            {
                                MatchModel.FileType = str.Value.ToString();
                            }
                            if (str.Key.ToString() == "FileSize")
                            {
                                MatchModel.ResourceSize = str.Value.ToString();
                            }
                            if (str.Key.ToString() == "FileName")
                            {
                                MatchModel.FileReturnName = str.Value.ToString();
                            }

                        }
                    }
                }
            }
            return MatchModel;
        }

        /// <summary>
        /// 知识点
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static List<int> RetKnowledge(string str)
        {
            List<int> lsKnowledge = new List<int>();
            if (str != string.Empty)
            {
                List<string> ls = str.Split(',').ToList();
                if (ls != null && ls.Count > 0)
                {
                    foreach (string s in ls)
                    {
                        int id = 0;
                        id = int.Parse(s.Substring(s.LastIndexOf('(') + 1, s.Length - s.LastIndexOf('(') - 2));
                        lsKnowledge.Add(id);
                    }
                }
            }
            else
            {
                lsKnowledge = null;
            }

            return lsKnowledge;
        }

        /// <summary>
        /// 适用对象
        /// </summary>
        /// <param name="?"></param>
        /// <returns></returns>
        public static List<int> RetApplicable(string str)
        {
            List<int> lsApplicable = new List<int>();
            if (str != string.Empty)
            {
                List<string> ls = str.Split(',').ToList();
                if (ls != null && ls.Count > 0)
                {
                    foreach (string s in ls)
                    {
                        int id = 0;
                        id = int.Parse(s.Substring(s.LastIndexOf('_') + 1, s.Length - s.LastIndexOf('_') - 1));
                        lsApplicable.Add(id);
                    }
                }
            }
            else
            {
                lsApplicable = null;
            }

            return lsApplicable;
        }


        /// <summary>
        /// 判断文件是否上传成功
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string IsFileUp(string str)
        {
            List<string> ls = str.Split(',').ToList();
            List<string> ls2 = ls[1].Split(':').ToList();
            string res = ls2[1].ToString();
            return res;
        }

        /// <summary>
        /// 返回FileID
        /// </summary>
        /// <returns></returns>
        public static string strID(string str)
        {
            List<string> ls = str.Split(',').ToList();
            List<string> ls2 = ls[2].Split(',').ToList();
            string res = ls2[0].Replace("\"Data\":{\"ID\":\"", "").Replace("\"", "");
            return res;
        }

        /// <summary>
        /// 返回文件后缀
        /// </summary>
        /// <returns></returns>
        public static string FileType(string str)
        {
            List<string> ls = str.Split(',').ToList();
            List<string> ls2 = ls[4].Split(':').ToList();
            string res = ls2[1].ToString().Replace("\"", "");
            return res;
        }

        /// <summary>
        /// 返回文件大小
        /// </summary>
        /// <returns></returns>
        public static string FileSize(string str)
        {
            List<string> ls = str.Split(',').ToList();
            List<string> ls2 = ls[5].Split(':').ToList();
            string res = ls2[1].Replace("}", "").Replace("\"", "");
            return res;
        }


    }
}
