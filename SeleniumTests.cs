using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;
using System;


[TestFixture]
public class SeleniumTests
{
    IWebDriver driver;

    [OneTimeSetUp]
    public void Setup()
    {
        driver = new ChromeDriver();
    }

    [Test]
    [Category("Google Search")]
    public void Test_GoogleSearch()
    {
        driver.Navigate().GoToUrl("https://www.google.com/");
        driver.SwitchTo().Frame(driver.FindElement(By.CssSelector("iframe")));
        driver.FindElement(By.CssSelector("#introAgreeButton")).Click();
        driver.SwitchTo().DefaultContent();

        IWebElement searchBar = driver.FindElement(By.XPath("//div[1]/div[3]/form/div[2]/div[1]/div[1]/div/div[2]/input"));
        searchBar.SendKeys("selenium");
        searchBar.SendKeys(Keys.Enter);

        IWebElement resultsPanel = driver.FindElement(By.Id("search"));

        var searchResults = resultsPanel.FindElements(By.XPath(".//a"));
        searchResults[0].Click();

        Assert.AreEqual(driver.Url, "https://www.selenium.dev/");
        Assert.AreEqual(driver.Title, "SeleniumHQ Browser Automation");
    }

    [Test]
    [Category("Wikipedia Search")]
    public void Test_WikipediaSearch()
    {
        driver.Navigate().GoToUrl("https://wikipedia.org");

        IWebElement searchBox = driver.FindElement(By.Id("searchInput"));
        searchBox.SendKeys("QA");
        searchBox.SendKeys(Keys.Enter);

        Assert.AreEqual("https://en.wikipedia.org/wiki/QA", driver.Url);
    }

    [Test]
    [Category("Summator of Numbers App")]
    public void Test_CheckCorrectSum()
    {
        driver.Navigate().GoToUrl("https://sum-numbers.evgenidimitrov0.repl.co");
        IWebElement firstNumber = driver.FindElement(By.Id("number1"));
        IWebElement secondNumber = driver.FindElement(By.Id("number2"));
        IWebElement calcButton = driver.FindElement(By.Id("calcButton"));
        IWebElement result = driver.FindElement(By.CssSelector("#result"));

        Random rand = new Random();
        int firstN = rand.Next(-1000, 1000);
        int secondN = rand.Next(-1000, 1000);
        firstNumber.SendKeys(firstN.ToString());
        secondNumber.SendKeys(secondN.ToString());
        calcButton.Click();

        Assert.AreEqual("Sum: " + (firstN + secondN), result.Text);
    }

    [Test]
    [Category("Summator of Numbers App")]
    public void Test_CheckIncorrectDataForSum()
    {
        driver.Navigate().GoToUrl("https://sum-numbers.evgenidimitrov0.repl.co");
        IWebElement firstNumber = driver.FindElement(By.Id("number1"));
        IWebElement secondNumber = driver.FindElement(By.Id("number2"));
        IWebElement calcButton = driver.FindElement(By.Id("calcButton"));
        IWebElement result = driver.FindElement(By.CssSelector("#result"));

        firstNumber.SendKeys("one");
        secondNumber.SendKeys("two");
        calcButton.Click();

        Assert.AreEqual("Sum: invalid input", result.Text);
    }

    [Test]
    [Category("Summator of Numbers App")]
    public void Test_ResetButton()
    {
        driver.Navigate().GoToUrl("https://sum-numbers.evgenidimitrov0.repl.co");

        IWebElement firstNumber = driver.FindElement(By.Id("number1"));
        IWebElement secondNumber = driver.FindElement(By.Id("number2"));
        IWebElement calcButton = driver.FindElement(By.Id("calcButton"));
        IWebElement resetButton = driver.FindElement(By.Id("resetButton"));
        IWebElement result = driver.FindElement(By.CssSelector("#result"));

        firstNumber.SendKeys("1");
        secondNumber.SendKeys("8");
        calcButton.Click();

        Assert.IsTrue(!string.IsNullOrEmpty(firstNumber.GetAttribute("value")));
        Assert.IsTrue(!string.IsNullOrEmpty(secondNumber.GetAttribute("value")));
        Assert.IsTrue(!string.IsNullOrEmpty(result.Text));

        resetButton.Click();

        Assert.IsTrue(string.IsNullOrEmpty(firstNumber.GetAttribute("value")));
        Assert.IsTrue(string.IsNullOrEmpty(secondNumber.GetAttribute("value")));
        Assert.IsTrue(string.IsNullOrEmpty(result.Text));
    }

    [Test]
    [Category("Automation Practice Navigation")]
    public void Test_AutomationPracticeCheckEmail()
    {
        WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(5));
        driver.Navigate().GoToUrl("http://automationpractice.com/index.php");

        IWebElement signIn = driver.FindElement(By.CssSelector("a.login"));
        signIn.Click();

        IWebElement createWithEmail = driver.FindElement(By.XPath("//*[@id=\"email_create\"]"));
        string emailSent = "getsata@imail.com";
        createWithEmail.SendKeys(emailSent);

        driver.FindElement(By.XPath("//*[@id=\"SubmitCreate\"]")).Click();

        wait.Until(ExpectedConditions.UrlContains("#account-creation"));
        IWebElement emailConfirm = driver.FindElement(By.Id("email"));
        Assert.AreEqual(emailSent, emailConfirm.GetAttribute("value"));
    }

    [OneTimeTearDown]
    public void TearDown()
    {
        driver.Quit();
    }
}