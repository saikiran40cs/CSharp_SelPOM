using AventStack.ExtentReports;
using NUnit.Framework;
using OpenQA.Selenium;
using SeleniumTestFramework.BaseScripts;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace COAT_POM.BaseMethods
{
    public class ScreenshotCapture : ExtentManager
    {

        private readonly IWebDriver DriverInsideAssertClass;


        public ScreenshotCapture(IWebDriver driver)
        {
            DriverInsideAssertClass = driver;
        }

        public String CaptureScreenShot()
        {
            try
            {
                // get the path of the currently executing assembly
                String currentPath = Assembly.GetExecutingAssembly().Location;
                // get the directory name of the current assembly
                String directory = Path.GetDirectoryName(currentPath);
                DirectoryInfo info = new DirectoryInfo(directory);
                String path = info.Parent.Parent.FullName;

                Screenshot screenImage = ((ITakesScreenshot)DriverInsideAssertClass).GetScreenshot();
                String imagename = DateTime.Now.ToString("yyyy_MM_dd_HH_mm_ss");
                String date = DateTime.Today.ToString("dd_MMM_yyyy");
                String TestResultLocation = path + "\\Reports\\ErrorScreenshots_" + date;
                if (Directory.Exists(TestResultLocation) == false){
                    Directory.CreateDirectory(TestResultLocation);
                }
                String localPathName = TestResultLocation + "\\" + testCaseName;
                if (Directory.Exists(localPathName) == false) {
                    Directory.CreateDirectory(localPathName);
                }
                screenShotPath = localPathName + "\\" + imagename + ".png";
                screenImage.SaveAsFile(screenShotPath, ScreenshotImageFormat.Png);
                Thinktime(2);
                return screenShotPath;
            }
            catch (Exception)
            {
                Console.WriteLine("Screensots Class failed to capture screens");
            }
            return "";
        }
    }
}
