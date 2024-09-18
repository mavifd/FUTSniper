using OpenQA.Selenium;
using OpenQA.Selenium.Edge;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SnipeBot
{
    public partial class SnipeMain : Form
    {
        private CancellationTokenSource _cts = new CancellationTokenSource();
        private EdgeDriver driver;

        private int BuyCount = 0;
        private int LoopCount = 0;
        private int ErrorCount = 0;

        public SnipeMain()
        {
            InitializeComponent();
        }

        private Task PriceSetup(int loopcount, int currentprice, int maxvalue, int increment) // fiyatı ayarla
        {
            try
            {

                DefaultWait<IWebDriver> wait = new DefaultWait<IWebDriver>(driver)
                {
                    Timeout = TimeSpan.FromSeconds(10),
                    PollingInterval = TimeSpan.FromMilliseconds(100)
                };

                IList<IWebElement> priceInputs = wait.Until(drv =>
                {
                    var elements = drv.FindElements(By.ClassName("ut-number-input-control"));
                    return elements.Any() ? elements : null;
                });

                if (priceInputs != null && priceInputs.Count > 0)
                {
                    if (!string.IsNullOrEmpty(maxpriceTxt.Text) && maxpriceTxt.Text != "0")
                    {
                        priceInputs[3].Clear();
                        priceInputs[3].SendKeys(currentprice.ToString());
                    }
                    LogApp($"Prices adjusted.");
                }
                else
                {
                    LogApp($"Price input not found.", Color.Red);
                    ErrorCount++;
                }
            }
            catch (Exception ex)
            {
                LogApp($"Price Setup Failed: {ex}", Color.Red);
                ErrorCount++;
            }
            return Task.CompletedTask;
        }

        private Task BackPageAction() //arama sayfasına dön
        {
            try
            {
                DefaultWait<IWebDriver> wait = new DefaultWait<IWebDriver>(driver)
                {
                    Timeout = TimeSpan.FromSeconds(10),
                    PollingInterval = TimeSpan.FromMilliseconds(100)
                };

                IWebElement pageTitle = wait.Until(drv =>
                {
                    var headers = drv.FindElements(By.TagName("h1"));
                    return headers.FirstOrDefault(header => header.Text == "Search Results" && header.GetAttribute("class") == "title");
                });

                if (pageTitle != null)
                {
                    driver.FindElement(By.ClassName("ut-navigation-button-control")).Click();
                    LogApp($"Returned to the search page.");
                }
            }
            catch (Exception ex)
            {
                LogApp($"Back Page Action Failed: {ex}", Color.Red);
                ErrorCount++;
            }
            return Task.CompletedTask;
        }

        private Task SearchAction() //arama yap
        {
            try
            {
                DefaultWait<IWebDriver> wait = new DefaultWait<IWebDriver>(driver)
                {
                    Timeout = TimeSpan.FromSeconds(10),
                    PollingInterval = TimeSpan.FromMilliseconds(100)
                };

                IWebElement searchButton = wait.Until(drv =>
                {
                    var buttons = drv.FindElements(By.TagName("button"));
                    return buttons.FirstOrDefault(header => header.Text == "Search");
                });

                if (searchButton != null)
                {
                    searchButton.Click();
                    LogApp($"Searching...");
                }
            }
            catch (Exception ex)
            {
                LogApp($"Search Action Failed: {ex}", Color.Red);
                ErrorCount++;
            }
            return Task.CompletedTask;
        }


        private async Task BuyFunction()
        {
            DefaultWait<IWebDriver> wait = new DefaultWait<IWebDriver>(driver)
            {
                Timeout = TimeSpan.FromSeconds(3),
                PollingInterval = TimeSpan.FromMilliseconds(500)
            };

            wait.IgnoreExceptionTypes(typeof(NoSuchElementException), typeof(StaleElementReferenceException));

            IWebElement element = wait.Until(drv =>
            {
                var noResultsElements = drv.FindElements(By.TagName("section"));
                var noResult = noResultsElements.FirstOrDefault(el => el.Text.Contains("No results"));
                if (noResult != null) return noResult;
                var buyButtonElements = drv.FindElements(By.TagName("button"));
                var buyButton = buyButtonElements.FirstOrDefault(el => el.Text.Contains("Buy Now for"));
                return buyButton;
            });

            if (element != null)
            {
                if (element.Text.Contains("No results"))
                {
                    LogApp("no results.");
                    return;
                }
                else if (element.Text.Contains("Buy Now for"))
                {
                    LogApp("buying player... [1]");
                    element.Click();
                }
            }
            else
            {
                LogApp("nothing founded. [error-1]");
                return;
            }

            IWebElement confirmButton = wait.Until(drv =>
            {
                var eles = drv.FindElements(By.TagName("button"));
                return eles.FirstOrDefault(el => el.Text == "Ok");
            });

            if (confirmButton != null)
            {
                LogApp($"Buying confirmed!", Color.Green);
                confirmButton.Click();
                BuyCount++;
                LogApp($"Waiting for bid update. (25 sec.)");
                await Task.Delay(25000);
            }
        }


        private async Task MainFunction(CancellationToken token)
        {
            LogApp($"Starting...");

            var options = new EdgeOptions();
            options.DebuggerAddress = "localhost:9111";

            var service = EdgeDriverService.CreateDefaultService();
            service.HideCommandPromptWindow = false;

            driver = new EdgeDriver(service, options);

            // Set Page EAFC
            var windowHandles = driver.WindowHandles;
            foreach (var handle in windowHandles)
            {
                driver.SwitchTo().Window(handle);
                if (driver.Title.Contains("FC Ultimate")) break;
            }

            LogApp("Title: " + driver.Title);

            //IReadOnlyCollection<IWebElement> containers = driver.FindElements(By.CssSelector("*"));
            //foreach (IWebElement element in containers) { LogApp($"Tag: {element.TagName} | Text: {element.Text} | Class: {element.GetAttribute("class")}"); }     

            int currentprice = int.Parse(maxpriceTxt.Text); ;
            int increment = int.Parse(changeTxt.Text);
            int maxvalue = int.Parse(maxchangeTxt.Text);
            int changeval = int.Parse(changevalueTxt.Text);
            int loopcount = 0;

            while (true)
            {
                LogApp($"Current Price: {currentprice}.");
                LogApp($"Loop Count: {loopcount}.");

                if (loopcount != 0) // ilk döngü - fiyat değişimi yok - ilk fiyat 300
                {
                    if (currentprice <= maxvalue) currentprice += increment;
                    else if (currentprice == maxvalue) currentprice += changeval;
                    else if (currentprice >= maxvalue) currentprice -= changeval;
                }

                await PriceSetup(loopcount, currentprice, maxvalue, increment);

                await SearchAction();

                await BuyFunction();

                await BackPageAction();

                loopcount++;

                LoopCount = loopcount;

                LogApp($"Rate Limit - 2 Second.");

                await Task.Delay(2000);

                if (token.IsCancellationRequested)
                {
                    LogApp($"CancellationRequested.", Color.DarkSalmon);
                    break;
                }
            }

            driver.Quit();
            LogApp($"End.", Color.DarkMagenta);
        }

        private void SnipeMain_Load(object sender, EventArgs e)
        {
            LogApp($"SnipeBot Main.", Color.Blue);
        }

        private void StartBtn_Click(object sender, EventArgs e)
        {
            LogApp($"Thread created.", Color.BurlyWood);
            StartBtn.Enabled = false;
            StopBtn.Enabled = true;

            maxpriceTxt.Enabled = false;
            changeTxt.Enabled = false;
            maxchangeTxt.Enabled = false;
            changevalueTxt.Enabled = false;


            _cts = new CancellationTokenSource();
            var token = _cts.Token;

            Task.Run(async () => await MainFunction(token));
        }

        private void StopBtn_Click(object sender, EventArgs e)
        {
            StartBtn.Enabled = true;
            StopBtn.Enabled = false;

            maxpriceTxt.Enabled = true;
            changeTxt.Enabled = true;
            maxchangeTxt.Enabled = true;
            changevalueTxt.Enabled = true;

            LogApp($"Thread cancelling.", Color.DarkSalmon);
            _cts?.Cancel();
        }

        private void UpdateCounts()
        {
            UpdateText(buycountTxt, BuyCount.ToString());
            UpdateText(loopcountTxt, LoopCount.ToString());
            UpdateText(errorcountTxt, ErrorCount.ToString());
        }

        private void UpdateText(Control control, string text)
        {
            if (control.InvokeRequired)
            {
                control.Invoke(new Action(() =>
                {
                    control.Text = text;
                }));
            }
            else
            {
                control.Text = text;
            }
        }

        private void LogApp(string message, Color? color = null)
        {
            string text = $"[{DateTime.Now:dd/MM HH:mm}] {message}{Environment.NewLine}";
            if (LoggerTXT.InvokeRequired)
            {
                LoggerTXT.Invoke(new Action(() =>
                {
                    AppendColoredText(text, color ?? LoggerTXT.ForeColor);
                }));
            }
            else
            {
                AppendColoredText(text, color ?? LoggerTXT.ForeColor);
            }
        }

        private void AppendColoredText(string text, Color color)
        {
            int start = LoggerTXT.TextLength;
            LoggerTXT.AppendText(text);
            int end = LoggerTXT.TextLength;
            LoggerTXT.Select(start, end - start);
            LoggerTXT.SelectionColor = color;
            LoggerTXT.SelectionLength = 0;
            LoggerTXT.SelectionColor = LoggerTXT.ForeColor;
            LoggerTXT.Focus();
        }

        private void StatUpdater_Tick(object sender, EventArgs e)
        {
            UpdateCounts();
        }

        private void resetstatsBtn_Click(object sender, EventArgs e)
        {
            BuyCount = 0;
            LoopCount = 0;
            ErrorCount = 0;
        }

        private void topmostcb_CheckedChanged(object sender, EventArgs e)
        {
            this.TopMost = topmostcb.Checked;
        }
    }
}