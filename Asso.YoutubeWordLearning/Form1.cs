using CefSharp;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Asso.YoutubeWordLearning
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

            chromiumWebBrowser1.Load("https://www.youtube.com/watch?v=Hu4Yvq-g7_Y");
        }

        private void chromiumWebBrowser1_LoadingStateChanged(object sender, CefSharp.LoadingStateChangedEventArgs e)
        {

        }

        public List<string> GetHtmlElements(string xPath)
        {
            //outerHTML
            List<string> retVal = new List<string>();
            var script =
                "function foo(){let xPathResult = document.evaluate('" + xPath.Replace('\'', '\"') + "', document, null, XPathResult.ANY_TYPE, null);" +
                "let nodes = [];  let node = xPathResult.iterateNext();" +
                "while (node) { nodes.push(node); node = xPathResult.iterateNext();} return nodes.map(x => ({outerHTML: x.textContent}));}foo()";

            //var script = "function foo(){const nodes = [];return nodes;}foo();";

            Task<JavascriptResponse> result = chromiumWebBrowser1.EvaluateScriptAsync(script);
            result.Wait();
            //if (result.Result.Success && result.Result.Result != null)
            //    retVal = Convert.ToInt32(result.Result.Result);

            List<object> abc = (List<object>)result.Result.Result;
            foreach (System.Dynamic.ExpandoObject item in abc)
            {
                // Console.WriteLine(item._data.ToString());

                IDictionary<string, object> propertyValues = item;

                foreach (var property in propertyValues.Keys)
                {
                    retVal.Add(propertyValues[property].ToString());
                    break;
                    //Console.WriteLine(String.Format("{0} : {1}", property, propertyValues[property]));
                }
            }



            return retVal;
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            string subtitleXpath = "//span['ytp-caption-segment']";
            List<string> els = GetHtmlElements(subtitleXpath);
            foreach (var item in els)
            {

                Debug.WriteLine(item);
            }

            string timePointXpath = "//span['ytp-time-current']";
            els = GetHtmlElements(timePointXpath);
            foreach (var item in els)
            {

                Debug.WriteLine(item);
            }
        }
    }
}
