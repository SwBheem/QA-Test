using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using OpenQA.Selenium.Edge;
using System;

namespace QA_Test
{
    [TestClass]
    public class RegistrationPage
    {
          
        public static bool ValidationTextIsCorrect(IWebDriver driver, string elementPath,string validationText)
        {
                
                return driver.FindElement(By.XPath(elementPath)).Text.Contains(validationText);
        }

        [TestMethod]
        public void RegisterPage_Registration()
        {
            //using (IWebDriver driver = new ChromeDriver())
            using (IWebDriver driver = new EdgeDriver())
            { 
                string url = "https://dev.id.netwealth.com/account/register";
             
                driver.Navigate().GoToUrl(url);
                Assert.IsTrue(driver.Url.StartsWith(url));
                driver.Manage().Window.Maximize();
                driver.FindElement(By.Id("jAccept")).Click();

                //Registration Process
                driver.FindElement(By.Id("FirstName")).SendKeys("first");
                driver.FindElement(By.Id("LastName")).SendKeys("user");
                driver.FindElement(By.Id("Email")).SendKeys("first.user@email.com");
                driver.FindElement(By.Id("Password")).SendKeys("SamplePassword12%");
                driver.FindElement(By.Id("ReferralSource")).FindElement(By.CssSelector("option[value='3']")).Click();
                driver.FindElement(By.Id("HasOptedOutOfMarketingMaterial")).Click();
                driver.FindElement(By.Name("RegistrationForm")).Click();
                System.Threading.Thread.Sleep(2000);
                                
                Assert.IsTrue(driver.Url.StartsWith("https://dev.id.netwealth.com/Account/RegisterCompleted"));

                //Verify user login
                driver.FindElement(By.XPath("/html/body/div[1]/nav/ul[1]/li/a")).Click();
                driver.FindElement(By.Id("Email")).SendKeys("first.user@email.com");
                driver.FindElement(By.Id("Password")).SendKeys("SamplePassword12%");
                driver.FindElement(By.CssSelector("body > main > div > div:nth-child(2) > div > form > button")).Click();

                Assert.IsTrue(ValidationTextIsCorrect(driver, "/html/body/main/div/div/div/h1", "Email address not confirmed"));
            }
        }

        public static bool ValidationMessageIsCorrect(IWebDriver driver, string element, string errorText)
        {

            return driver.FindElement(By.Id(element)).Text.Equals(errorText);
        }

        [TestMethod]
        public void RegisterPage_FieldValidation()
        {
            using (IWebDriver driver = new ChromeDriver())
            //using (IWebDriver driver = new EdgeDriver())
            {
                string url = "https://dev.id.netwealth.com/account/register";

                driver.Navigate().GoToUrl(url);
                Assert.IsTrue(driver.Url.StartsWith(url));
                driver.Manage().Window.Maximize();
                driver.FindElement(By.Id("jAccept")).Click();
                                
                driver.FindElement(By.Name("RegistrationForm")).Click();
                Assert.IsTrue(ValidationMessageIsCorrect(driver, "FirstName-error", "Please provide your first name"));
                Assert.IsTrue(ValidationMessageIsCorrect(driver, "LastName-error", "Please provide your last name"));
                Assert.IsTrue(ValidationMessageIsCorrect(driver, "Email-error", "Please provide an email address"));
                Assert.IsTrue(ValidationMessageIsCorrect(driver, "Password-error", "Please enter a valid password"));
                Assert.IsTrue(ValidationMessageIsCorrect(driver, "ReferralSource-error", "Please tell us where you heard about Netwealth"));

            }
        }
    }

    
}
