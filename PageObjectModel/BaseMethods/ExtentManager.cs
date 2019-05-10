using AventStack.ExtentReports;
using AventStack.ExtentReports.Reporter;
using NUnit.Framework;
using NUnit.Framework.Interfaces;
using System;
using System.Diagnostics;
using System.IO;

namespace SeleniumTestFramework.BaseScripts
{
    [SetUpFixture]
    public class ExtentManager : BaseScript
    {
        protected ExtentReports extentInstance;
        protected ExtentTest testInstance;
        protected ExtentKlovReporter klov;

        [OneTimeSetUp]
        protected void Setup()
        {
            testCaseName = TestContext.CurrentContext.Test.Name;
            DirectoryInfo dir = new DirectoryInfo(AppDomain.CurrentDomain.BaseDirectory).Parent.Parent;
            string htmlReportName = dir.FullName + @"\Reports\TestAutomationReport.html";
            var htmlReporter = new ExtentHtmlReporter(htmlReportName);
            htmlReporter.LoadConfig(dir.FullName + @"\Resources\extent-config.xml");
            htmlReporter.AnalysisStrategy = AnalysisStrategy.Class;
            extentInstance = new ExtentReports();
            extentInstance.AddSystemInfo("Operating System","Windows 10");
            extentInstance.AddSystemInfo("Selenium Version", "3.141.59");
            extentInstance.AttachReporter(htmlReporter);
            testInstance = extentInstance.CreateTest(testCaseName, "Executing the test to verify application");
            testInstance.AssignAuthor(Environment.UserName);
            testInstance.AssignCategory("Regression");
        }


        public void KlovReportCreation()
        {
            //Use for Historical Report
            klov = new ExtentKlovReporter();
            // specify mongoDb connection
            klov.InitMongoDbConnection("localhost", 27017);
            // specify project !you must specify a project, other a "Default project will be used"
            klov.ProjectName = "Automation_Project";
            // you must specify a reportName otherwise a default timestamp will be used
            klov.ReportName = "COAT QA " + DateTime.Now.ToString();
            // URL of the KLOV server
            extentInstance.AttachReporter(klov);
        }

        [OneTimeTearDown]
        protected void TearDown()
        {
            extentInstance.Flush();
        }

        /* Function to capture the screenshot and add it to extentreports */
        protected void AddScreenCapture()
        {
            screenShotPath = sCapture.CaptureScreenShot();
            //Console.WriteLine("The Screenshot Path is" + screenShotPath);
            testInstance.Log(Status.Info, "Attached the Snapshot below: ");
            testInstance.AddScreenCaptureFromPath(screenShotPath);
        }

        [TearDown]
        public void AfterTest()
        {
            var errorMessage = TestContext.CurrentContext.Result.Message;
            var status = TestContext.CurrentContext.Result.Outcome.Status;
            var stacktrace = string.IsNullOrEmpty(TestContext.CurrentContext.Result.StackTrace)
                    ? ""
                    : string.Format("{0}", TestContext.CurrentContext.Result.StackTrace);
            Status logstatus;
            switch (status)
            {
                case TestStatus.Failed:
                    logstatus = Status.Fail;
                    testInstance.Log(logstatus, "The Test Script Failed with status " + logstatus + " & stacktrace:" + errorMessage);
                    //Capture only if it is a failure
                    if (TestContext.CurrentContext.Result.Outcome != ResultState.Success){
                        AddScreenCapture();
                    }
                    break;
                case TestStatus.Inconclusive:
                    logstatus = Status.Warning;
                    testInstance.Log(logstatus, "Test Script Ended Abruptly with status " + logstatus + " & stacktrace:" + stacktrace);
                    //Capture only if it is a failure
                    if (TestContext.CurrentContext.Result.Outcome != ResultState.Success)
                    {
                        AddScreenCapture();
                    }
                    break;
                case TestStatus.Skipped:
                    logstatus = Status.Skip;
                    testInstance.Log(logstatus, "The Test Script Skipped with status " + logstatus + " & stacktrace:" + stacktrace);
                    break;
                default:
                    logstatus = Status.Pass;
                    testInstance.Log(logstatus, "The Test Script Ended with status " + logstatus+" successfully.");
                    //Capture only if it is a failure
                    if (TestContext.CurrentContext.Result.Outcome != ResultState.Success)
                    {
                        AddScreenCapture();
                    }
                    break;
            }
            extentInstance.Flush();
        }

    }
}
