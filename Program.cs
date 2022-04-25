using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//需导入命名空间 （方式二）
using System.Xml;


//需导入命名空间 （方式三）
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace ReadXML
{
    internal class Program
    {
        static void Main(string[] args)
        {
            //方式一：通过DataSet 读取XML文件
            //First();
            //方式二：通过XmlDocument，读取XML文件，根据节点名字来获取节点的内容
            //Second();
            //方式三：使用JSON.NET 读取XML文件
            Third();

        }
        private static void First()
        {
            //1.先拿到需要读取的  xml文件路径
            //先获取Program.cs 的 路径
            string root = Path.GetFullPath(Path.Combine(
                Path.GetDirectoryName(typeof(Program).Assembly.Location)));
            //拼接 XML目录
            string xmlPath = Path.GetFullPath(Path.Combine(root, "XML\\不带属性的文件.xml"));
            string xmlPaths = Path.GetFullPath(Path.Combine(root, "XML\\带属性文件.xml"));


            //dataset读取简单xml文件
            DataSet ds = new DataSet();
            ds.ReadXml(xmlPath);
            //读取第一条数据的name节点
            string name = ds.Tables[0].Rows[0]["name"].ToString();
            //输出：张三
            Console.WriteLine(name);


            //dataset读取带属性的xml文件
            ds = new DataSet();
            ds.ReadXml(xmlPaths);
            //读取的节点名称
            string nodeName = "name";
            name = ds.Tables[nodeName].Rows[0][nodeName + "_Text"].ToString();
            //读取节点的id属性
            string id = ds.Tables[nodeName].Rows[0]["id"].ToString();
            //输出：id:1,name:张三
            Console.WriteLine("id:{0},name:{1}", id, name);

            Console.ReadKey();
        }
        private static void Second()
        {
            //先获取Program.cs 的 路径
            string root = Path.GetFullPath(Path.Combine(
                Path.GetDirectoryName(typeof(Program).Assembly.Location)));
            //拼接 XML目录
            string xmlPath = Path.GetFullPath(Path.Combine(root, "XML\\不带属性的文件.xml"));
            string xmlPaths = Path.GetFullPath(Path.Combine(root, "XML\\带属性文件.xml"));


            //XmlDocument读取xml文件
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(xmlPaths);

            //获取xml根节点
            XmlNode xmlRoot = xmlDoc.DocumentElement;
            //根据节点顺序逐步读取
            //读取第一个name节点
            string name = xmlRoot.SelectSingleNode("student/name").InnerText;

            //读取节点的id属性
            string id = xmlRoot.SelectSingleNode("student/name").Attributes["id"].InnerText;
            //输出：id:1,name:张三
            Console.WriteLine("id:{0},name:{1}", id, name);

            //读取所有的name节点
            foreach (XmlNode node in xmlRoot.SelectNodes("student/name"))
            {
                //循环输出
                Console.WriteLine("id:{0},name:{1}", node.Attributes["id"].InnerText, node.InnerText);
            }
            Console.ReadKey();
        }
        private static void Third()
        {

            //先获取Program.cs 的 路径
            string root = Path.GetFullPath(Path.Combine(
                Path.GetDirectoryName(typeof(Program).Assembly.Location)));
            //拼接 XML目录
            string xmlPath = Path.GetFullPath(Path.Combine(root, "XML\\不带属性的文件.xml"));
            string xmlPaths = Path.GetFullPath(Path.Combine(root, "XML\\带属性文件.xml"));


            //XmlDocument读取xml文件
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(xmlPaths);
            //转换为json
            string json = JsonConvert.SerializeXmlNode(xmlDoc);
            //解析json
            JObject jobj = JObject.Parse(json);
            string ss = "[" + jobj["studentList"]["student"].ToString() + "]";
            JArray jarr = new JArray();
            try
            {
                //Newtonsoft.Json.Linq.JArray.Parse 是用来解析数组的
                jarr = JArray.Parse(ss);//注意：Parse 解析的内容必须有"[]"括起来
            }
            catch (Exception ex)
            {
                Console.WriteLine("解析失败，" + ex.Message);
                return;
            }

            //输出：id:1,name:张三
            Console.WriteLine("id:{0},name:{1}", jarr[0]["name"]["@id"], jarr[0]["name"]["#text"]);
            Console.ReadKey();
        }
    }
}
