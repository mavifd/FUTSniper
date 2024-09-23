using Discord.Webhook;
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

        public static DiscordWebhookClient DiscordLog = new DiscordWebhookClient("https://discord.com/api/webhooks/1287842012564815952/ga0kN_kJVZDWKSOSJafmZZkFWEDcj62VJe0C0aib5oLyf6oldoRBI3X4Gb_-qqXjWdxD");

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
                LogApp($"Fiyat ayarlanıyor...");
                DefaultWait<IWebDriver> wait = new DefaultWait<IWebDriver>(driver)
                {
                    Timeout = TimeSpan.FromSeconds(5),
                    PollingInterval = TimeSpan.FromMilliseconds(50)
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
                        LogApp($"Fiyat ayarlama başarılı.", Color.Green);
                    }
                    else
                    {
                        LogApp($"Fiyat ayarlama başarısız. (hata-01)", Color.Red); //hata 1
                    }
                }
                else
                {
                    LogApp($"Fiyat ayarlama başarısız. (hata-02)", Color.Red); //hata 2
                    ErrorCount++;
                }
            }
            catch (Exception ex)
            {
                LogApp($"Fiyat ayarlama başarısız. (hata-03)\nDetay:{ex}", Color.Red); //hata 3
                ErrorCount++;
            }
            return Task.CompletedTask;
        }

        private Task BackPageAction() //arama sayfasına dön
        {
            try
            {
                LogApp($"Arama sayfasına dönülüyor...");
                DefaultWait<IWebDriver> wait = new DefaultWait<IWebDriver>(driver)
                {
                    Timeout = TimeSpan.FromSeconds(5),
                    PollingInterval = TimeSpan.FromMilliseconds(50)
                };

                IWebElement pageTitle = wait.Until(drv =>
                {
                    var headers = drv.FindElements(By.TagName("h1"));
                    return headers.FirstOrDefault(header => header.Text == "Search Results" && header.GetAttribute("class") == "title");
                });

                if (pageTitle != null)
                {
                    driver.FindElement(By.ClassName("ut-navigation-button-control")).Click();
                    LogApp($"Arama sayfasına dönme başarılı.", Color.Green);
                }
                else
                {
                    LogApp($"Arama sayfasına dönme başarısız. (hata-04)", Color.Red); //hata 4
                }
            }
            catch (Exception ex)
            {
                LogApp($"Arama sayfasına dönme başarısız. (hata-05)\nDetay:{ex}", Color.Red); //hata 5
                ErrorCount++;
            }
            return Task.CompletedTask;
        }

        private Task SearchAction() //arama yap
        {
            try
            {
                LogApp($"Arama yapılıyor...");

                DefaultWait<IWebDriver> wait = new DefaultWait<IWebDriver>(driver)
                {
                    Timeout = TimeSpan.FromSeconds(5),
                    PollingInterval = TimeSpan.FromMilliseconds(50)
                };

                IWebElement searchButton = wait.Until(drv =>
                {
                    var buttons = drv.FindElements(By.TagName("button"));
                    return buttons.FirstOrDefault(header => header.Text == "Search");
                });

                if (searchButton != null)
                {
                    searchButton.Click();
                    LogApp($"Arama yapıldı.", Color.Green);
                }
                else
                {
                    LogApp($"Arama başarısız. (hata-06)", Color.Red); //hata 6
                }
            }
            catch (Exception ex)
            {
                LogApp($"Arama başarısız. (hata-07)\nDetay:{ex}", Color.Red); //hata 7
                ErrorCount++;
            }
            return Task.CompletedTask;
        }

        private async Task BuyFunction() //sonuçları kontrol et ve satın al
        {
            DefaultWait<IWebDriver> wait = new DefaultWait<IWebDriver>(driver)
            {
                Timeout = TimeSpan.FromSeconds(5),
                PollingInterval = TimeSpan.FromMilliseconds(50)
            };
            wait.IgnoreExceptionTypes(typeof(NoSuchElementException), typeof(StaleElementReferenceException));

            LogApp($"Arama sonuçları kontrol ediliyor...");

            try
            {
                IWebElement element = wait.Until(drv =>
                {
                    var noResult = drv.FindElements(By.TagName("section")).FirstOrDefault(el => el.Text.Contains("No results"));
                    if (noResult != null) return noResult;
                    return drv.FindElements(By.TagName("button")).FirstOrDefault(el => el.Text.Contains("Buy Now for"));
                });

                if (element != null)
                {
                    if (element.Text.Contains("No results"))
                    {
                        LogApp("Sonuç yok.");
                        return;
                    }
                    else if (element.Text.Contains("Buy Now for"))
                    {
                        LogApp("Oyuncu alınıyor... Aşama 1", Color.Green);
                        element.Click();
                    }
                    else
                    {
                        LogApp("Herhangi bir öge bulunamadı. Geçersiz sonuç. (error-08)", Color.Red); //hata 8
                    }
                }
                else
                {
                    LogApp("Herhangi bir öge bulunamadı. (error-09)", Color.Red); //hata 9
                    return;
                }
            }
            catch (Exception ex)
            {
                LogApp($"Arama sonuç kontrolü başarısız. (hata-010)\nDetay:{ex}", Color.Red); //hata 10
                ErrorCount++;
            }

            LogApp($"Satın alma onaylanıyor...");

            try
            {
                IWebElement confirmButton = wait.Until(drv =>
                {
                    var eles = drv.FindElements(By.TagName("button"));
                    return eles.FirstOrDefault(el => el.Text == "Ok");
                });

                if (confirmButton != null)
                {
                    LogApp($"Satın alma onaylandı!", Color.Green);
                    await DiscordMessage("Yeni bir satın alma tamamlandı.");
                    confirmButton.Click();
                    BuyCount++;
                    LogApp($"EAFC Sunucu güncellemesi için 25 saniye bekleniyor...");
                    await Task.Delay(25000);
                }
                else
                {
                    LogApp("Onay kutusu yok. (error-11)", Color.Red); //hata 11
                    return;
                }
            }
            catch (Exception ex)
            {
                LogApp($"Satın alma onay kontrolü başarısız. (hata-012)\nDetay:{ex}", Color.Red); //hata 12
                ErrorCount++;
            }
        }

        private async Task MainFunction(CancellationToken token)
        {
            LogApp($"Başlatılıyor...");

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

            LogApp("Sayfa: " + driver.Title);

            //IReadOnlyCollection<IWebElement> containers = driver.FindElements(By.CssSelector("*"));
            //foreach (IWebElement element in containers) { LogApp($"Tag: {element.TagName} | Text: {element.Text} | Class: {element.GetAttribute("class")}"); }

            int currentprice = int.Parse(maxpriceTxt.Text); ;
            int increment = int.Parse(changeTxt.Text);
            int maxvalue = int.Parse(maxchangeTxt.Text);
            int changeval = int.Parse(changevalueTxt.Text);
            int loopcount = 0;

            while (true)
            {
                LogApp($"Mevcut Fiyat: {currentprice}.");
                LogApp($"Döngü Sayısı: {loopcount}.");

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

                LogApp($"Hız Limiti Koruması - 2 Saniye Bekleme");

                await Task.Delay(2000);

                if (token.IsCancellationRequested)
                {
                    LogApp($"İşlem durdurma komutu alındı.", Color.DarkSalmon);
                    break;
                }
            }

            driver.Quit();
            LogApp($"İşlem sonu.", Color.DarkMagenta);
        }

        private void SnipeMain_Load(object sender, EventArgs e)
        {
            LogApp($"SnipeBot.", Color.Blue);
        }

        private void StartBtn_Click(object sender, EventArgs e)
        {
            LogApp($"İşlem başlatıldı.", Color.BurlyWood);
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

            LogApp($"İşlem durdurma komutu verildi.", Color.DarkSalmon);
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

        public static async Task DiscordMessage(string message, bool ping = false)
        {
            string logEntry = $"{DateTime.Now:dd/MM HH:mm} | {message}";
            if (ping) logEntry = $"{DateTime.Now:dd/MM HH:mm} | <@everyone> | {message}";
            await DiscordLog.SendMessageAsync(logEntry);
        }

        private async void buycountTxt_TextChanged(object sender, EventArgs e)
        {
            await DiscordMessage($"Yeni bir satın alma tamamlandı. (Toplam: {buycountTxt.Text})", true);
        }

        private async void errorcountTxt_TextChanged(object sender, EventArgs e)
        {
            await DiscordMessage($"Bir hata oluştu. (Toplam: {errorcountTxt.Text})", false);
        }
    }
}