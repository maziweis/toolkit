using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;

namespace ResourceToolkit
{
    /// <summary>
    /// 公共类
    /// </summary>
    class Common
    {
        /// <summary>
        ///返回请求数据
        /// </summary>
        /// <param name="Url"></param>
        /// <param name="postDataStr"></param>
        /// <returns></returns>
        public static string HttpGet(string Url)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(Url);
            request.Method = "GET";
            request.ContentType = "text/html;charset=UTF-8";

            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            Stream myResponseStream = response.GetResponseStream();
            StreamReader myStreamReader = new StreamReader(myResponseStream, Encoding.GetEncoding("utf-8"));
            string jsonStr = myStreamReader.ReadToEnd();
            myStreamReader.Close();
            myResponseStream.Close();
            return jsonStr;
        }

        /// <summary>
        /// 将json数据反序列化为Dictionary
        /// </summary>
        /// <param name="jsonData">json数据</param>
        /// <returns></returns>
        public static Dictionary<string, object> JsonToDictionary(string jsonStr)
        {
            //实例化JavaScriptSerializer类的新实例
            JavaScriptSerializer jss = new JavaScriptSerializer();
            try
            {
                //将指定的 JSON 字符串转换为 Dictionary<string, object> 类型的对象
                return jss.Deserialize<Dictionary<string, object>>(jsonStr);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public static ArrayList JsonToList(string jsonStr)
        {
            //实例化JavaScriptSerializer类的新实例
            JavaScriptSerializer jss = new JavaScriptSerializer();
            try
            {
                //将指定的 JSON 字符串转换为 ArrayList类型的对象
                return jss.Deserialize<ArrayList>(jsonStr);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// 处理json格式数据，返回字符串列表
        /// </summary>
        /// <param name="jsonStr"></param>
        /// <param name="keyword"></param>
        /// <returns></returns>
        public static List<String> DealJson(String jsonStr, String keyword)
        {
            Dictionary<string, object> dic = JsonToDictionary(jsonStr); // 将JSON 数据转成dic
            List<String> resultList = new List<string>();

            if (dic.Count == 1)
            {
                if (keyword == "NEWJXMK")
                {
                    if (dic[keyword].GetType() == typeof(ArrayList))
                    {
                        ArrayList subDicts = (ArrayList)dic[keyword];
                        for (int i = 0; i < subDicts.Count; i++)
                        {
                            if (subDicts[i].GetType() == typeof(Dictionary<string, object>))
                            {
                                Dictionary<string, object> subDict = (Dictionary<string, object>)subDicts[i];
                                String id = "";
                                String title = "";
                                ArrayList children = new ArrayList();
                                String isFolder = "";
                                String comStr;
                                foreach (KeyValuePair<string, object> item in subDict)
                                {
                                    if (item.Key.ToString() == "id")
                                        id = item.Value.ToString();
                                    if (item.Key.ToString() == "title")
                                        title = item.Value.ToString();
                                    if (item.Key.ToString() == "children")
                                        children = (ArrayList)item.Value;
                                    if (item.Key.ToString() == "isFolder")
                                        isFolder = item.Value.ToString();
                                }
                                comStr = title + "_" + id.PadLeft(3, '0');
                                resultList.Add(comStr);

                                if (isFolder == "True")
                                {
                                    for (int j = 0; j < children.Count; j++)
                                    {
                                        string id2 = "";
                                        string title2 = "";
                                        string isFolder2 = "";
                                        Dictionary<string, object> subChild = (Dictionary<string, object>)children[j];
                                        foreach (KeyValuePair<string, object> item2 in subChild)
                                        {
                                            if (item2.Key.ToString() == "id")
                                                id2 = item2.Value.ToString();
                                            if (item2.Key.ToString() == "title")
                                                title2 = item2.Value.ToString();
                                            if (item2.Key.ToString() == "isFolder")
                                                isFolder2 = item2.Value.ToString();
                                        }
                                        if (isFolder2 == "False")
                                        {
                                            comStr = "    " + title2 + "_" + id2.PadLeft(3, '0');
                                            resultList.Add(comStr);
                                        }
                                        else
                                            resultList.Add("出错啦");
                                    }
                                }
                            }
                        }
                    }
                }
                else if (keyword == "RSORT")
                {
                    if (dic[keyword].GetType() == typeof(ArrayList))
                    {
                        ArrayList subDicts = (ArrayList)dic[keyword];
                        for (int i = 0; i < subDicts.Count; i++)
                        {
                            if (subDicts[i].GetType() == typeof(Dictionary<string, object>))
                            {
                                Dictionary<string, object> subDict = (Dictionary<string, object>)subDicts[i];
                                String id = "";
                                String title = "";
                                ArrayList children = new ArrayList();
                                ArrayList children2 = new ArrayList();
                                String isFolder = "";
                                String comStr;
                                foreach (KeyValuePair<string, object> item in subDict)
                                {
                                    if (item.Key.ToString() == "id")
                                        id = item.Value.ToString();
                                    if (item.Key.ToString() == "title")
                                        title = item.Value.ToString();
                                    if (item.Key.ToString() == "children")
                                        children = (ArrayList)item.Value;
                                    if (item.Key.ToString() == "isFolder")
                                        isFolder = item.Value.ToString();
                                }
                                comStr = "(" + id.PadLeft(3, '0') + ")" + title;
                                resultList.Add(comStr);

                                if (isFolder == "True")
                                {
                                    for (int j = 0; j < children.Count; j++)
                                    {
                                        string id2 = "";
                                        string title2 = "";
                                        string isFolder2 = "";

                                        Dictionary<string, object> subChild = (Dictionary<string, object>)children[j];
                                        foreach (KeyValuePair<string, object> item2 in subChild)
                                        {

                                            if (item2.Key.ToString() == "id")
                                                id2 = item2.Value.ToString();
                                            if (item2.Key.ToString() == "title")
                                                title2 = item2.Value.ToString();
                                            if (item2.Key.ToString() == "children")
                                                children2 = (ArrayList)item2.Value;
                                            if (item2.Key.ToString() == "isFolder")
                                                isFolder2 = item2.Value.ToString();
                                        }
                                        if (isFolder2 == "True")
                                        {
                                            comStr = "    (" + id2.PadLeft(3, '0') + ")" + title2;
                                            resultList.Add(comStr);
                                            for (int k = 0; k < subChild.Count; k++)
                                            {
                                                string id3 = "";
                                                string title3 = "";
                                                string isFolder3 = "";
                                                Dictionary<string, object> subChildchild = (Dictionary<string, object>)children2[k];
                                                foreach (KeyValuePair<string, object> item3 in subChildchild)
                                                {

                                                    if (item3.Key.ToString() == "id")
                                                        id3 = item3.Value.ToString();
                                                    if (item3.Key.ToString() == "title")
                                                        title3 = item3.Value.ToString();
                                                    if (item3.Key.ToString() == "isFolder")
                                                        isFolder3 = item3.Value.ToString();
                                                }
                                                comStr = "        (" + id3.PadLeft(3, '0') + ")" + title3;
                                                resultList.Add(comStr);
                                            }
                                        }

                                        if (isFolder2 == "False")
                                        {
                                            comStr = "    (" + id2.PadLeft(3, '0') + ")" + title2;
                                            resultList.Add(comStr);
                                        }
                                        //else
                                        //    resultList.Add("出错啦");
                                    }
                                }
                            }
                        }
                    }
                }
                else
                {
                    if (dic[keyword].GetType() == typeof(ArrayList))
                    {
                        ArrayList subDicts = (ArrayList)dic[keyword];

                        for (int i = 0; i < subDicts.Count; i++)
                        {
                            if (subDicts[i].GetType() == typeof(Dictionary<string, object>))
                            {
                                Dictionary<string, object> subDict = (Dictionary<string, object>)subDicts[i];
                                String ID = "";
                                String CodeName = "";
                                String comStr;
                                foreach (KeyValuePair<string, object> item in subDict)
                                {
                                    if (item.Key.ToString() == "ID")
                                        ID = item.Value.ToString();
                                    if (item.Key.ToString() == "CodeName")
                                        CodeName = item.Value.ToString();
                                }
                                comStr = CodeName + "_" + ID.PadLeft(3, '0');
                                resultList.Add(comStr);
                            }
                        }

                    }
                }

            }
            return resultList;
        }

        /// <summary>
        /// 处理KNOWLEDGE知识点json格式数据，返回字符串组列表
        /// </summary>
        /// <param name="jsonStr"></param>
        /// <param name="keyword"></param>
        /// <returns></returns>
        public static List<List<String>> DealJsonOfKNOWLEDGE(String jsonStr, String keyword)
        {
            Dictionary<string, object> dic = JsonToDictionary(jsonStr); // 将JSON 数据转成dic
            List<List<string>> resultList = new List<List<string>>();
            List<String> resultList1 = new List<string>();//语文知识点
            List<String> resultList2 = new List<string>();//数学知识点
            List<String> resultList3 = new List<string>();//英语知识点

            if (dic.Count == 1)
            {
                if (keyword == "KNOWLEDGE")
                {
                    if (dic[keyword].GetType() == typeof(ArrayList))
                    {
                        ArrayList subDicts = (ArrayList)dic[keyword];
                        for (int i = 0; i < subDicts.Count; i++)
                        {
                            if (subDicts[i].GetType() == typeof(Dictionary<string, object>))
                            {
                                Dictionary<string, object> subDict = (Dictionary<string, object>)subDicts[i];
                                String id = "";
                                String title = "";
                                ArrayList children = new ArrayList();
                                String isFolder = "";
                                String subID = "";
                                String StageID = "";
                                String comStr;
                                foreach (KeyValuePair<string, object> item in subDict)
                                {
                                    if (item.Key.ToString() == "id")
                                        id = item.Value.ToString();
                                    if (item.Key.ToString() == "title")
                                        title = item.Value.ToString();
                                    if (item.Key.ToString() == "children")
                                        children = (ArrayList)item.Value;
                                    if (item.Key.ToString() == "isFolder")
                                        isFolder = item.Value.ToString();
                                    if (item.Key.ToString() == "subID")
                                        subID = item.Value.ToString();
                                    if (item.Key.ToString() == "StageID")
                                        StageID = item.Value.ToString();
                                }
                                comStr = "(" + subID.PadLeft(4, '0') + ")" + title + "(" + id.PadLeft(4, '0') + ")";
                                if (subID == "1") resultList1.Add(comStr);
                                else if (subID == "2") resultList2.Add(comStr);
                                else resultList3.Add(comStr);

                                if (isFolder == "True")
                                {
                                    for (int j = 0; j < children.Count; j++)
                                    {
                                        string id2 = "";
                                        string title2 = "";
                                        ArrayList children2 = new ArrayList();
                                        string isFolder2 = "";
                                        string subID2 = "";
                                        string StageID2 = "";
                                        Dictionary<string, object> subChild = (Dictionary<string, object>)children[j];
                                        foreach (KeyValuePair<string, object> item2 in subChild)
                                        {
                                            if (item2.Key.ToString() == "id")
                                                id2 = item2.Value.ToString();
                                            if (item2.Key.ToString() == "title")
                                                title2 = item2.Value.ToString();
                                            if (item2.Key.ToString() == "children")
                                                children2 = (ArrayList)item2.Value;
                                            if (item2.Key.ToString() == "isFolder")
                                                isFolder2 = item2.Value.ToString();
                                            if (item2.Key.ToString() == "subID")
                                                subID2 = item2.Value.ToString();
                                            if (item2.Key.ToString() == "StageID")
                                                StageID2 = item2.Value.ToString();
                                        }
                                        comStr = "    (" + subID2.PadLeft(4, '0') + ")" + title2 + "(" + id2.PadLeft(4, '0') + ")";
                                        if (subID2 == "1") resultList1.Add(comStr);
                                        else if (subID2 == "2") resultList2.Add(comStr);
                                        else resultList3.Add(comStr);

                                        if (isFolder2 == "True")
                                        {
                                            for (int k = 0; k < children2.Count; k++)
                                            {
                                                string id3 = "";
                                                string title3 = "";
                                                ArrayList children3 = new ArrayList();
                                                string isFolder3 = "";
                                                string subID3 = "";
                                                string StageID3 = "";
                                                Dictionary<string, object> subChild2 = (Dictionary<string, object>)children2[k];
                                                foreach (KeyValuePair<string, object> item3 in subChild2)
                                                {
                                                    if (item3.Key.ToString() == "id")
                                                        id3 = item3.Value.ToString();
                                                    if (item3.Key.ToString() == "title")
                                                        title3 = item3.Value.ToString();
                                                    if (item3.Key.ToString() == "children")
                                                        children3 = (ArrayList)item3.Value;
                                                    if (item3.Key.ToString() == "isFolder")
                                                        isFolder3 = item3.Value.ToString();
                                                    if (item3.Key.ToString() == "subID")
                                                        subID3 = item3.Value.ToString();
                                                    if (item3.Key.ToString() == "StageID")
                                                        StageID3 = item3.Value.ToString();
                                                }
                                                comStr = "        (" + subID3.PadLeft(4, '0') + ")" + title3 + "(" + id3.PadLeft(4, '0') + ")";
                                                if (subID3 == "1") resultList1.Add(comStr);
                                                else if (subID3 == "2") resultList2.Add(comStr);
                                                else resultList3.Add(comStr);
                                                if (isFolder3 == "True")
                                                {
                                                    for (int l = 0; l < children3.Count; l++)
                                                    {
                                                        string id4 = "";
                                                        string title4 = "";
                                                        ArrayList children4 = new ArrayList();
                                                        string isFolder4 = "";
                                                        string subID4 = "";
                                                        string StageID4 = "";
                                                        Dictionary<string, object> subChild3 = (Dictionary<string, object>)children3[l];
                                                        foreach (KeyValuePair<string, object> item4 in subChild3)
                                                        {
                                                            if (item4.Key.ToString() == "id")
                                                                id4 = item4.Value.ToString();
                                                            if (item4.Key.ToString() == "title")
                                                                title4 = item4.Value.ToString();
                                                            if (item4.Key.ToString() == "children")
                                                                children4 = (ArrayList)item4.Value;
                                                            if (item4.Key.ToString() == "isFolder")
                                                                isFolder4 = item4.Value.ToString();
                                                            if (item4.Key.ToString() == "subID")
                                                                subID4 = item4.Value.ToString();
                                                            if (item4.Key.ToString() == "StageID")
                                                                StageID4 = item4.Value.ToString();
                                                        }
                                                        comStr = "            (" + subID4.PadLeft(4, '0') + ")" + title4 + "(" + id4.PadLeft(4, '0') + ")";
                                                        if (subID4 == "1") resultList1.Add(comStr);
                                                        else if (subID4 == "2") resultList2.Add(comStr);
                                                        else resultList3.Add(comStr);

                                                        if (isFolder4 == "True")
                                                        {
                                                            if (subID4 == "1") resultList1.Add("出错啦");
                                                            else if (subID4 == "2") resultList2.Add("出错啦");
                                                            else resultList3.Add("出错啦");
                                                        }


                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            resultList.Add(resultList1);
            resultList.Add(resultList2);
            resultList.Add(resultList3);
            return resultList;
        }

        /// <summary>
        /// 根据具体版本生成册别
        /// </summary>
        /// <param name="url"></param>
        /// <param name="editonNum"></param>
        /// <param name="BooksList"></param>
        /// <returns></returns>
        public static List<String> GetBooksOfSpecificEdition(String url, int editonNum, ArrayList BooksList)
        {
            List<String> specificBooksList = new List<string>();
            if (BooksList.Count > 0)
            {
                for (int i = 0; i < BooksList.Count; i++)
                {
                    Dictionary<String, object> bookDic = (Dictionary<String, object>)BooksList[i];
                    String comboStr = "";
                    String BooKName = "", ID = "", Edition = "", Stage = "", Subject = "", Grade = "", Booklet = "";
                    foreach (KeyValuePair<string, object> item in bookDic)
                    {
                        if (item.Key.ToString() == "BooKName")
                            BooKName = item.Value.ToString();
                        if (item.Key.ToString() == "ID")
                            ID = item.Value.ToString();
                        if (item.Key.ToString() == "Edition")
                            Edition = item.Value.ToString();
                        if (item.Key.ToString() == "Stage")
                            Stage = item.Value.ToString();
                        if (item.Key.ToString() == "Subject")
                            Subject = item.Value.ToString();
                        if (item.Key.ToString() == "Grade")
                            Grade = item.Value.ToString();
                        if (item.Key.ToString() == "Booklet")
                            Booklet = item.Value.ToString();
                    }
                    if (editonNum.ToString() == Edition)
                    {
                        comboStr = BooKName + "_" + ID + "_" + Stage + "-" + Subject + "-" + Grade + "-" + Booklet + "-" + Edition;
                        specificBooksList.Add(comboStr);
                    }
                }
            }
            return specificBooksList;
        }

        /// <summary>
        /// 根据具体册别生成目录
        /// </summary>
        /// <param name="jsonStr"></param>
        /// <returns></returns>
        public static List<String> GetContentsOfSpecificBook(String jsonStr)
        {
            List<String> contentsList = new List<string>();
            ArrayList dictList = (ArrayList)JsonToList(jsonStr);
            contentsList = RecursiveContents(dictList, "", contentsList);

            return contentsList;
        }

        // public static List<String> contentsList = new List<string>();
        public static List<String> RecursiveContents(ArrayList dictList, String indent, List<String> contentsList)
        {
            String indent2 = "";
            for (int i = 0; i < dictList.Count; i++)
            {
                Dictionary<String, object> dict = (Dictionary<String, object>)dictList[i];
                String id = "", title = "", isFolder = "", sord = "";
                ArrayList children = new ArrayList();
                foreach (KeyValuePair<string, object> item in dict)
                {
                    if (item.Key.ToString() == "id")
                        id = item.Value.ToString();
                    if (item.Key.ToString() == "title")
                        title = item.Value.ToString();
                    if (item.Key.ToString() == "children")
                        children = (ArrayList)item.Value;
                    if (item.Key.ToString() == "isFolder")
                        isFolder = item.Value.ToString();
                    if (item.Key.ToString() == "sord")
                        sord = item.Value.ToString();
                }
                contentsList.Add(indent + title + "(" + id + ")");
                if (isFolder == "True")
                {
                    indent2 = indent + "    ";
                    RecursiveContents(children, indent2, contentsList);
                }
            }
            return contentsList;
        }
    }
}
