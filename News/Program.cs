using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace News
{
    class Program
    {
        static void Main(string[] args)
        {
            foreach (Item item in ItemGet())
            {
                Console.WriteLine(item);
            }

        }
        static XmlDocument GetNewsItems()
        {
            XmlDocument doc = new XmlDocument();
            XmlElement root = doc.CreateElement("items");
            try
            {
                doc.Load("https://habrahabr.ru/rss/interesting/");
                if (doc.HasChildNodes)
                {
                    foreach (XmlNode rootItem in doc.SelectNodes("rss/channel/item"))
                    {
                        string title = rootItem.SelectSingleNode("title").InnerText;
                        string description = rootItem.SelectSingleNode("description").InnerText;

                        XmlElement ItemNews = doc.CreateElement(title);
                        ItemNews.InnerText = description;

                        XmlAttribute pubDateAtr = doc.CreateAttribute("pubDate");
                        pubDateAtr.InnerText = rootItem.SelectSingleNode("pubDate").InnerText;

                        ItemNews.Attributes.Append(pubDateAtr);

                        root.AppendChild(ItemNews);
                    }
                    doc.AppendChild(root);
                    doc.Save("Items.xml");
                }
                return doc;
            }
            catch (Exception)
            {
                return null;
            }
        }
        static List<Item> ItemGet()
        {
            List<Item> items = new List<Item>();

            foreach (XmlNode i in GetNewsItems().SelectNodes("rss/chanel/item"))
            {
                Item item = new Item();
                item.Title = i.SelectSingleNode("title").InnerText;
                item.Link = i.SelectSingleNode("link").InnerText;
                item.Descrition = i.SelectSingleNode("description").InnerText;
                item.PubDate = DateTime.Parse(i.SelectSingleNode("pubDate").InnerText);
                items.Add(item);
            }
            return items;
        }
        public class Item
        {
            public string Title { get; set; }
            public string Link { get; set; }
            public string Descrition { get; set; }

            public DateTime PubDate { get; set; }
        }

    }
}
