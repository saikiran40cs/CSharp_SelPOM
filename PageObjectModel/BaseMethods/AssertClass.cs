using NUnit.Framework;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeleniumTestFramework.BaseScripts
{
    public class AssertClass 
    {
        private readonly IWebDriver DriverInsideAssertClass;

        public AssertClass(IWebDriver driver)
        {
            DriverInsideAssertClass = driver;
        }
        
        public  void AssertElementIsPresent(IWebElement element)
        {
            try
            {
                Assert.AreEqual(true, IsElementPresent(element));

            }

            catch (Exception ex)
            {
                Console.WriteLine("Test Failed, No element {0} is found ", ex.Message);
                throw ex;
            }
        }

        // Makes sure that the test case will not fail unless its absolutely necessary. Returns a true or false to AssertElement is Presesent Method
        public  bool IsElementPresent(IWebElement element)
        {
            bool Present = false;
            try
            {
                DriverInsideAssertClass.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(60);
                if (element.Displayed)
                {
                    Present = true;
                    Console.WriteLine("Element Is Seen");
                }

                else
                {
                    DriverInsideAssertClass.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(60);
                    if (element.Displayed)
                    {
                        Present = true;
                    }
                }
                return Present;
            }
            catch (NoSuchElementException ex)
            {
                Console.WriteLine("No Such Element : {0}", ex.Message);
                Assert.Fail("No Such Element");
                return false;
            }
        }


        /** Checks for Text and confirms its a match*/
        public  void ContainsText(IWebElement element, string neededText)
        {
            string actualText = element.Text;
            try
            {
                if (actualText.Contains(neededText))
                {
                    Console.WriteLine("Text Matched");
                }
                else
                    Assert.Fail("Text mismatch");
            }
            catch (Exception e)
            {
                Console.WriteLine("Text Mismatched : {0}", e.Message);
                throw e;
            }
        }
    }
}
