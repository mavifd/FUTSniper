using Discord.Webhook;
using Newtonsoft.Json.Linq;
using OpenQA.Selenium;
using OpenQA.Selenium.Edge;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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

        private async Task PriceSetup(int loopcount, int currentprice, int maxvalue, int increment, int minbidval, CancellationToken token) // fiyatı ayarla
        {
            try
            {
                LogApp($"Fiyat ayarlanıyor...");
                if (token.IsCancellationRequested || driver == null)
                {
                    LogApp($"İşlem durdurma komutu alındı.", Color.DarkSalmon);
                    return;
                }
                int loopcounter = 0;
                while (loopcounter < processtimer.Value)
                {
                    IReadOnlyList<IWebElement> priceInput = driver.FindElements(By.ClassName("ut-number-input-control"));
                    if (priceInput.Count != 0)
                    {
                        priceInput[0].Clear();
                        priceInput[0].SendKeys(minbidval.ToString());
                        priceInput[3].Clear();
                        priceInput[3].SendKeys(currentprice.ToString());
                        LogApp($"Fiyat ayarlama başarılı.", Color.Green);
                        return;
                    }
                    await Task.Delay(100);
                    loopcounter++;
                }
                LogApp($"Fiyat ayarlama başarısız. (hata-01)", Color.Red);
            }
            catch (Exception ex)
            {
                LogApp($"Fiyat ayarlama başarısız. (hata-02)\nDetay:{ex}", Color.Red);
                await DiscordMessage($"Fiyat ayarlama başarısız. (hata-02)\nDetay:{ex}");
                ErrorCount++;
            }
            return;
        }

        private async Task BackPageAction(CancellationToken token) //arama sayfasına dön
        {
            try
            {
                LogApp($"Arama sayfasına dönülüyor...");
                if (token.IsCancellationRequested || driver == null)
                {
                    LogApp($"İşlem durdurma komutu alındı.", Color.DarkSalmon);
                    return;
                }
                int loopcounter = 0;
                while (loopcounter < processtimer.Value)
                {
                    IWebElement backPgs = driver.FindElements(By.TagName("h1")).FirstOrDefault(header => header.Text == "Search Results" && header.GetAttribute("class") == "title");
                    if (backPgs != null)
                    {
                        IWebElement backPgsBtn = driver.FindElement(By.ClassName("ut-navigation-button-control"));
                        if (backPgsBtn != null)
                        {
                            backPgsBtn.Click();
                            LogApp($"Arama sayfasına dönme başarılı.", Color.Green);
                            return;
                        }
                    }
                    await Task.Delay(100);
                    loopcounter++;
                }
                LogApp($"Arama sayfasına dönme başarısız. (hata-03)", Color.Red);
            }
            catch (Exception ex)
            {
                LogApp($"Arama sayfasına dönme başarısız. (hata-04)\nDetay:{ex}", Color.Red);
                await DiscordMessage($"Arama sayfasına dönme başarısız. (hata-04)\nDetay:{ex}");
                ErrorCount++;
            }
            return;
        }

        private async Task SearchAction(CancellationToken token) //arama yap
        {
            try
            {
                LogApp($"Arama yapılıyor...");
                if (token.IsCancellationRequested || driver == null)
                {
                    LogApp($"İşlem durdurma komutu alındı.", Color.DarkSalmon);
                    return;
                }
                int loopcounter = 0;
                while (loopcounter < processtimer.Value)
                {
                    IWebElement searchButton = driver.FindElements(By.TagName("button")).FirstOrDefault(header => header.Text == "Search");
                    if (searchButton != null)
                    {
                        searchButton.Click();
                        LogApp($"Arama yapıldı.", Color.Green);
                        return;
                    }
                    await Task.Delay(100);
                    loopcounter++;
                }
                LogApp($"Arama başarısız. (hata-05)", Color.Red); //hata 6

            }
            catch (Exception ex)
            {
                LogApp($"Arama başarısız. (hata-06)\nDetay:{ex}", Color.Red); //hata 7
                await DiscordMessage($"Arama başarısız. (hata-06)\nDetay:{ex}");
                ErrorCount++;
            }
            return;
        }

        private async Task BuyFunction(CancellationToken token, int curprice) //sonuçları kontrol et ve satın al
        {
            LogApp($"Arama sonuçları kontrol ediliyor...");
            if (token.IsCancellationRequested || driver == null)
            {
                LogApp($"İşlem durdurma komutu alındı.", Color.DarkSalmon);
                return;
            }
            int resultCheckCounter = 0;
            bool buyBtnCheck = false;
            bool noResultCheck = false;
            while (resultCheckCounter < processtimer.Value)
            {
                try
                {
                    IWebElement buyBtn = driver.FindElements(By.TagName("button")).FirstOrDefault(el => el.Text.Contains("Buy Now for"));
                    if (buyBtn != null)
                    {
                        buyBtn.Click();
                        buyBtnCheck = true;
                        break;
                    }

                    IWebElement noResult = driver.FindElements(By.TagName("section")).FirstOrDefault(el => el.Text.Contains("No results"));
                    if (noResult != null)
                    {
                        noResultCheck = true;
                        break;
                    }
                }
                catch (StaleElementReferenceException)
                {
                    LogApp("Öğe güncellenmiş, tekrar deneniyor... (Sorun yok.)");
                }
                catch (Exception ex)
                {
                    LogApp($"Sonuç kontrol başarısız. (hata-07)\nDetay:{ex}", Color.Red);
                    await DiscordMessage($"Sonuç kontrol başarısız. (hata-07)\nDetay:{ex}");
                }
                await Task.Delay(100);
                resultCheckCounter++;
            }

            if (buyBtnCheck) LogApp($"Satın alma bulundu.", Color.Green);
            else if (noResultCheck) { LogApp($"Sonuç yok."); return; }

            LogApp($"Satın alma onaylanıyor...");

            if (token.IsCancellationRequested)
            {
                LogApp($"İşlem durdurma komutu alındı.", Color.DarkSalmon);
                return;
            }

            try
            {
                int confirmBtnCounter = 0;
                bool success = false;
                while (confirmBtnCounter < processtimer.Value)
                {
                    IWebElement searchButton = driver.FindElements(By.TagName("button")).FirstOrDefault(header => header.Text == "Ok");
                    if (searchButton != null)
                    {
                        searchButton.Click();
                        success = true;
                        break;
                    }
                    await Task.Delay(100);
                    confirmBtnCounter++;
                }
                if (success)
                {
                    LogApp($"Satın alma onaylandı!", Color.Green);
                    await DiscordMessage($"Yeni bir satın alma tamamlandı. Fiyat: {curprice}");
                    BuyCount++;
                    LogApp($"EAFC Sunucu güncellemesi için 25 saniye bekleniyor...");
                    await Task.Delay((int)(buycdval.Value * 1000));
                }
                else LogApp("Onay kutusu yok. (error-8)", Color.Red);
            }
            catch (Exception ex)
            {
                LogApp($"Satın alma onay kontrolü başarısız. (hata-9)\nDetay:{ex}", Color.Red);
                await DiscordMessage($"Satın alma onay kontrolü başarısız. (hata-9)\nDetay:{ex}");
                ErrorCount++;
            }
        }

        private async Task MainFunction(CancellationToken token)
        {
            LogApp($"Başlatılıyor...");

            var processes = Process.GetProcessesByName("msedgedriver");
            foreach (var process in processes)
            {
                try
                {
                    process.Kill();
                    process.WaitForExit();
                    LogApp($"Eski sürücü sonlandırıldı: {process.Id}");
                }
                catch (Exception ex)
                {
                    LogApp($"İşlem sonlandırılamadı (Sürücü): {process.Id}, Hata: {ex.Message}");
                }
            }

            var options = new EdgeOptions();
            options.DebuggerAddress = "localhost:9111";

            var service = EdgeDriverService.CreateDefaultService();
            service.HideCommandPromptWindow = true;

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
            int minbidval = 0;
            int loopcount = 0;
            int cdloopcount = 0;

            while (true)
            {

                if (driver == null)
                {
                    LogApp($"Driver kapalı, baştan başlatın.");
                    break;
                }

                LogApp($"Mevcut Fiyat: {currentprice}.");
                LogApp($"Döngü Sayısı: {loopcount}.");

                if (cdloopcount == targetloopval.Value)
                {
                    LogApp($"Döngü molası başladı. {loopcdval.Value} saniye.");
                    await Task.Delay((int)(loopcdval.Value * 1000));
                    cdloopcount = 0;
                }

                if (loopcount != 0) // ilk döngü - fiyat değişimi yok - ilk fiyat 300
                {
                    if (currentprice <= maxvalue) currentprice += increment;
                    if (loopcount % 2 == 0) minbidval = 150;
                    else minbidval = 300;
                }

                await PriceSetup(loopcount, currentprice, maxvalue, increment, minbidval, token);

                await SearchAction(token);

                await BuyFunction(token, currentprice);

                await BackPageAction(token);

                loopcount++;
                cdloopcount++;
                LoopCount = loopcount;

                LogApp($"Hız Limiti Koruması - {ratelimitval.Value / 1000} Saniye Bekleme");

                await Task.Delay((int)(ratelimitval.Value));

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
            string userName = System.Security.Principal.WindowsIdentity.GetCurrent().Name;
            string logEntry = $"{DateTime.Now:dd/MM HH:mm} - {userName} | {message}";
            if (ping) logEntry = $"{DateTime.Now:dd/MM HH:mm} - {userName} | @everyone | {message}";
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

        private void prepareEdge_Click(object sender, EventArgs e)
        {
            var processes = Process.GetProcessesByName("msedge");
            foreach (var process in processes)
            {
                try
                {
                    process.Kill();
                    process.WaitForExit();
                    LogApp($"İşlem sonlandırıldı: {process.Id}");
                }
                catch (Exception ex)
                {
                    LogApp($"İşlem sonlandırılamadı: {process.Id}, Hata: {ex.Message}");
                }
            }
            try
            {
                ProcessStartInfo psi = new ProcessStartInfo
                {
                    FileName = @"C:\Program Files (x86)\Microsoft\Edge\Application\msedge.exe",
                    Arguments = "--remote-debugging-port=9111",
                    UseShellExecute = false
                };
                Process.Start(psi);
                LogApp("msedge.exe başarıyla başlatıldı.");
            }
            catch (Exception ex)
            {
                LogApp($"msedge.exe başlatılamadı. Hata: {ex.Message}");
            }
        }
    }
}