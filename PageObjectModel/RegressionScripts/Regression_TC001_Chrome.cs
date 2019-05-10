using AventStack.ExtentReports;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using SeleniumTestFramework.BaseScripts;
using SeleniumTestFramework.Resources;
using System;

namespace SeleniumTestFramework
{
    [TestFixture("chrome")]
    public class Regression_TC001_Chrome : ExtentManager
    {
        
        public Regression_TC001_Chrome(String browserName)
        {
            TestConfigurations.browserName = browserName;     
        }

        [Test]
        public void PerformGoogleSearch()
        {
            // 1. Find the search textbox (by ID) on the homepage
            var searchBox = WebDriverFromBaseClass.FindElement(By.Name("q"));
            AddScreenCapture();
            // 2. Enter the text (to search for) in the textbox
            searchBox.SendKeys("Automation using selenium 3.0 in C#");
            // 3. Find the search button (by Name) on the homepage
            var searchButton = WebDriverFromBaseClass.FindElement(By.Name("btnK"));
            // 4. Click "Submit" to start the search
            searchButton.Submit();
            // 5. Find the "Id" of the "Div" containing results stats,located just above the results table.
            var searchResults = WebDriverFromBaseClass.FindElement(By.Id("resultStats")).Text;
            testInstance.Log(Status.Pass, "Your first passing test with Search Results as " + searchResults);
        }

        
    }
}
