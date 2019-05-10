using COAT_POM.BaseMethods;
using NUnit.Framework;
using NUnit.Framework.Interfaces;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Edge;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.IE;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Support.UI;
using SeleniumTestFramework.Resources;
using System;
using System.Drawing.Imaging;

namespace SeleniumTestFramework.BaseScripts
{
    public class BaseScript
    {
        protected String testCaseName;
        public AssertClass aClass;
        public ScreenshotCapture sCapture;
        public String screenShotPath;
        public IWebDriver WebDriverFromBaseClass { get; set; }

        [SetUp]
        public void SetUpDriver()
        {
            switch (TestConfigurations.browserName)
            {
                case "firefox":
                    FirefoxOptions firefoxOptions = new FirefoxOptions();
                    firefoxOptions.AddArguments("--start-maximized");
                    WebDriverFromBaseClass = new FirefoxDriver(firefoxOptions);
                    break;
                case "chrome":
                    ChromeOptions chromeOptions = new ChromeOptions();
                    chromeOptions.AddAdditionalCapability("useAutomationExtension", false);
                    chromeOptions.AddArguments("--start-maximized");
                    WebDriverFromBaseClass = new ChromeDriver(chromeOptions);
                    break;
                case "ie":
                    InternetExplorerOptions ieOptions = new InternetExplorerOptions();
                    //ieOptions.AddArguments("--start-maximized");
                    //IEDriverServer.exe
                    WebDriverFromBaseClass = new InternetExplorerDriver();
                    break;
                case "edge":
                    WebDriverFromBaseClass = new EdgeDriver();
                    //EdgeOptions edOptions = new EdgeOptions();
                    //edOptions.AddAdditionalCapability("--start-maximized", true);
                    //WebDriverFromBaseClass = new EdgeDriver(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), edOptions);
                    break;
                default:
                    Console.WriteLine("Improper Browser Name Keyed in");
                    break;
            }
            WebDriverFromBaseClass.Navigate().GoToUrl(TestConfigurations.baseUrl);
            aClass = new AssertClass(WebDriverFromBaseClass);
            sCapture = new ScreenshotCapture(WebDriverFromBaseClass);
        }

        // custom method for entering a text in to a field
        public void EnterText(IWebElement element, string value)
        {
            element.Click();
            element.Clear();
            element.SendKeys(value);
        }

        public void EnterDropDownText(IWebElement element, string value)
        {
            element.SendKeys(value);
        }

        // Custom Method for Selecting from a dropdown. we can specifty select type as Text, Value or Index
        public void SelectFromDropDownByText(IWebElement element, string inputText)
        {
            //new SelectElement(element).SelectByText(inputValue);
            SelectElement se = new SelectElement(element);
            se.SelectByText(inputText);
        }
        public void SelectFromDropDownByValue(IWebElement element, string inputValue)
        {
            //new SelectElement(element).SelectByValue(inputValue);
            SelectElement se = new SelectElement(element);
            se.SelectByValue(inputValue);
        }
        public void SelectFromDropDownByIndex(IWebElement element, int index)
        {
            //new SelectElement(element).SelectByValue(inputValue);
            SelectElement se = new SelectElement(element);
            se.SelectByIndex(index);
        }

        // Custom Method for Drag and drop. we need to specify the origin element and destination elements
        public void DragAndDropItem(IWebElement sourceElement, IWebElement destinationElement)
        {
            Actions action = new Actions(WebDriverFromBaseClass);
            action.ClickAndHold(sourceElement).MoveToElement(destinationElement).Release().Build().Perform();
        }

        public void ActionClick(IWebElement Element)
        {
            Actions action = new Actions(WebDriverFromBaseClass);
            action.MoveToElement(Element).Build().Perform();
        }


        // This method removes all spaces present between the text that are inserted 
        // accidentaly by the user and converts the text to upper Case
        public string TrimAllSpace(string Text)
        {
            return String.Join(" ", Text.Split(new char[] { ' ' },
                StringSplitOptions.RemoveEmptyEntries));

        }

        public void ScrollintoView(IWebElement Element)
        {
            IJavaScriptExecutor js = WebDriverFromBaseClass as IJavaScriptExecutor;
            // Run the javascript command 'scrollintoview on the element
            js.ExecuteScript("arguments[0].scrollIntoView(true);", Element);
            Thinktime(2);
        }

        // Method to click and verify page navigation 
        public void ClickPageNavigation(IWebElement hyperlink, IWebElement landingPageObj)
        {
            try
            {
                hyperlink.Click();
                Thinktime(5);
                aClass.AssertElementIsPresent(landingPageObj);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Not able to navigate : {0}", ex.Message);
                throw ex;
            }

        }

        public void Thinktime(int Time)
        {
            System.Threading.Thread.Sleep(Time * 1000);
        }

        [OneTimeTearDown]
        public void CloseDriver()
        {
            WebDriverFromBaseClass.Close();
        }
    }
}
